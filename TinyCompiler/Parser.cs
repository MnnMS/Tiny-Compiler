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
            else
            {
                return null;
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
            else
            {
                return null;
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
            Node temp;
            int indexTemp = tokenIndex;
            List<String> errorTemp = Errors.Error_List;
            temp = Equation();
            if (temp != null)
            {
                factor.children.Add(temp);
            }
            else
            {
                tokenIndex = indexTemp;
                Errors.Error_List = errorTemp;
                temp = Term();
                if (temp != null)
                {
                    factor.children.Add(temp);                   
                }
                else
                {
                    //Error
                    tokenIndex = indexTemp;
                    Errors.Error_List = errorTemp;
                    Errors.Error_List.Add("Expected Equation or Term but " +
                        TokenStream[tokenIndex].token_type + "Found at " + tokenIndex);
                    return null;
                }
            }         
            return factor;
        }

        Node Expression()
        {
            //Expression -> string | Term | Equation
            Node exp = new Node("Expression");
            int indexTemp = tokenIndex;
            Node temp;
            List<String> errorTemp;

            if (check(Token_Class.String))
            {
                exp.children.Add(match(Token_Class.String));
            }
            else
            {
                errorTemp = Errors.Error_List;
                temp = Factor();
                if (temp != null)
                {
                    exp.children.Add(temp);
                }
                else
                {
                    tokenIndex = indexTemp;
                    Errors.Error_List = errorTemp;
                    Errors.Error_List.Add("Expexted to find Expression but " +
                        TokenStream[tokenIndex].token_type + "Found at " + tokenIndex);
                    return null;
                }
            }
            

            return exp;
        }
        Node AssignmentStatment()
        {
            //AssignmentStatment -> Identifier := Expresison
            Node node = new Node("AssignmentStatment");

            node.children.Add(match(Token_Class.Identifier));
            node.children.Add(match(Token_Class.Assignment));
            node.children.Add(Expression());

            return node;
        }
        Node DataType()
        {
            // DataType -> int | float | string

            Node data = new Node("DataType");

            if (check(Token_Class.INT))
            {
                data.children.Add(match(Token_Class.INT));
            }
            else if (check(Token_Class.Float))
            {
                data.children.Add(match(Token_Class.Float));
            }
            else if (check(Token_Class.String))
            {
                data.children.Add(match(Token_Class.String));
            }
            else
            {
                Errors.Error_List.Add("Expected to find DataType but " +
                    TokenStream[tokenIndex].token_type + "Found at " + tokenIndex);
                return null;
            }
            return data;
        }

        Node DeclarStat()
        {
            //DeclarationStatement -> DataType Identifier Declaration_
            Node node = new Node("DeclarationStatement");

            node.children.Add(DataType());
            node.children.Add(match(Token_Class.Identifier));
            node.children.Add(Declaration_());
            node.children.Add(match(Token_Class.Semicolon));

            return node;
        }

        private Node Declaration_()
        {
            // Declaration_ -> := Expression MoreDeclare | MoreDeclare
            Node node = new Node("Declaration_");

            if (check(Token_Class.Assignment))
            {
                node.children.Add(match(Token_Class.Assignment));
                node.children.Add(Expression());
                
            }
            node.children.Add(MoreDeclare());

            return node;
        }

        private Node MoreDeclare()
        {
            // MoreDeclare -> ,Identifier Declaration_ | E

            Node more = new Node("MoreDeclaration");
            if (check(Token_Class.Comma))
            {
                more.children.Add(match(Token_Class.Comma));
                more.children.Add(match(Token_Class.Identifier));
                more.children.Add(Declaration_());
            }
            else
            {
                return null;
            }
            return more;
        }

        Node Write()
        {
            // Write -> write Write_ ;

            Node write = new Node("Write");

            write.children.Add(match(Token_Class.Write));
            write.children.Add(Write_());
            write.children.Add(match(Token_Class.Semicolon));

            return write;
        }

        private Node Write_()
        {
            // Write_ -> Expression | endl

            Node write = new Node("Write_");

            if (check(Token_Class.EndLine))
            {
                write.children.Add(match(Token_Class.EndLine));
            }
            else 
            {
                int indexTemp = tokenIndex;
                List<String> errorTemp = Errors.Error_List;
                Node temp = Expression();
                if (temp != null)
                {
                    write.children.Add(temp);
                }
                else
                {
                    tokenIndex = indexTemp;
                    Errors.Error_List = errorTemp;
                    Errors.Error_List.Add("Expected To find Expression or Endl but " +
                        TokenStream[tokenIndex].token_type + "Found at " + tokenIndex);
                    return null;
                }
            }

            return write;
        }

        Node Read()
        {
            // Write -> Read Identifier ;

            Node Read = new Node("Read");

            Read.children.Add(match(Token_Class.Read));
            Read.children.Add(match(Token_Class.Identifier));
            Read.children.Add(match(Token_Class.Semicolon));

            return Read;
        }
    }

}
