using MyCompiler.Common;
using MyCompiler.Syntax_Layer.AST;

namespace MyCompiler.Semantic_Layer
{
    public class MyTypeChecker
    {
        public bool IsTypeCompatible(TokenType target, TokenType source)
        {
            if (target == source)
                return true;

            // INT يمكن تحويله إلى FLOAT
            if (target == TokenType.FLOAT && source == TokenType.INT)
            {
                return true;
            }

            return false;
        }
    }
}