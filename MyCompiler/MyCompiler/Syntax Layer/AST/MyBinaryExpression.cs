using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyBinaryExpression : MyExpression
    {
        public MyExpression Left { get; }

        public TokenType Operator { get; }

        public MyExpression Right { get; }

        public MyBinaryExpression(MyExpression left, TokenType op, MyExpression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}
