using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyForStatement : MyStatement
    {
        public MyStatement? Initializer { get; }

        public MyExpression? Condition { get; }

        public MyExpression? Increment { get; }

        public MyStatement Body { get; }

        public MyForStatement(MyStatement? initializer, MyExpression? condition, MyExpression? increment, MyStatement body)
        {
            Initializer = initializer;
            Condition = condition;
            Increment = increment;
            Body = body;
        }
    }
}
