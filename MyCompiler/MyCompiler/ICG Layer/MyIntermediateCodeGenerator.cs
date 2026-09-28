using MyCompiler.Common;
using MyCompiler.Syntax_Layer.AST;

namespace MyCompiler.ICG
{
    public class MyIntermediateCodeGenerator
    {
        private readonly MyInstructionEmitter emitter;
        private readonly MyTempGenerator tempGenerator;
        private readonly MyLabelGenerator labelGenerator;

        private readonly MyExpressionGenerator expressionGenerator;
        private readonly MyStatementGenerator statementGenerator;

        public MyIntermediateCodeGenerator(ErrorHandler errorHandler)
        {
            emitter = new MyInstructionEmitter();

            tempGenerator = new MyTempGenerator();

            labelGenerator = new MyLabelGenerator();

            expressionGenerator = new MyExpressionGenerator(emitter, tempGenerator, errorHandler);

            statementGenerator = new MyStatementGenerator(emitter, expressionGenerator, labelGenerator, errorHandler);
        }

        public List<string> Generate(MyProgramNode program)
        {
            emitter.Clear();

            tempGenerator.Reset();

            labelGenerator.Reset();

            if (program == null)
                return emitter.GetInstructions();

            foreach (var statement in program.Statements)
            {
                statementGenerator.Generate(statement);
            }

            return emitter.GetInstructions();
        }

        public string GetCode()
        {
            return emitter.GetCode();
        }
    }
}