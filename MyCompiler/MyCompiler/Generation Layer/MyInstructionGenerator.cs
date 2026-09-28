using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MyCompiler.Generation
{
    public class MyInstructionGenerator
    {
        private readonly MyTargetCodeEmitter emitter;

        private readonly Dictionary<string, string> locations;

        private readonly Regex assignmentRegex = new Regex(@"^\s*(\w+)\s*=\s*(.+?)\s*$" );

        private readonly Regex binaryRegex = 
                            new Regex(@"^\s*(\w+)\s*=\s*(\w+|-?\d+(?:\.\d+)?)\s*([\+\-\*/])\s*(\w+|-?\d+(?:\.\d+)?)\s*$");

        private readonly Regex comparisonRegex =
                            new Regex(@"^\s*(\w+)\s*=\s*(\w+|-?\d+(?:\.\d+)?)\s*(<=|>=|==|!=|<|>)\s*(\w+|-?\d+(?:\.\d+)?)\s*$");
        public MyInstructionGenerator(MyTargetCodeEmitter emitter)
        {
            this.emitter = emitter;

            locations =
                new Dictionary<string, string>();
        }

        public void Generate(string instruction)
        {
            if (string.IsNullOrWhiteSpace(instruction))
                return;

            instruction =
                instruction.Trim();

            // Label
            if (instruction.EndsWith(":"))
            {
                GenerateLabel(instruction);
                return;
            }

            // Conditional jump
            if (instruction.StartsWith("ifFalse "))
            {
                GenerateIfFalse(instruction);
                return;
            }

            // GOTO
            if (instruction.StartsWith("goto "))
            {
                GenerateGoto(instruction);
                return;
            }

            Match comparisonMatch = comparisonRegex.Match(instruction);

            if (comparisonMatch.Success)
            {
                GenerateComparison(
                    comparisonMatch.Groups[1].Value,
                    comparisonMatch.Groups[2].Value,
                    comparisonMatch.Groups[3].Value,
                    comparisonMatch.Groups[4].Value
                );

                return;
            }

            // Assignment / expression
            Match binaryMatch = binaryRegex.Match(instruction);

            if (binaryMatch.Success)
            {
                GenerateBinaryExpression(binaryMatch);
                return;
            }

            Match assignmentMatch =
                assignmentRegex.Match(instruction);

            if (assignmentMatch.Success)
            {
                GenerateAssignment(assignmentMatch);
            }
        }

        private void GenerateBinaryExpression(
            Match match)
        {
            string target =
                match.Groups[1].Value;

            string left =
                match.Groups[2].Value;

            string op =
                match.Groups[3].Value;

            string right =
                match.Groups[4].Value;

            string targetRegister =
                GetLocation(target);

            emitter.Emit(
                $"MOV {targetRegister}, {GetLocation(left)}"
            );

            switch (op)
            {
                case "+":
                    emitter.Emit(
                        $"ADD {targetRegister}, {GetLocation(right)}"
                    );
                    break;

                case "-":
                    emitter.Emit(
                        $"SUB {targetRegister}, {GetLocation(right)}"
                    );
                    break;

                case "*":
                    emitter.Emit(
                        $"MUL {targetRegister}, {GetLocation(right)}"
                    );
                    break;

                case "/":
                    emitter.Emit(
                        $"DIV {targetRegister}, {GetLocation(right)}"
                    );
                    break;
            }
        }

        private void GenerateAssignment(
            Match match)
        {
            string target =
                match.Groups[1].Value;

            string value =
                match.Groups[2].Value.Trim();

            string targetRegister =
                GetLocation(target);

            emitter.Emit(
                $"MOV {targetRegister}, {GetLocation(value)}"
            );
        }

        private void GenerateLabel(
            string instruction)
        {
            string label =
                instruction.TrimEnd(':');

            emitter.Emit($"{label}:");
        }

        private void GenerateGoto(
            string instruction)
        {
            string label =
                instruction.Substring(5).Trim();

            emitter.Emit(
                $"JMP {label}"
            );
        }

        private void GenerateIfFalse(
            string instruction)
        {
            string content =
                instruction.Substring(8).Trim();

            string[] parts =
                content.Split(
                    new[] { "goto" },
                    StringSplitOptions.RemoveEmptyEntries
                );

            if (parts.Length != 2)
                return;

            string condition =
                parts[0].Trim();

            string label =
                parts[1].Trim();

            emitter.Emit(
                $"CMP {GetLocation(condition)}, 0"
            );

            emitter.Emit(
                $"JE {label}"
            );
        }

        private string GetLocation(
            string value)
        {
            value = value.Trim();

            // رقم
            if (decimal.TryParse(
                    value,
                    out _))
            {
                return value;
            }

            // موجود مسبقاً
            if (locations.TryGetValue(
                    value,
                    out string location))
            {
                return location;
            }

            // إنشاء Register جديد
            string register =
                CreateRegister();

            locations[value] =
                register;

            return register;
        }

        private int registerCounter = 1;

        private string CreateRegister()
        {
            string register =
                $"R{registerCounter}";

            registerCounter++;

            if (registerCounter > 4)
                registerCounter = 1;

            return register;
        }

        private void GenerateComparison(string target, string left, string op, string right)
        {
            string targetRegister = GetLocation(target);

            emitter.Emit($"CMP {GetLocation(left)}, {GetLocation(right)}");

            switch (op)
            {
                case "<":
                    emitter.Emit(
                        $"SETL {targetRegister}"
                    );
                    break;

                case ">":
                    emitter.Emit(
                        $"SETG {targetRegister}"
                    );
                    break;

                case "==":
                    emitter.Emit(
                        $"SETE {targetRegister}"
                    );
                    break;

                case "!=":
                    emitter.Emit(
                        $"SETNE {targetRegister}"
                    );
                    break;

                case "<=":
                    emitter.Emit(
                        $"SETLE {targetRegister}"
                    );
                    break;

                case ">=":
                    emitter.Emit(
                        $"SETGE {targetRegister}"
                    );
                    break;
            }
        }
    }
}