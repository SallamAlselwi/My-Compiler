using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyIfStatement : MyStatement
    {
        public TokenType Type { get; }
        public MyExpression Condition { get; }

        public MyStatement ThenBranch { get; }

        public MyStatement? ElseBranch { get; }

        public MyIfStatement(TokenType tokenType, MyExpression condition, MyStatement thenBranch, MyStatement? elseBranch)
        {
            Type = tokenType;
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }
    }
}
