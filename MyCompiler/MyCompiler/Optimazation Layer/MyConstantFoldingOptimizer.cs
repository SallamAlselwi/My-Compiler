using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MyCompiler.Optimazation_Layer
{
    public class MyConstantFoldingOptimizer
    {
        private readonly Regex arithmeticRegex =
            new Regex(
                @"^\s*(\w+)\s*=\s*(-?\d+(?:\.\d+)?)\s*([\+\-\*/])\s*(-?\d+(?:\.\d+)?)\s*$"
            );

        public List<string> Optimize(List<string> instructions)
        {
            List<string> result = new List<string>();

            foreach (string instruction in instructions)
            {
                Match match = arithmeticRegex.Match(instruction);

                if (!match.Success)
                {
                    result.Add(instruction);
                    continue;
                }

                string target = match.Groups[1].Value;
                string leftText = match.Groups[2].Value;
                string op = match.Groups[3].Value;
                string rightText = match.Groups[4].Value;

                if (!decimal.TryParse(
                        leftText,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out decimal left))
                {
                    result.Add(instruction);
                    continue;
                }

                if (!decimal.TryParse(
                        rightText,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out decimal right))
                {
                    result.Add(instruction);
                    continue;
                }

                decimal value;

                switch (op)
                {
                    case "+":
                        value = left + right;
                        break;

                    case "-":
                        value = left - right;
                        break;

                    case "*":
                        value = left * right;
                        break;

                    case "/":

                        if (right == 0)
                        {
                            result.Add(instruction);
                            continue;
                        }

                        value = left / right;
                        break;

                    default:
                        result.Add(instruction);
                        continue;
                }

                result.Add(
                    $"{target} = {value.ToString(CultureInfo.InvariantCulture)}"
                );
            }

            return result;
        }
    }
}
