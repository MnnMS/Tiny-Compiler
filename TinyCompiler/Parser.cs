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
       bool check(Token_Class tc)
        {
            if (TokenStream[tokenIndex].token_type == tc)
                return true;

            return false;
        }

        
        public Node match(Token_Class ExpectedToken)
        {
            Token token = TokenStream[tokenIndex];
            if (token.token_type == ExpectedToken)
            {
                tokenIndex++;
                Node node = new Node(token.lex);
                return node;
            }
            else
            {
                Errors.Error_List.Add("Expected to find "
                        + ExpectedToken.ToString() + " and" +
                        token.token_type.ToString() +
                        " found\r\n"
                        + " at " + tokenIndex.ToString() + "\n");
                tokenIndex++;
            }

            return null;
        }

        Node FunctionCall()
        {
            // FuncitonCall -> Identifier FunctionParameter
            Node FC = new Node("FunctionCall");

            FC.children.Add(match(Token_Class.Identifier));
            FC.children.Add(FunctionParameter());

            return FC;

        }

        private Node FunctionParameter()
        {
            // FunctionParameter -> ( Parameter )
            Node FP = new Node("FunctionParameter");

            FP.children.Add(match(Token_Class.LParanthesis));
            FP.children.Add(Parameter());
            FP.children.Add(match(Token_Class.RParanthesis));

            return FP;
        }

        private Node Parameter()
        {
            // Parameter -> Identifier MoreParameter | E

            Node par = new Node("Parameter");
            if (check(Token_Class.Identifier))
            {
                par.children.Add(match(Token_Class.Identifier));
                par.children.Add(MoreParameter());
            }
           
            return par;
        }

        private Node MoreParameter()
        {
            // MoreParameter -> , Identifier MoreParameter | E
            Node MP = new Node("MoreParameter");

            if (check(Token_Class.Comma))
            {
                MP.children.Add(match(Token_Class.Comma));
                MP.children.Add(match(Token_Class.Identifier));
                MP.children.Add(MoreParameter());
                
            }
            return MP;

        }


        Node Term()
        {
            //Term -> Number | Identifier | FunctionCall

            Node term = new Node("Term");

            if (check(Token_Class.Number))
            {
                term.children.Add(match(Token_Class.Number));
            }
            else if (check(Token_Class.Identifier))
            {
                term.children.Add(match(Token_Class.Identifier));
                if (check(Token_Class.LParanthesis))
                {
                    term.children.Add(FunctionParameter());
                }
            }
            else 
            {
                //Error
                Errors.Error_List.Add("Expected to find Term and " +
                         TokenStream[tokenIndex].lex.ToString() +
                        " found\r\n" + " at " + tokenIndex.ToString() + "\n");
                return null;
            }

            return term;
        }

        Node ArithamaticOperator()
        {
            // ArithOp -> * | / | + | -

            Node Aop = new Node("ArithmaticOperator");
            if (check(Token_Class.MultiplyOp))
            {
                Aop.children.Add(match(Token_Class.MultiplyOp));
            }
            else if (check(Token_Class.DivideOp))
            {
                Aop.children.Add(match(Token_Class.DivideOp));
            }
            else if (check(Token_Class.PlusOp))
            {
                Aop.children.Add(match(Token_Class.PlusOp));
            }
            else if (check(Token_Class.PlusOp))
            {
                Aop.children.Add(match(Token_Class.PlusOp));
            }
            else
            {
                //Error
                Errors.Error_List.Add("Expected to find " +
                    "Arthimatic Operator "+ "and " +
                    TokenStream[tokenIndex].token_type + "Found ");
                return null;
            }

            return Aop;
        }
        Node Equation()
        {
            // Equation -> Term Eq` | ( Term Eq`) Term_
            Node eq = new Node("Equation");
            //Node temp;
            //int indexTemp = tokenIndex;
            if (check(Token_Class.LParanthesis))
            {
                eq.children.Add(match(Token_Class.LParanthesis));
                eq.children.Add(Term());
                eq.children.Add(Equation_());
                eq.children.Add(match(Token_Class.RParanthesis));
                return eq;
            }
            eq.children.Add(Term());
            eq.children.Add(Equation_());
            return eq;
            //temp = Term();
            //if (temp == null)
            //{
            //    tokenIndex = indexTemp;
            //    return null;
            //}
            //else
            //{
            //    eq.children.Add(temp);
            //}
            //indexTemp = tokenIndex;
            //temp = Equation_();
            //if (temp == null)
            //{
            //    tokenIndex = indexTemp;
            //    return null;
            //}
            //else
            //{
            //    eq.children.Add(temp);
            //}




        }

        private Node Equation_()
        {
            //Equation_ -> Aop Factor Term_
            Node eq = new Node("Equation_");

            eq.children.Add(ArithamaticOperator());
            eq.children.Add(Factor());
            eq.children.Add(Term_());

            return eq;
        }

        private Node Term_()
        {
            Node term = new Node("Term_");
            Node temp;
            int indexTemp = tokenIndex;
            temp = ArithamaticOperator();
            if (temp == null)
            {
                tokenIndex = indexTemp;
            }
            else
            {
                term.children.Add(ArithamaticOperator());
                term.children.Add(Term());
                term.children.Add(Term_());
            }
            

            return term;
        }

        private Node Factor()
        {
            Node factor = new Node("Factor");



            return factor;
        }
    }

}
