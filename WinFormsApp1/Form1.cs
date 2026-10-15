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
        ErrorHandler errorHandler = new ErrorHandler();
        SymbolTable symbolTable = new SymbolTable();

        public Form1()
        {
            InitializeComponent();
        }

        void printTokens(List<Token> tokensList)
        {
            foreach (Token t in tokensList)
                rich1SourceCode.Text += ($"{t.Lexeme} ---> {t.Type}\n");
        }
        void printParser(MyProgramNode program)
        {
            MyParserTreePrinter printer = new MyParserTreePrinter();
            rich1SourceCode.Text = printer.Print(program);
        }

        bool _IsError()
        {
            if (errorHandler.HasErrors())
            {
                richTextBox1.Text = ("Error Handler List:\n");
                richTextBox1.Text += errorHandler.ReturnError();

                return true;
            }
            else
            {
                richTextBox1.Text = "";
            }

            return false;
        }
        void Run()
        {
            errorHandler.ClearAll();
            symbolTable.ClearAll();

            MyLexical tokens = new MyLexical("TextFile1.txt", errorHandler);

            List<Token> tokensList = tokens.Analyze();

            if(_IsError())
            {
                return;
            }

            MySyntax syntax = new MySyntax(tokensList, symbolTable, errorHandler);
            MyProgramNode myProgramNode = syntax.Parse();

            if(_IsError())
            {
                return;
            }

            MySemantic semantic = new MySemantic(symbolTable, errorHandler);
            semantic.Analyze(myProgramNode);

            if(_IsError())
            {
                return;
            }

            MyIntermediateCodeGenerator icg = new MyIntermediateCodeGenerator(errorHandler);
            List<string> intermediatecode = icg.Generate(myProgramNode);

            if(_IsError())
            {
                return;
            }


            MyCodeOptimizer optimizer = new MyCodeOptimizer();
            List<string> optimizedCode = optimizer.Optimize(intermediatecode);

            //richTextBox1.Text += string.Join(Environment.NewLine, optimizedCode);

            MyCodeGenerator myCodeGenerator = new MyCodeGenerator();

            frmResult frmResult = new frmResult(myCodeGenerator.Generate(optimizedCode));

            frmResult.ShowDialog();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Items.Add("Type");
            listView1.Items.Add("MyType");

            listView1.Items.Add("int");
            listView1.Items.Add("ints");

            listView1.Items.Add("float");
            listView1.Items.Add("floats");

            listView1.Items.Add("if");
            listView1.Items.Add("ifs");

            listView1.Items.Add("while");
            listView1.Items.Add("whiles");

            listView1.Items.Add("do");
            listView1.Items.Add("dos");

            listView1.Items.Add("for");
            listView1.Items.Add("fors");

            rich1SourceCode.Focus();
        }

        private void rich1SourceCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void btRun_Click(object sender, EventArgs e)
        {
            using(StreamWriter streamWriter = new StreamWriter("TextFile1.txt", false))
            {
                streamWriter.Write(rich1SourceCode.Text);

                streamWriter.Close();
            }

            Run();



        }
    }
}
