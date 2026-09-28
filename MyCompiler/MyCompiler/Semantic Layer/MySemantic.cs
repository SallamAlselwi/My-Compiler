using MyCompiler.Common;
using MyCompiler.Syntax_Layer.AST;

namespace MyCompiler.Semantic_Layer
{
    public class MySemantic
    {
        private readonly MyStatementSemantic statementSemantic;

        public MySemantic(SymbolTable symbolTable,ErrorHandler errorHandler)
        {
            MyExpressionSemantic expressionSemantic = new MyExpressionSemantic(symbolTable,errorHandler);

            MyConditionSemantic conditionSemantic = new MyConditionSemantic(expressionSemantic,errorHandler);

            MyTypeChecker typeChecker = new MyTypeChecker();

            statementSemantic = new MyStatementSemantic(symbolTable, errorHandler, expressionSemantic, conditionSemantic, typeChecker);
        }

        public void Analyze(MyProgramNode program)
        {
            if (program == null)
                return;

            foreach (var statement in program.Statements)
            {
                statementSemantic.Analyze(statement);
            }
        }
    }
}