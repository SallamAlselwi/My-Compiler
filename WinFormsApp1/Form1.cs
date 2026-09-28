using MyCompiler;
using MyCompiler.Common;
using MyCompiler.Lexical_Layer;
using MyCompiler.Semantic_Layer;
using MyCompiler.Syntax_Layer;
using MyCompiler.Syntax_Layer.AST;
using MyCompiler.ICG;
using MyCompiler.Optimazation_Layer;
using MyCompiler.Generation;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void printTokens(List<Token> tokensList)
        {
            foreach (Token t in tokensList)
                richTextBox1.Text += ($"{t.Lexeme} ---> {t.Type}\n");
        }
        void printParser(MyProgramNode program)
        {
            MyParserTreePrinter printer = new MyParserTreePrinter();
            richTextBox1.Text = printer.Print(program);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ErrorHandler errorHandler = new ErrorHandler();
            SymbolTable symbolTable = new SymbolTable();

            MyLexical tokens = new MyLexical("TextFile1.txt", errorHandler);

            List<Token> tokensList = tokens.Analyze();

            //printTokens(tokensList);

            MySyntax syntax = new MySyntax(tokensList, symbolTable, errorHandler);
            MyProgramNode myProgramNode = syntax.Parse();

            //printParser(myProgramNode);

            MySemantic semantic = new MySemantic(symbolTable, errorHandler);
            semantic.Analyze(myProgramNode);

            MyIntermediateCodeGenerator icg = new MyIntermediateCodeGenerator(errorHandler);
            List<string> intermediatecode = icg.Generate(myProgramNode);

            //richTextBox1.Text = icg.GetCode() +"\n\n\n";

            MyCodeOptimizer optimizer = new MyCodeOptimizer();
            List<string> optimizedCode = optimizer.Optimize(intermediatecode);

            //richTextBox1.Text += string.Join(Environment.NewLine, optimizedCode);

            MyCodeGenerator myCodeGenerator = new MyCodeGenerator();
            
            richTextBox1.Text += myCodeGenerator.Generate(optimizedCode);

            if (errorHandler.HasErrors())
            {
                richTextBox1.Text += ("\nError Handler List:\n");

                richTextBox1.Text += errorHandler.ReturnError();
            }


        }
    }
}
