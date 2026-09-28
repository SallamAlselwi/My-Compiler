using MyCompiler.Common;
using MyCompiler.Syntax_Layer.AST;

namespace MyCompiler.ICG
{
    public class MyExpressionGenerator
    {
        private readonly MyInstructionEmitter emitter;
        private readonly MyTempGenerator tempGenerator;
        private readonly ErrorHandler errorHandler;

        public MyExpressionGenerator(MyInstructionEmitter emitter,MyTempGenerator tempGenerator,ErrorHandler errorHandler)
        {
            this.emitter = emitter;
            this.tempGenerator = tempGenerator;
            this.errorHandler = errorHandler;
        }

        public string Generate(MyExpression expression)
        {
            if (expression == null)
                return string.Empty;

            // ========================================================
            // Literal
            // ========================================================

            if (expression is MyLiteralExpression literal)
            {
                return literal.Value;
            }

            // ========================================================
            // Identifier
            // ========================================================

            if (expression is MyIdentifierExpression identifier)
            {
                return identifier.Name;
            }

            // ========================================================
            // Binary Expression
            // ========================================================

            if (expression is MyBinaryExpression binary)
            {
                string left = Generate(binary.Left);

                string right = Generate(binary.Right);

                string temp = tempGenerator.NewTemp();

                string operation = GetOperator(binary.Operator);

                emitter.Emit(
                    $"{temp} = {left} {operation} {right}");

                return temp;
            }

            // ========================================================
            // Unary Expression
            // ========================================================

            if (expression is MyUnaryExpression unary)
            {
                string operand = Generate(unary.Operand);

                string operation = GetOperator(unary.Operator);

                string temp = tempGenerator.NewTemp();

                emitter.Emit(
                    $"{temp} = {operation}{operand}");

                return temp;
            }

            errorHandler.Add(
                $"Unsupported expression type '{expression.GetType().Name}'.",
                0,
                0);

            return string.Empty;
        }

        private string GetOperator(TokenType operatorType)
        {
            switch (operatorType)
            {
                case TokenType.PLUS:
                    return "+";

                case TokenType.MINUS:
                    return "-";

                case TokenType.MULTIPLY:
                    return "*";

                case TokenType.DIVIDE:
                    return "/";

                case TokenType.LESS:
                    return "<";

                case TokenType.GREATER:
                    return ">";

                case TokenType.EQUAL:
                    return "==";

                case TokenType.LESSEQUAL:
                    return "<=";

                case TokenType.GREATEREQUAL:
                    return ">=";

                case TokenType.NOTEQUAL:
                    return "!=";

                case TokenType.INCREMENT:
                    return "++";

                case TokenType.DECREMENT:
                    return "--";

                default:
                    return operatorType.ToString();
            }
        }
    }
}