using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyAssignment : MyStatement
    {
        public string Name { get; }

        public MyExpression Value { get; }

        public MyAssignment(string name, MyExpression value)
        {
            Name = name;
            Value = value;
        }
    }
}
