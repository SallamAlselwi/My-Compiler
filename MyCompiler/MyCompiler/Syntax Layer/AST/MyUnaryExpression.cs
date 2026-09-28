using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyUnaryExpression : MyExpression
    {
        public TokenType Operator { get; }

        public MyExpression Operand { get; }

        public MyUnaryExpression(TokenType op, MyExpression operand)
        {
            Operator = op;
            Operand = operand;
        }
    }
}
