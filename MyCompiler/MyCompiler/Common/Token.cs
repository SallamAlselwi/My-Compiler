using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompiler.Common
{
    public class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public int Line { get; }
        public int Column { get; }

        public Token(TokenType type, string lexeme, int line, int column)
        {
            Type = type;
            Lexeme = lexeme;
            Line = line;
            Column = column;
        }
    }
}
