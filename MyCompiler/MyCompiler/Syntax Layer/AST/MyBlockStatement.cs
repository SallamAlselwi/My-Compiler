using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyBlockStatement : MyStatement
    {
        public List<MyStatement> Statements { get; }

        public MyBlockStatement()
        {
            Statements = new List<MyStatement>();
        }
    }
}
