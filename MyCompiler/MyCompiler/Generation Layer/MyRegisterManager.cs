using System.Collections.Generic;

namespace MyCompiler.Generation
{
    public class MyRegisterManager
    {
        private readonly Queue<string> availableRegisters;

        public MyRegisterManager()
        {
            availableRegisters =
                new Queue<string>();

            availableRegisters.Enqueue("R1");
            availableRegisters.Enqueue("R2");
            availableRegisters.Enqueue("R3");
            availableRegisters.Enqueue("R4");
        }

        public string GetRegister()
        {
            if (availableRegisters.Count == 0)
            {
                return "R1";
            }

            return availableRegisters.Dequeue();
        }

        public void ReleaseRegister(string register)
        {
            if (!availableRegisters.Contains(register))
            {
                availableRegisters.Enqueue(register);
            }
        }
    }
}