using MyCompiler.Common;
using MyCompiler.Syntax_Layer.AST;

namespace MyCompiler.Semantic_Layer
{
    public class MyConditionSemantic
    {
        private readonly MyExpressionSemantic expressionSemantic;
        private readonly ErrorHandler errorHandler;

        public MyConditionSemantic(MyExpressionSemantic expressionSemantic, ErrorHandler errorHandler)
        {
            this.expressionSemantic = expressionSemantic;
            this.errorHandler = errorHandler;
        }

        public void Check(MyExpression condition, string statementType)
        {
            if (condition == null)
                return;

            if (condition is MyBinaryExpression binary)
            {
                bool isRelational =
                    binary.Operator == TokenType.LESS ||
                    binary.Operator == TokenType.GREATER ||
                    binary.Operator == TokenType.EQUAL ||
                    binary.Operator == TokenType.LESSEQUAL ||
                    binary.Operator == TokenType.GREATEREQUAL ||
                    binary.Operator == TokenType.NOTEQUAL;

                if (!isRelational)
                {
                    errorHandler.Add($"Semantic Warning: Condition in '{statementType}' statement is arithmetic rather than logical/relational.",0,0);
                }

                expressionSemantic.Analyze(condition);
            }
            else
            {
                errorHandler.Add($"Semantic Warning: Condition expression in '{statementType}' should evaluate to a boolean statement.",0,0);

                expressionSemantic.Analyze(condition);
            }
        }
    }
}