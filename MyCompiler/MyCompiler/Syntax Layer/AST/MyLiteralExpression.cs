using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyLiteralExpression : MyExpression
    {
        public string Value { get; }

        public MyLiteralExpression(string value)
        {
            Value = value;
        }
    }
}
