using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MyCompiler.Optimazation_Layer
{
    public class MyDeadCodeOptimizer
    {
        private readonly Regex tempDefinitionRegex =
            new Regex(
                @"^\s*(t\d+)\s*=\s*(.+?)\s*$"
            );

        private readonly Regex tempUsageRegex =
            new Regex(@"\bt\d+\b");

        public List<string> Optimize(List<string> instructions)
        {
            List<string> current = new List<string>(instructions);

            bool changed;

            do
            {
                changed = false;

                HashSet<string> usedTemps =
                    FindUsedTemps(current);

                List<string> result =
                    new List<string>();

                foreach (string instruction in current)
                {
                    Match match =
                        tempDefinitionRegex.Match(instruction);

                    if (match.Success)
                    {
                        string tempName =
                            match.Groups[1].Value;

                        if (!usedTemps.Contains(tempName))
                        {
                            changed = true;
                            continue;
                        }
                    }

                    result.Add(instruction);
                }

                current = result;

            } while (changed);

            return current;
        }

        private HashSet<string> FindUsedTemps(
            List<string> instructions)
        {
            HashSet<string> used =
                new HashSet<string>();

            foreach (string instruction in instructions)
            {
                MatchCollection matches =
                    tempUsageRegex.Matches(instruction);

                foreach (Match match in matches)
                {
                    used.Add(match.Value);
                }
            }

            // إزالة تعريف المتغير نفسه من الاستخدام
            foreach (string instruction in instructions)
            {
                Match match =
                    tempDefinitionRegex.Match(instruction);

                if (!match.Success)
                    continue;

                string tempName =
                    match.Groups[1].Value;

                string rightSide =
                    match.Groups[2].Value;

                if (!tempUsageRegex.IsMatch(rightSide))
                {
                    // سيتم تحديده من باقي التعليمات
                }
            }

            return used;
        }
    }
}