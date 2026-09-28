using System;
using System.Collections.Generic;

namespace MyCompiler.Generation
{
    public class MyCodeGenerator
    {
        private readonly MyTargetCodeEmitter emitter;
        private readonly MyInstructionGenerator instructionGenerator;

        public MyCodeGenerator()
        {
            emitter =
                new MyTargetCodeEmitter();

            instructionGenerator =
                new MyInstructionGenerator(
                    emitter
                );
        }

        public string Generate(
            List<string> optimizedCode)
        {
            if (optimizedCode == null)
                return string.Empty;

            foreach (string instruction
                     in optimizedCode)
            {
                instructionGenerator.Generate(
                    instruction
                );
            }

            return emitter.GetCode();
        }

        public string Generate(
            string optimizedCode)
        {
            if (string.IsNullOrWhiteSpace(
                    optimizedCode))
            {
                return string.Empty;
            }

            string[] instructions =
                optimizedCode.Split(
                    new[] { "\r\n", "\n" },
                    StringSplitOptions.RemoveEmptyEntries
                );

            return Generate(
                new List<string>(instructions)
            );
        }
    }
}