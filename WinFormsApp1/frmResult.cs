using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class frmResult : Form
    {
        string Text = "";
        public frmResult(string Text)
        {
            InitializeComponent();

            this.Text = Text;
        }

        private void frmResult_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = Text;
        }
    }
}
