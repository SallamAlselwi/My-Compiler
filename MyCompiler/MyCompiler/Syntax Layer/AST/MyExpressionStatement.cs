using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyExpressionStatement : MyStatement
    {
        public MyExpression Expression { get; }

        public MyExpressionStatement(MyExpression expression)
        {
            Expression = expression;
        }
    }
}
