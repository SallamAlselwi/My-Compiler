using MyCompiler.Common;
using MyCompiler.Syntax_Layer.AST;

namespace MyCompiler.ICG
{
    public class MyStatementGenerator
    {
        private readonly MyInstructionEmitter emitter;
        private readonly MyExpressionGenerator expressionGenerator;
        private readonly MyLabelGenerator labelGenerator;
        private readonly ErrorHandler errorHandler;

        public MyStatementGenerator(MyInstructionEmitter emitter, MyExpressionGenerator expressionGenerator,
                    MyLabelGenerator labelGenerator, ErrorHandler errorHandler)
        {
            this.emitter = emitter;
            this.expressionGenerator = expressionGenerator;
            this.labelGenerator = labelGenerator;
            this.errorHandler = errorHandler;
        }

        public void Generate(MyStatement statement)
        {
            if (statement == null)
                return;

            switch (statement)
            {
                case MyVariableDeclaration varDecl:
                    GenerateVariableDeclaration(varDecl);
                    break;

                case MyAssignment assignment:
                    GenerateAssignment(assignment);
                    break;

                case MyIfStatement ifStatement:
                    GenerateIf(ifStatement);
                    break;

                case MyWhileStatement whileStatement:
                    GenerateWhile(whileStatement);
                    break;

                case MyDoWhileStatement doWhileStatement:
                    GenerateDoWhile(doWhileStatement);
                    break;

                case MyBlockStatement block:
                    GenerateBlock(block);
                    break;

                case MyExpressionStatement expressionStatement:
                    expressionGenerator.Generate(
                        expressionStatement.Expression);
                    break;

                default:
                    errorHandler.Add(
                        $"Unsupported statement type '{statement.GetType().Name}'.",
                        0,
                        0);
                    break;
            }
        }

        private void GenerateVariableDeclaration(MyVariableDeclaration declaration)
        {
            if (declaration.Initializer != null)
            {
                string value = expressionGenerator.Generate(declaration.Initializer);

                emitter.Emit($"{declaration.Name} = {value}");
            }
        }

        private void GenerateAssignment(
            MyAssignment assignment)
        {
            string value =
                expressionGenerator.Generate(
                    assignment.Value);

            emitter.Emit(
                $"{assignment.Name} = {value}");
        }

        private void GenerateIf(MyIfStatement ifStatement)
        {
            string condition = expressionGenerator.Generate(ifStatement.Condition);

            string elseLabel = labelGenerator.NewLabel();

            emitter.Emit($"ifFalse {condition} goto {elseLabel}");

            Generate(ifStatement.ThenBranch);

            if (ifStatement.ElseBranch != null)
            {
                string endLabel = labelGenerator.NewLabel();

                emitter.Emit($"goto {endLabel}");

                emitter.Emit($"{elseLabel}:");

                Generate(ifStatement.ElseBranch);

                emitter.Emit($"{endLabel}:");
            }
            else
            {
                emitter.Emit($"{elseLabel}:");
            }
        }

        private void GenerateWhile(
            MyWhileStatement whileStatement)
        {
            string startLabel =
                labelGenerator.NewLabel();

            string endLabel =
                labelGenerator.NewLabel();

            emitter.Emit($"{startLabel}:");

            string condition =
                expressionGenerator.Generate(
                    whileStatement.Condition);

            emitter.Emit(
                $"ifFalse {condition} goto {endLabel}");

            Generate(whileStatement.Body);

            emitter.Emit($"goto {startLabel}");

            emitter.Emit($"{endLabel}:");
        }

        private void GenerateDoWhile(
            MyDoWhileStatement doWhileStatement)
        {
            string startLabel =
                labelGenerator.NewLabel();

            string endLabel =
                labelGenerator.NewLabel();

            emitter.Emit($"{startLabel}:");

            Generate(doWhileStatement.Body);

            string condition =
                expressionGenerator.Generate(
                    doWhileStatement.Condition);

            emitter.Emit(
                $"ifFalse {condition} goto {endLabel}");

            emitter.Emit($"goto {startLabel}");

            emitter.Emit($"{endLabel}:");
        }

        private void GenerateBlock(
            MyBlockStatement block)
        {
            foreach (var statement in block.Statements)
            {
                Generate(statement);
            }
        }
    }
}