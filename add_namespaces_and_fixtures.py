import os
import re

src_dir = "src"
for root, dirs, files in os.walk(src_dir):
    for file in files:
        if file.endswith(".cs"):
            filePath = os.path.join(root, file)
            with open(filePath, 'r', encoding='utf-8') as f:
                content = f.read()
            
            # 1. Add namespace if not present
            if "namespace " not in content:
                lines = content.split('\n')
                out_lines = []
                added_namespace = False
                in_usings = True
                
                has_usings = any(l.startswith("using ") for l in lines)
                if not has_usings:
                    out_lines.append("namespace PeoTest;")
                    out_lines.append("")
                    added_namespace = True

                for line in lines:
                    if in_usings and has_usings:
                        if line.startswith("using ") or line.strip() == "":
                            out_lines.append(line)
                        else:
                            if not added_namespace:
                                out_lines.append("")
                                out_lines.append("namespace PeoTest;")
                                out_lines.append("")
                                added_namespace = True
                            out_lines.append(line)
                            in_usings = False
                    else:
                        out_lines.append(line)
                
                content = '\n'.join(out_lines)
            
            # 2. Add [TestFixture] to test classes
            if (" : CommonBaseTest" in content or "class CommonBaseTest" in content) and "[TestFixture]" not in content:
                content = re.sub(r'(\s*)(public class\s+\w+)', r'\1[TestFixture]\1\2', content)

            with open(filePath, 'w', encoding='utf-8') as f:
                f.write(content)

print("Done updating .cs files.")
