using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public class Node
    {
        public List<Node> children = new List<Node>();
        public string Name;
        public Node(string Name)
        {
            this.Name = Name;
        }
    }
    class Parser
    {
        int tokenIndex = 0;
        static List<Token> TokenStream;
        public static Node root;
        public static Node Parse(List<Token> Tokens)
        {
            TokenStream = Tokens;
           

            //write your parser code

            return root;
        }
       
        public Node match(Token_Class ExpectedToken)
        {



            return null;
        }

        Node FunctionCall()
        {
            Node FC = new Node("FunctionCall");

            FC.children.Add(match(Token_Class.Identifier));
            FC.children.Add(FunctionParameter());

            return FC;

        }

        private Node FunctionParameter()
        {
            Node FP = new Node("FunctionParameter");

            FP.children.Add(match(Token_Class.LParanthesis));
            FP.children.Add(Parameter());
            FP.children.Add(match(Token_Class.RParanthesis));

            return FP;
        }

        private Node Parameter()
        {
            Node par = new Node("Parameter");
            if (TokenStream[tokenIndex].token_type == Token_Class.Identifier)
            {
                par.children.Add(match(Token_Class.Identifier));
                par.children.Add(MoreParameter());
            }
            else
            {
                return null;
            }
            return par;
        }

        private Node MoreParameter()
        {
            Node MP = new Node("MoreParameter");

            if (TokenStream[tokenIndex].token_type == Token_Class.Comma)
            {
                MP.children.Add(match(Token_Class.Comma));
                MP.children.Add(match(Token_Class.Identifier));
                MP.children.Add(MoreParameter());
                return MP;
            }
            else
            {
                return null;
            }

        }


        Node Term()
        {
            //Term -> Number | Identifier | FunctionCall

            Node term = new Node("Term");

            if (TokenStream[tokenIndex].token_type == Token_Class.Number)
            {
                term.children.Add(match(Token_Class.Number));
            }
            else if (TokenStream[tokenIndex].token_type == Token_Class.Identifier)
            {
                term.children.Add(match(Token_Class.Identifier));
                if (TokenStream[tokenIndex].token_type == Token_Class.LParanthesis)
                {
                    term.children.Add(FunctionParameter());
                }
            }
            else 
            {
                //Error
                return null;
            }

            return term;
        }
    }

}
