using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompiler.Common
{
    public class ErrorHandler
    {
        private List<CompilerError> errors = new List<CompilerError>();

        public void Add(string message, int line, int column)
        {
            errors.Add(new CompilerError(message, line, column));
        }
        public void ClearAll()
        {
            errors.Clear();
        }
        public bool HasErrors()
        {
            return errors.Count > 0;
        }
        public void Print()
        {
            foreach (CompilerError error in errors)
            {
                Console.WriteLine($"Line {error.Line}, Column {error.Column}: {error.Message}");
            }
        }
        public string ReturnError()
        {
            string rr = "";
            foreach (CompilerError error in errors)
            {
                rr += $"Line {error.Line}, Column {error.Column}: {error.Message}\n";
            }

            return rr;
        }
        public List<CompilerError> GetErrors()
        {
            return errors;
        }
    }

    public class CompilerError
    {
        public string Message { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public CompilerError(string message, int line, int column)
        {
            Message = message;
            Line = line;
            Column = column;
        }
    }

}
