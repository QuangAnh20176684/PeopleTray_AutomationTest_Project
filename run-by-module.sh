#!/usr/bin/env bash
# ============================================================
# run-playwright-tests.sh
# Runs dotnet test (Playwright) and posts results to MS Teams
# Usage: ./run-playwright-tests.sh [SLN_PATH] [RUNSETTINGS] [WEBHOOK_URL]
# ============================================================
jq_escape() {
    local s="$1"
    s="${s//\\/\\\\}"
    s="${s//\"/\\\"}"
    s="${s//$'\n'/\\n}"
    s="${s//$'\r'/}"
    printf '"%s"' "$s"
}

set -euo pipefail

# ── Config ───────────────────────────────────────────────────
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SLN_PATH="${1:-"${SCRIPT_DIR}/TestX.sln"}"
RUNSETTINGS="${2:-"${SCRIPT_DIR}/.runsettings"}"
FILTER="${3:-"DemoTest"}"
RESULTS_DIR="${SCRIPT_DIR}/playwright-results"
TRX_FILE="${RESULTS_DIR}/TestResult.trx"
WEBHOOK_URL="${4:-"https://nobisoftvn.webhook.office.com/webhookb2/1b0d8698-1bd1-41ad-8260-5a63ff4fc3ae@dfd263e5-5cf1-42c9-947c-2722e7018c6b/IncomingWebhook/da0acfd5c2074ddaa9cd1fd37bfcc450/f8d45a56-b768-42df-86c2-c9e40f8545f4/V2oCTmbzi4hqIoSr5RhmmItF-qRC02XCYGO02oWqi29-01"}"

mkdir -p "$RESULTS_DIR"

# ── 1. Run tests ─────────────────────────────────────────────
echo ""
echo ">> Running Playwright tests..."
echo "   Solution : $SLN_PATH"
echo "   Settings : $RUNSETTINGS"
echo "   Filter   : $FILTER"
echo ""

START_TS=$(date +%s)
START_TIME=$(date "+%Y-%m-%d %H:%M:%S")

dotnet test "$SLN_PATH" \
    --filter "$FILTER" \
    --settings "$RUNSETTINGS" \
    --logger "trx;LogFileName=TestResult.trx" \
    --results-directory "$RESULTS_DIR" || true

EXIT_CODE=$?
END_TS=$(date +%s)
DURATION=$(( END_TS - START_TS ))



# ── 2. Parse TRX ─────────────────────────────────────────────
TOTAL=0; PASSED=0; FAILED=0; SKIPPED=0
FAILED_TESTS_JSON="[]"

if [[ -f "$TRX_FILE" ]]; then
    # 👉 đọc counters an toàn (multi-line)
    COUNTERS_LINE=$(tr -d '\n' < "$TRX_FILE" | grep -o '<Counters[^>]*>' || true)

    extract_attr() {
        echo "$COUNTERS_LINE" | grep -o "${1}=\"[0-9]*\"" | grep -o '[0-9]*' || echo "0"
    }

    TOTAL=$(extract_attr "total")
    PASSED=$(extract_attr "passed")
    FAILED=$(extract_attr "failed")
    NOT_EXEC=$(extract_attr "notExecuted")
    ABORTED=$(extract_attr "aborted")
    SKIPPED=$(( NOT_EXEC + ABORTED ))

    echo ">> Parsing failed tests..."

    FAILED_TESTS_JSON="["
    FIRST=1
    COUNT=0

    # ⚠️ tắt set -e tạm để tránh crash
    set +e

    while IFS= read -r block; do
        [[ $COUNT -ge 10 ]] && break

        # 👉 lấy test name
        test_name=$(echo "$block" \
            | grep -o 'testName="[^"]*"' \
            | sed 's/testName="//;s/"//' \
            | sed 's/.*\.//')

        # 👉 lấy message (multi-line safe)
        message=$(echo "$block" \
            | awk '/<Message>/,/<\/Message>/' \
            | sed '1s/.*<Message>//' \
            | sed '$s/<\/Message>.*//' \
            | tr '\n' ' ' \
            | sed 's/  */ /g' \
            | cut -c1-200)

        entry="{\"name\":$(jq_escape "$test_name"),\"msg\":$(jq_escape "$message")}"

        if [[ $FIRST -eq 1 ]]; then
            FAILED_TESTS_JSON+="$entry"
            FIRST=0
        else
            FAILED_TESTS_JSON+=",$entry"
        fi

        COUNT=$(( COUNT + 1 ))

    done < <(
        awk '
        /<UnitTestResult / {capture=1; block=""}
        capture {block=block $0 "\n"}
        /<\/UnitTestResult>/ {
            if (block ~ /outcome="Failed"/) print block
            capture=0
        }' "$TRX_FILE" 2>/dev/null || true
    )

    set -e

    FAILED_TESTS_JSON+="]"
else
    echo "WARNING: TRX file not found: $TRX_FILE"
fi

