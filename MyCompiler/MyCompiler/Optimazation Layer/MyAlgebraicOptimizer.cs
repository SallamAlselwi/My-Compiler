using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MyCompiler.Optimazation_Layer
{
    public class MyAlgebraicOptimizer
    {
        private readonly Regex expressionRegex =
            new Regex(
                @"^\s*(\w+)\s*=\s*(\w+|\-?\d+(?:\.\d+)?)\s*([\+\-\*/])\s*(\w+|\-?\d+(?:\.\d+)?)\s*$"
            );

        public List<string> Optimize(List<string> instructions)
        {
            List<string> result = new List<string>();

            foreach (string instruction in instructions)
            {
                Match match = expressionRegex.Match(instruction);

                if (!match.Success)
                {
                    result.Add(instruction);
                    continue;
                }

                string target = match.Groups[1].Value;
                string left = match.Groups[2].Value;
                string op = match.Groups[3].Value;
                string right = match.Groups[4].Value;

                string optimized = OptimizeExpression(
                    target,
                    left,
                    op,
                    right
                );

                result.Add(optimized);
            }

            return result;
        }

        private string OptimizeExpression(
            string target,
            string left,
            string op,
            string right)
        {
            switch (op)
            {
                case "+":
                    if (right == "0")
                        return $"{target} = {left}";

                    if (left == "0")
                        return $"{target} = {right}";
                    break;

                case "-":
                    if (right == "0")
                        return $"{target} = {left}";
                    break;

                case "*":
                    if (right == "1")
                        return $"{target} = {left}";

                    if (left == "1")
                        return $"{target} = {right}";

                    if (right == "0" || left == "0")
                        return $"{target} = 0";
                    break;

                case "/":
                    if (right == "1")
                        return $"{target} = {left}";
                    break;
            }

            return $"{target} = {left} {op} {right}";
        }
    }
}