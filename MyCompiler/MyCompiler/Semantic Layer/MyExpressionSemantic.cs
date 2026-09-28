using MyCompiler.Common;
using MyCompiler.Syntax_Layer.AST;

namespace MyCompiler.Semantic_Layer
{
    public class MyExpressionSemantic
    {
        private readonly SymbolTable symbolTable;
        private readonly ErrorHandler errorHandler;

        public MyExpressionSemantic(SymbolTable symbolTable, ErrorHandler errorHandler)
        {
            this.symbolTable = symbolTable;
            this.errorHandler = errorHandler;
        }

        public TokenType Analyze(MyExpression expression)
        {
            if (expression == null)
                return TokenType.UNKNOWN;

            // ========================================================
            // Literal
            // ========================================================

            if (expression is MyLiteralExpression literal)
            {
                return literal.Value.Contains(".")? TokenType.FLOAT : TokenType.INT;
            }

            // ========================================================
            // Identifier
            // ========================================================

            if (expression is MyIdentifierExpression identifier)
            {
                if (!symbolTable.Exists(identifier.Name))
                {
                    errorHandler.Add($"Semantic Error: Variable '{identifier.Name}' is used without declaration.", 0, 0);

                    return TokenType.UNKNOWN;
                }

                return symbolTable
                    .Get(identifier.Name)
                    .DataType;
            }

            // ========================================================
            // Binary Expression
            // ========================================================

            if (expression is MyBinaryExpression binary)
            {
                TokenType leftType = Analyze(binary.Left);

                TokenType rightType = Analyze(binary.Right);

                if (leftType == TokenType.UNKNOWN || rightType == TokenType.UNKNOWN)
                {
                    return TokenType.UNKNOWN;
                }

                // إذا كان أحد الطرفين FLOAT
                // فالنتيجة FLOAT
                if (leftType == TokenType.FLOAT || rightType == TokenType.FLOAT)
                {
                    return TokenType.FLOAT;
                }

                return TokenType.INT;
            }

            // ========================================================
            // Unary Expression
            // ========================================================

            if (expression is MyUnaryExpression unary)
            {
                return Analyze(unary.Operand);
            }

            return TokenType.UNKNOWN;
        }
    }
}