# Helper: minimal JSON string escaping
jq_escape() {
    local s="$1"
    s="${s//\\/\\\\}"
    s="${s//\"/\\\"}"
    s="${s//$'\n'/ }"
    s="${s//$'\r'/ }"
    printf '"%s"' "$s"
}

# ── 3. Build Teams message ────────────────────────────────────
if [[ $EXIT_CODE -eq 0 ]]; then
    STATUS_TEXT="PASSED"
    THEME_COLOR="00b050"
else
    STATUS_TEXT="FAILED"
    THEME_COLOR="d93025"
fi

RUN_BY="${USER:-$(whoami)}"
HOSTNAME_VAL=$(hostname)

FACTS=$(cat <<EOF
[
  {"name":"Status",        "value":"${STATUS_TEXT}"},
  {"name":"Total",         "value":"${TOTAL}"},
  {"name":"[OK] Passed",   "value":"${PASSED}"},
  {"name":"[FAIL] Failed", "value":"${FAILED}"},
  {"name":"[SKIP] Skipped","value":"${SKIPPED}"},
  {"name":"Duration",      "value":"${DURATION}s"},
  {"name":"Run by",        "value":"${RUN_BY}@${HOSTNAME_VAL}"},
  {"name":"Time",          "value":"${START_TIME}"}
]
EOF
)

SECTIONS=$(cat <<EOF
[
  {
    "activityTitle":    "PeopleTray Playwright Tests",
    "activitySubtitle": "Solution: $(basename "$SLN_PATH")",
    "facts":            ${FACTS},
    "markdown":         true
  }
]
EOF
)

PAYLOAD=$(cat <<EOF
{
  "@type":    "MessageCard",
  "@context": "https://schema.org/extensions",
  "themeColor": "${THEME_COLOR}",
  "summary":    "Test run ${STATUS_TEXT} - ${PASSED}/${TOTAL} passed",
  "sections":   ${SECTIONS}
}
EOF
)


# Append failed test details section if any

if [[ $FAILED -gt 0 ]]; then
    DETAIL_TEXT=""
    count=0

    set +e

    while IFS= read -r block; do
        [[ $count -ge 10 ]] && break

        # 👉 lấy test name
        tname=$(echo "$block" \
            | grep -o 'testName="[^"]*"' \
            | sed 's/testName="//;s/"//' \
            | sed 's/.*\.//')

        # 👉 lấy message (multi-line safe)
        tmsg=$(echo "$block" \
            | awk '/<Message>/,/<\/Message>/' \
            | sed '1s/.*<Message>//' \
            | sed '$s/<\/Message>.*//' \
            | tr '\n' ' ' \
            | sed 's/  */ /g' \
            | cut -c1-200)
        if [[ "$tmsg" == *"Timeout"* ]]; then
    tmsg="Timeout waiting for element"
fi
        # 👉 build text (escape newline cho JSON)
       
        if [[ -n "$DETAIL_TEXT" ]]; then
    DETAIL_TEXT+="\\n"
fi

DETAIL_TEXT+="- ${tname} - FAILED"

if [[ -n "$tmsg" ]]; then
    DETAIL_TEXT+=" - ${tmsg}"
fi

        count=$(( count + 1 ))

    
    done < <(
    awk '
    /<UnitTestResult / {capture=1; block=""}
    capture {block = block $0 " "}
    /<\/UnitTestResult>/ {
        if (block ~ /outcome="Failed"/) print block
        capture=0
    }' "$TRX_FILE" 2>/dev/null || true
)

    set -e

    if [[ $FAILED -gt 10 ]]; then
        DETAIL_TEXT+="\\n\\n_… and $(( FAILED - 10 )) more failed test(s)_"
    fi
    

    FAILED_SECTION=$(cat <<EOF
,
  {
    "title":    "Failed Tests",
    "text":     "${DETAIL_TEXT}",
    "markdown": true
  }
EOF
)


    # 👉 insert section (giữ logic cũ của bạn)
    PAYLOAD="${PAYLOAD%\}}"
    PAYLOAD=$(echo "$PAYLOAD" | sed 's/\]$//')
    PAYLOAD+="${FAILED_SECTION}]}"
   
fi

# ── 4. Send to Teams ──────────────────────────────────────────
echo ""
echo ">> Sending notification to Teams..."

HTTP_STATUS=$(curl -s -o /tmp/teams_response.txt -w "%{http_code}" \
    -X POST "$WEBHOOK_URL" \
    -H "Content-Type: application/json" \
    -d "$PAYLOAD")

if [[ "$HTTP_STATUS" == "200" ]]; then
    echo "✅ Notification sent (HTTP ${HTTP_STATUS}): $(cat /tmp/teams_response.txt)"
else
    echo "⚠️  Teams notification failed (HTTP ${HTTP_STATUS}): $(cat /tmp/teams_response.txt)"
fi

# ── 5. Exit with test exit code ───────────────────────────────
echo ""
echo "Done. Exit code: ${EXIT_CODE}"
echo ""
exit $EXIT_CODE
