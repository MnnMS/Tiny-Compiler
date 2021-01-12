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
        Parser parser = new Parser();
        public Form1()
        {
            InitializeComponent();
            
        }

        private void compileBtn_Click(object sender, EventArgs e)
        {
            errorText.Clear();
            string srcCode = srcCodeText.Text;
            Tiny_Compiler.Start_Compiling(srcCode);
            Node root = parser.Parse(Tiny_Compiler.Tiny_Scanner.Tokens);
            treeView1.Nodes.Add(PrintParseTree(root));
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

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.children.Count == 0)
                return tree;
            foreach (Node child in root.children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            errorText.Text = "";
            Errors.Error_List.Clear();
            tokenTable.Rows.Clear();
            Tiny_Compiler.TokenStream.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
