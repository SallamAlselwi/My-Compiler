using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyIdentifierExpression : MyExpression
    {
        public string Name { get; }

        public MyIdentifierExpression(string name)
        {
            Name = name;
        }
    }
}
