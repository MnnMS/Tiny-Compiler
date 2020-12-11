using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyCompiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void compileBtn_Click(object sender, EventArgs e)
        {
            errorText.Clear();
            string srcCode = srcCodeText.Text;
            Tiny_Compiler.Start_Compiling(srcCode);
            PrintTokens();
            PrintErrors();
        }

        void PrintTokens()
        {
            for (int i = 0; i < Tiny_Compiler.Tiny_Scanner.Tokens.Count; i++)
            {
                List<Token> tokens = Tiny_Compiler.Tiny_Scanner.Tokens;
                tokenTable.Rows.Add(tokens.ElementAt(i).lex, tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
            {
                errorText.Text += Errors.Error_List[i];
                errorText.Text += "\r\n";
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            tokenTable.Rows.Clear();
            Tiny_Compiler.TokenStream.Clear();
        }
    }
}
