using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompiler.Common
{
    public enum TokenType
    {
        // Keywords
        IF,
        ELSE,
        WHILE,
        DO,
        FOR,

        //Data Type
        INT,
        FLOAT,

        // Identifier
        IDENTIFIER,

        // Numbers
        INTEGER,

        //Text
        //STRINGS,


        // Operators
        ASSIGN,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,

        LESS,
        GREATER,
        LESSEQUAL,
        GREATEREQUAL,
        EQUAL,
        NOTEQUAL,

        INCREMENT,
        DECREMENT,

        // Symbols
        LPAREN,
        RPAREN,

        LBRACE, // {
        RBRACE, // }

        SEMICOLON,
        COMMA,

        UNKNOWN,
        EOF
    }
}
