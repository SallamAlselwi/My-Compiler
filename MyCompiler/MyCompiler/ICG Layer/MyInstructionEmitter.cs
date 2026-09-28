using System.Collections.Generic;
using System.Text;

namespace MyCompiler.ICG
{
    public class MyInstructionEmitter
    {
        private readonly List<string> instructions;

        public MyInstructionEmitter()
        {
            instructions = new List<string>();
        }

        public void Emit(string instruction)
        {
            instructions.Add(instruction);
        }

        public List<string> GetInstructions()
        {
            return new List<string>(instructions);
        }

        public string GetCode()
        {
            StringBuilder builder = new StringBuilder();

            foreach (string instruction in instructions)
            {
                builder.AppendLine(instruction);
            }

            return builder.ToString();
        }

        public void Clear()
        {
            instructions.Clear();
        }
    }
}