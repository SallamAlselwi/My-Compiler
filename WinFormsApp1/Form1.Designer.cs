namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rich1SourceCode = new RichTextBox();
            panel1 = new Panel();
            btRun = new Button();
            richTextBox1 = new RichTextBox();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // rich1SourceCode
            // 
            rich1SourceCode.BackColor = Color.Gainsboro;
            rich1SourceCode.Font = new Font("Segoe UI", 11F);
            rich1SourceCode.ForeColor = SystemColors.WindowText;
            rich1SourceCode.Location = new Point(0, 52);
            rich1SourceCode.Margin = new Padding(0, 3, 3, 3);
            rich1SourceCode.Name = "rich1SourceCode";
            rich1SourceCode.Size = new Size(1161, 584);
            rich1SourceCode.TabIndex = 0;
            rich1SourceCode.Text = "";
            rich1SourceCode.TextChanged += rich1SourceCode_TextChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(btRun);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1283, 52);
            panel1.TabIndex = 2;
            // 
            // btRun
            // 
            btRun.Location = new Point(12, 12);
            btRun.Name = "btRun";
            btRun.Size = new Size(108, 37);
            btRun.TabIndex = 0;
            btRun.Text = "Run";
            btRun.UseVisualStyleBackColor = true;
            btRun.Click += btRun_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.Control;
            richTextBox1.Dock = DockStyle.Bottom;
            richTextBox1.Location = new Point(0, 636);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(1283, 94);
            richTextBox1.TabIndex = 3;
            richTextBox1.Text = "";
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listView1.Dock = DockStyle.Right;
            listView1.Location = new Point(1161, 52);
            listView1.Name = "listView1";
            listView1.Size = new Size(122, 584);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Type";
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "MyType";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1283, 730);
            Controls.Add(listView1);
            Controls.Add(richTextBox1);
            Controls.Add(panel1);
            Controls.Add(rich1SourceCode);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "My Compiler";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox rich1SourceCode;
        private Panel panel1;
        private Button btRun;
        private RichTextBox richTextBox1;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
    }
}
