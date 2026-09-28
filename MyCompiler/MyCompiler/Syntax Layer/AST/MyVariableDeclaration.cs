using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Syntax_Layer.AST
{
    public class MyVariableDeclaration : MyStatement
    {
        public TokenType DataType { get; }

        public string Name { get; }

        public MyExpression? Initializer { get; }

        public MyVariableDeclaration(TokenType dataType, string name, MyExpression? initializer)
        {
            DataType = dataType;
            Name = name;
            Initializer = initializer;
        }
    }
}
