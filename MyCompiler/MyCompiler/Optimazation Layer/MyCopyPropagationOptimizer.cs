using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MyCompiler.Optimazation_Layer
{
    public class MyCopyPropagationOptimizer
    {
        private readonly Dictionary<string, string> values =
            new Dictionary<string, string>();

        private readonly Regex assignmentRegex =
            new Regex(
                @"^\s*(\w+)\s*=\s*(.+?)\s*$"
            );

        private readonly Regex identifierRegex =
            new Regex(@"\b[a-zA-Z_]\w*\b");

        public List<string> Optimize(List<string> instructions)
        {
            values.Clear();

            List<string> result = new List<string>();

            foreach (string instruction in instructions)
            {
                string trimmed = instruction.Trim();

                // عند الوصول إلى Label
                // لا ننشر القيم عبر نقطة تحكم جديدة
                if (trimmed.EndsWith(":"))
                {
                    result.Add(instruction);
                    values.Clear();
                    continue;
                }

                if (trimmed.StartsWith("goto ") ||
                    trimmed.StartsWith("ifFalse "))
                {
                    result.Add(
                        ReplaceKnownValues(instruction)
                    );

                    values.Clear();
                    continue;
                }

                Match match = assignmentRegex.Match(instruction);

                if (!match.Success)
                {
                    result.Add(instruction);
                    continue;
                }

                string target = match.Groups[1].Value;
                string expression = match.Groups[2].Value;

                // الهدف تغيرت قيمته، لذلك نحذف القيمة القديمة
                values.Remove(target);

                expression = ReplaceKnownValues(expression);

                result.Add($"{target} = {expression}");

                string simpleValue = expression.Trim();

                // x = y
                if (IsIdentifier(simpleValue))
                {
                    values[target] = simpleValue;
                }
                // x = 10
                else if (IsConstant(simpleValue))
                {
                    values[target] = simpleValue;
                }
            }

            return result;
        }

        private string ReplaceKnownValues(string expression)
        {
            return identifierRegex.Replace(
                expression,
                match =>
                {
                    string name = match.Value;

                    if (name == "goto" || name == "ifFalse")
                        return name;

                    if (values.TryGetValue(name, out string value))
                        return value;

                    return name;
                });
        }

        private bool IsIdentifier(string value)
        {
            return Regex.IsMatch(
                value,
                @"^[a-zA-Z_]\w*$"
            );
        }

        private bool IsConstant(string value)
        {
            return decimal.TryParse(
                value,
                out _
            );
        }
    }
}