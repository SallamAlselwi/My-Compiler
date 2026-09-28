using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyDoWhileStatement : MyStatement
    {
        public TokenType TypeDo { get; }
        public MyStatement Body { get; }
        public TokenType TypeWhile { get; }

        public MyExpression Condition { get; }

        public MyDoWhileStatement(TokenType typeDo, MyStatement body, TokenType typeWhile, MyExpression condition)
        {
            TypeDo = typeDo;
            Body = body;
            TypeWhile = typeWhile;
            Condition = condition;
        }
    }
}
