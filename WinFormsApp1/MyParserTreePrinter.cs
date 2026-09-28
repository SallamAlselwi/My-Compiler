using System.Text;
using MyCompiler;
using MyCompiler.Common;
using MyCompiler.Lexical_Layer;
using MyCompiler.Syntax_Layer;
using MyCompiler.Syntax_Layer.AST;

namespace WinFormsApp1
{
    public class MyParserTreePrinter
    {
        public string Print(MyProgramNode program)
        {
            StringBuilder tree = new StringBuilder();

            if (program == null)
                return "";

            tree.AppendLine("Program");

            for (int i = 0; i < program.Statements.Count; i++)
            {
                bool isLast = i == program.Statements.Count - 1;

                PrintStatement(
                    program.Statements[i],
                    tree,
                    "",
                    isLast
                );
            }

            return tree.ToString();
        }

        private void PrintStatement(
            MyStatement statement,
            StringBuilder tree,
            string indent,
            bool isLast)
        {
            string branch = isLast ? "└── " : "├── ";

            switch (statement)
            {
                case MyVariableDeclaration varDecl:
                    tree.AppendLine(
                        indent + branch +
                        $"VariableDeclaration: {varDecl.DataType} {varDecl.Name}"
                    );

                    if (varDecl.Initializer != null)
                    {
                        PrintExpression(
                            varDecl.Initializer,
                            tree,
                            indent + (isLast ? "    " : "│   "),
                            true
                        );
                    }
                    break;

                case MyAssignment assignment:
                    tree.AppendLine(
                        indent + branch +
                        $"Assignment: {assignment.Name}"
                    );

                    PrintExpression(
                        assignment.Value,
                        tree,
                        indent + (isLast ? "    " : "│   "),
                        true
                    );
                    break;

                case MyExpressionStatement exprStmt:
                    tree.AppendLine(
                        indent + branch +
                        "ExpressionStatement"
                    );

                    PrintExpression(
                        exprStmt.Expression,
                        tree,
                        indent + (isLast ? "    " : "│   "),
                        true
                    );
                    break;

                case MyIfStatement ifStmt:
                    tree.AppendLine(
                        indent + branch + "IfStatement"
                    );

                    tree.AppendLine(
                        indent + (isLast ? "    " : "│   ") +
                        "├── Condition"
                    );

                    PrintExpression(
                        ifStmt.Condition,
                        tree,
                        indent + (isLast ? "    │   " : "│   │   "),
                        true
                    );

                    tree.AppendLine(
                        indent + (isLast ? "    " : "│   ") +
                        "├── Then"
                    );

                    PrintStatement(
                        ifStmt.ThenBranch,
                        tree,
                        indent + (isLast ? "    │   " : "│   │   "),
                        ifStmt.ElseBranch == null
                    );

                    if (ifStmt.ElseBranch != null)
                    {
                        tree.AppendLine(
                            indent + (isLast ? "    " : "│   ") +
                            "└── Else"
                        );

                        PrintStatement(
                            ifStmt.ElseBranch,
                            tree,
                            indent + (isLast ? "        " : "    │   "),
                            true
                        );
                    }

                    break;

                case MyWhileStatement whileStmt:
                    tree.AppendLine(
                        indent + branch + "WhileStatement"
                    );

                    tree.AppendLine(
                        indent + (isLast ? "    " : "│   ") +
                        "├── Condition"
                    );

                    PrintExpression(
                        whileStmt.Condition,
                        tree,
                        indent + (isLast ? "    │   " : "│   │   "),
                        true
                    );

                    tree.AppendLine(
                        indent + (isLast ? "    " : "│   ") +
                        "└── Body"
                    );

                    PrintStatement(
                        whileStmt.Body,
                        tree,
                        indent + (isLast ? "        " : "    │   "),
                        true
                    );

                    break;

                case MyDoWhileStatement doWhileStmt:
                    tree.AppendLine(
                        indent + branch + "DoWhileStatement"
                    );

                    tree.AppendLine(
                        indent + (isLast ? "    " : "│   ") +
                        "├── Body"
                    );

                    PrintStatement(
                        doWhileStmt.Body,
                        tree,
                        indent + (isLast ? "    │   " : "│   │   "),
                        true
                    );

                    tree.AppendLine(
                        indent + (isLast ? "    " : "│   ") +
                        "└── Condition"
                    );

                    PrintExpression(
                        doWhileStmt.Condition,
                        tree,
                        indent + (isLast ? "        " : "    │   "),
                        true
                    );

                    break;

                case MyForStatement forStmt:
                    tree.AppendLine(
                        indent + branch + "ForStatement"
                    );

                    if (forStmt.Initializer != null)
                    {
                        tree.AppendLine(
                            indent + (isLast ? "    " : "│   ") +
                            "├── Initializer"
                        );

                        PrintStatement(
                            forStmt.Initializer,
                            tree,
                            indent + (isLast ? "    │   " : "│   │   "),
                            true
                        );
                    }

                    if (forStmt.Condition != null)
                    {
                        tree.AppendLine(
                            indent + (isLast ? "    " : "│   ") +
                            "├── Condition"
                        );

                        PrintExpression(
                            forStmt.Condition,
                            tree,
                            indent + (isLast ? "    │   " : "│   │   "),
                            true
                        );
                    }

                    if (forStmt.Increment != null)
                    {
                        tree.AppendLine(
                            indent + (isLast ? "    " : "│   ") +
                            "├── Increment"
                        );

                        PrintExpression(
                            forStmt.Increment,
                            tree,
                            indent + (isLast ? "    │   " : "│   │   "),
                            true
                        );
                    }

                    if (forStmt.Body != null)
                    {
                        tree.AppendLine(
                            indent + (isLast ? "    " : "│   ") +
                            "└── Body"
                        );

                        PrintStatement(
                            forStmt.Body,
                            tree,
                            indent + (isLast ? "        " : "    │   "),
                            true
                        );
                    }

                    break;

                case MyBlockStatement block:
                    tree.AppendLine(
                        indent + branch + "Block"
                    );

                    for (int i = 0; i < block.Statements.Count; i++)
                    {
                        PrintStatement(
                            block.Statements[i],
                            tree,
                            indent + (isLast ? "    " : "│   "),
                            i == block.Statements.Count - 1
                        );
                    }

                    break;
            }
        }

