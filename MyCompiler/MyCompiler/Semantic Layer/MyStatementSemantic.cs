using MyCompiler.Common;
using MyCompiler.Syntax_Layer.AST;

namespace MyCompiler.Semantic_Layer
{
    public class MyStatementSemantic
    {
        private readonly SymbolTable symbolTable;
        private readonly ErrorHandler errorHandler;

        private readonly MyExpressionSemantic expressionSemantic;
        private readonly MyConditionSemantic conditionSemantic;
        private readonly MyTypeChecker typeChecker;

        public MyStatementSemantic(SymbolTable symbolTable, ErrorHandler errorHandler, MyExpressionSemantic expressionSemantic,
                            MyConditionSemantic conditionSemantic, MyTypeChecker typeChecker)
        {
            this.symbolTable = symbolTable;
            this.errorHandler = errorHandler;
            this.expressionSemantic = expressionSemantic;
            this.conditionSemantic = conditionSemantic;
            this.typeChecker = typeChecker;
        }

        public void Analyze(MyStatement statement)
        {
            if (statement == null)
                return;

            switch (statement)
            {
                case MyVariableDeclaration variableDeclaration:
                    AnalyzeVariableDeclaration(variableDeclaration);
                    break;

                case MyAssignment assignment:
                    AnalyzeAssignment(assignment);
                    break;

                case MyIfStatement ifStatement:
                    AnalyzeIf(ifStatement);
                    break;

                case MyWhileStatement whileStatement:
                    AnalyzeWhile(whileStatement);
                    break;

                case MyDoWhileStatement doWhileStatement:
                    AnalyzeDoWhile(doWhileStatement);
                    break;

                case MyForStatement forStatement:
                    AnalyzeFor(forStatement);
                    break;

                case MyBlockStatement block:
                    AnalyzeBlock(block);
                    break;

                case MyExpressionStatement expressionStatement:
                    expressionSemantic.Analyze(expressionStatement.Expression);
                    break;
            }
        }

        // ============================================================
        // Variable Declaration
        // ============================================================

        private void AnalyzeVariableDeclaration(MyVariableDeclaration declaration)
        {
            if (declaration.Initializer == null)
                return;

            TokenType initType = expressionSemantic.Analyze(declaration.Initializer);

            if (initType != TokenType.UNKNOWN && !typeChecker.IsTypeCompatible(declaration.DataType, initType))
            {
                errorHandler.Add($"Semantic Error: Cannot initialize variable '{declaration.Name}' of type '{declaration.DataType}' with value of type '{initType}'.",0, 0);
            }
        }

        // ============================================================
        // Assignment
        // ============================================================

        private void AnalyzeAssignment(MyAssignment assignment)
        {
            if (!symbolTable.Exists(assignment.Name))
            {
                errorHandler.Add($"Semantic Error: Variable '{assignment.Name}' is not declared before assignment.",0,0);

                return;
            }

            Symbol symbol = symbolTable.Get(assignment.Name);

            TokenType valueType = expressionSemantic.Analyze(assignment.Value);

            if (valueType != TokenType.UNKNOWN && !typeChecker.IsTypeCompatible(symbol.DataType, valueType))
            {
                errorHandler.Add($"Semantic Error: Cannot assign value of type '{valueType}' to variable '{assignment.Name}' of type '{symbol.DataType}'.",0,0);
            }
        }

        // ============================================================
        // IF
        // ============================================================

        private void AnalyzeIf(MyIfStatement statement)
        {
            conditionSemantic.Check(statement.Condition,"if");

            Analyze(statement.ThenBranch);

            if (statement.ElseBranch != null)
            {
                Analyze(statement.ElseBranch);
            }
        }

        // ============================================================
        // WHILE
        // ============================================================

        private void AnalyzeWhile(MyWhileStatement statement)
        {
            conditionSemantic.Check(statement.Condition, "while");

            Analyze(statement.Body);
        }

        // ============================================================
        // DO WHILE
        // ============================================================

        private void AnalyzeDoWhile(MyDoWhileStatement statement)
        {
            Analyze(statement.Body);

            conditionSemantic.Check(statement.Condition,"do-while");
        }

        // ============================================================
        // FOR
        // ============================================================

        private void AnalyzeFor(MyForStatement statement)
        {
            if (statement.Initializer != null)
            {
                Analyze(statement.Initializer);
            }

            if (statement.Condition != null)
            {
                conditionSemantic.Check(statement.Condition,"for");
            }

            if (statement.Increment != null)
            {
                expressionSemantic.Analyze(statement.Increment);
            }

            Analyze(statement.Body);
        }

        // ============================================================
        // BLOCK
        // ============================================================

        private void AnalyzeBlock(MyBlockStatement block)
        {
            foreach (var statement in block.Statements)
            {
                Analyze(statement);
            }
        }
    }
}