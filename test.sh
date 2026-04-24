#!/usr/bin/env bash
set -euo pipefail

# ── Config ───────────────────────────────────────────────────
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SLN_PATH="${1:-"${SCRIPT_DIR}/TestX.sln"}"
RUNSETTINGS="${2:-"${SCRIPT_DIR}/.runsettings"}"
FILTER="${3:-"DemoTest"}"
RESULTS_DIR="${SCRIPT_DIR}/playwright-results"
TRX_FILE="${RESULTS_DIR}/TestResult.trx"
WEBHOOK_URL="${4:-"YOUR_WEBHOOK_URL"}"

mkdir -p "$RESULTS_DIR"

# ── 1. Run tests ─────────────────────────────────────────────
echo ">> Running Playwright tests..."

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
DETAIL_TEXT=""

if [[ -f "$TRX_FILE" ]]; then
    echo ">> Parsing TRX..."

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

    set +e
    count=0

    while IFS= read -r block; do
        [[ $count -ge 10 ]] && break

        tname=$(echo "$block" | sed -n 's:.*testName="\([^"]*\)".*:\1:p' | sed 's/.*\.//')

        tmsg=$(echo "$block" \
            | awk '/<Message>/,/<\/Message>/' \
            | sed '1s/.*<Message>//' \
            | sed '$s/<\/Message>.*//' \
            | tr '\n' ' ' \
            | sed 's/  */ /g' \
            | cut -c1-200)

        [[ -n "$DETAIL_TEXT" ]] && DETAIL_TEXT+=$'\n'
        DETAIL_TEXT+="- ${tname} - FAILED"
        [[ -n "$tmsg" ]] && DETAIL_TEXT+=" - ${tmsg}"

        count=$((count+1))

    done < <(
        awk '
        /<UnitTestResult / {capture=1; block=""}
        capture {block = block $0 " "}
        /<\/UnitTestResult>/ {
            if (block ~ /outcome="Failed"/) print block
            capture=0
        }' "$TRX_FILE" || true
    )

    set -e

    if [[ $FAILED -gt 10 ]]; then
        DETAIL_TEXT+=$'\n... and more failed tests'
    fi
else
    echo "WARNING: TRX file not found"
fi

# ── 3. Build JSON bằng jq ─────────────────────────────────────
STATUS_TEXT=$([[ $EXIT_CODE -eq 0 ]] && echo "PASSED" || echo "FAILED")
THEME_COLOR=$([[ $EXIT_CODE -eq 0 ]] && echo "00b050" || echo "d93025")

RUN_BY="${USER:-$(whoami)}"
HOSTNAME_VAL=$(hostname)

echo ">> Building Teams payload..."

PAYLOAD=$(jq -n \
  --arg status "$STATUS_TEXT" \
  --arg total "$TOTAL" \
  --arg passed "$PASSED" \
  --arg failed "$FAILED" \
  --arg skipped "$SKIPPED" \
  --arg duration "${DURATION}s" \
  --arg runby "${RUN_BY}@${HOSTNAME_VAL}" \
  --arg time "$START_TIME" \
  --arg solution "$(basename "$SLN_PATH")" \
  --arg detail "$DETAIL_TEXT" \
  --arg color "$THEME_COLOR" \
'
{
  "@type": "MessageCard",
  "@context": "https://schema.org/extensions",
  "themeColor": $color,
  "summary": ("Test run " + $status),
  "sections": (
    [
      {
        activityTitle: "Playwright Test Report",
        activitySubtitle: ("Solution: " + $solution),
        facts: [
          {name:"Status", value:$status},
          {name:"Total", value:$total},
          {name:"Passed", value:$passed},
          {name:"Failed", value:$failed},
          {name:"Skipped", value:$skipped},
          {name:"Duration", value:$duration},
          {name:"Run by", value:$runby},
          {name:"Time", value:$time}
        ],
        markdown: true
      }
    ]
    +
    (if ($failed|tonumber) > 0 then
      [
        {
          title: "Failed Tests",
          text: $detail,
          markdown: true
        }
      ]
    else [] end)
  )
}
')

# ── 4. Send to Teams ──────────────────────────────────────────
echo ">> Sending to Teams..."

HTTP_STATUS=$(curl -s -o /tmp/teams_response.txt -w "%{http_code}" \
    -X POST "$WEBHOOK_URL" \
    -H "Content-Type: application/json" \
    -d "$PAYLOAD")

echo "HTTP_STATUS = $HTTP_STATUS"
echo "RESPONSE = $(cat /tmp/teams_response.txt)"

if [[ "$HTTP_STATUS" == "200" ]]; then
    echo "✅ Sent successfully"
else
    echo "❌ Failed"
fi

# ── 5. Exit ───────────────────────────────────────────────────
exit $EXIT_CODE