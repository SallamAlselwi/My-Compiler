using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyWhileStatement : MyStatement
    {
        public TokenType Type { get; }
        public MyExpression Condition { get; }

        public MyStatement Body { get; }

        public MyWhileStatement(TokenType type, MyExpression condition, MyStatement body)
        {
            Type = type;
            Condition = condition;
            Body = body;
        }
    }
}
