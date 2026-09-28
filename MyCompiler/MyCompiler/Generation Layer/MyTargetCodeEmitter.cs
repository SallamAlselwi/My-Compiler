using System.Collections.Generic;
using System.Text;

namespace MyCompiler.Generation
{
    public class MyTargetCodeEmitter
    {
        private readonly List<string> instructions;

        public MyTargetCodeEmitter()
        {
            instructions = new List<string>();
        }

        public void Emit(string instruction)
        {
            instructions.Add(instruction);
        }

        public string GetCode()
        {
            StringBuilder result = new StringBuilder();

            foreach (string instruction in instructions)
            {
                result.AppendLine(instruction);
            }

            return result.ToString();
        }

        public List<string> GetInstructions()
        {
            return new List<string>(instructions);
        }
    }
}