        private void PrintExpression(
            MyExpression expression,
            StringBuilder tree,
            string indent,
            bool isLast)
        {
            string branch = isLast ? "└── " : "├── ";

            switch (expression)
            {
                case MyLiteralExpression literal:
                    tree.AppendLine(
                        indent + branch +
                        $"Literal: {literal.Value}"
                    );
                    break;

                case MyIdentifierExpression identifier:
                    tree.AppendLine(
                        indent + branch +
                        $"Identifier: {identifier.Name}"
                    );
                    break;

                case MyBinaryExpression binary:
                    tree.AppendLine(
                        indent + branch +
                        $"BinaryExpression: {binary.Operator}"
                    );

                    tree.AppendLine(
                        indent + (isLast ? "    " : "│   ") +
                        "├── Left"
                    );

                    PrintExpression(
                        binary.Left,
                        tree,
                        indent + (isLast ? "    │   " : "│   │   "),
                        true
                    );

                    tree.AppendLine(
                        indent + (isLast ? "    " : "│   ") +
                        "└── Right"
                    );

                    PrintExpression(
                        binary.Right,
                        tree,
                        indent + (isLast ? "        " : "    │   "),
                        true
                    );

                    break;

                case MyUnaryExpression unary:
                    tree.AppendLine(
                        indent + branch +
                        $"UnaryExpression: {unary.Operator}"
                    );

                    PrintExpression(
                        unary.Operand,
                        tree,
                        indent + (isLast ? "    " : "│   "),
                        true
                    );

                    break;
            }
        }
    }
}