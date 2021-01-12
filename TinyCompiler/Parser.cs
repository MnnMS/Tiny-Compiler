using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

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

        Node Program()
        {
            //Program -> Functions mainFunction
            Node prog = new Node("Program");

            prog.children.Add(Functions());
            prog.children.Add(mainFunction());

            return prog;
        }

        private Node mainFunction()
        {
            //mainFunction -> Datatype Identifier
            Node main = new Node("mainFunction");

            main.children.Add(match(Datatype()));
            main.children.Add(match(Token_Class.Identifier));

            return main;
        }
        private Node Functions()
        {
            //Functions -> FunctionStatement FunctionStatementDash | FunctionStatementDash
            Node Functions = new Node("Functions");

            Functions.children.Add(FunctionStatement());
            Functions.children.Add(FunctionStatementDash());

            return Functions;

        }

        private Node FunctionStatementDash()
        {
            //FunctionStatmentDash -> FunctionStatment FunctionStatmentDash | E
            Node FunDash = new Node("FunctionStatementDash");

            FunDash.children.Add(FunctionStatement());
            FunDash.children.Add(FunctionStatementDash());

            return FunDash;
        }

        private Node FunctionStatement()
        {
            //FunctionStatment -> FunctionDecleration FunctionBody
            Node FunState = new Node("FunctionStatment");

            FunState.children.Add(FunctionDecleration());
            FunState.children.Add(FunctionBody());

            return FunState;

        }

        private Node FunctionDecleration()
        {
            //FunctionDecleration -> FunctionName ( Parameter )
            Node FunDec = new Node("FunctionDecleration");

            FunDec.children.Add(FunctionName());
            FunDec.children.Add(Parameter());

            return FunDec;
        }

        private Node FunctionBody()
        {
            // FunctionBody -> { Statments RetunStatment }
            Node FunBody = new Node("FunctionBody");
            if (check(Token_Class.LBraces))
            {
                FunBody.children.Add(match(Token_Class.LBraces));
                FunBody.children.Add(Statements());
                FunBody.children.Add(ReturnStatements());
                FunBody.children.Add(match(Token_Class.RBraces));

                return FunBody;
            }
            else
            {
                return null;
            }
        }

        private Node ReturnStatements()
        {
            //ReturnStatement -> return Expression ; 
            Node Rs = new Node("ReturnStatement");
            if (check(Token_Class.Return))
            {
                Rs.children.Add(match(Token_Class.Return));
                Rs.children.Add(Expression());

                return Rs;
            }
            else
            {
                return null;
            }
        }
        private Node Statements()
        {
            //Statements -> Statement MoreStatements
            Node S = new Node("Statements");

            S.children.Add(Statement());
            S.children.Add(MoreStatments());

            return S;
        }

        private Node MoreStatments()
        {
            //MoreStatements -> ; Statement MoreStatements | E
            Node MS = new Node("MoreStatements");

            if (check(Token_Class.Semicolon))
            {
                MS.children.Add(match(Token_Class.Semicolon));
                MS.children.Add(Statement());
                MS.children.Add(MoreStatments());

                return MS;
            }
            else
            {
                return null;
            }
        }

        private Node Statement()
        {
            //Statement -> Comment | Assignment | Decleration | WriteStatement | ReadStatement | ReturnStatement | if_Statement | RepeatStatement | E
            Node Statment = new Node("Statement");

            //if(check(Token_Class.Return))
            Statment.children.Add(ReturnStatements());
            Statment.children.Add(if_Statement());
            Statment.children.Add(RepeatStatement());

            return Statment;
            

        }

        private Node RepeatStatement()
        {
            // RepeatStatements -> repeat Statements Until ConditionStatement
            Node Repeat = new Node("RepeatStatment");

            if (check(Token_Class.Repeat))
            {
                Repeat.children.Add(match(Token_Class.Repeat));
                Repeat.children.Add(Statements());
                Repeat.children.Add(match(Token_Class.Until));
                Repeat.children.Add(ConditionStatement());

                return Repeat;
            }
            else
            {
                return null;
            }
        }

        private Node ConditionStatement()
        {
            // ConditionStatement -> Condition MoreConditions
            Node Cs = new Node("ConditionStatement");

            Cs.children.Add(Condition());
            Cs.children.Add(MoreConditions());

            return Cs;
        }

        private Node MoreConditions()
        {
            //MoreCondition -> BoolOP Condition MoreConditions | E
            Node MC = new Node("MoreConditions");

            MC.children.Add(BoolOP());
            MC.children.Add(Condition());
            MC.children.Add(MoreConditions());

            return MC;
        }

        private Node BoolOP()
        {
            //BoolOP -> "||" | &&
            Node Bol = new Node("BoolOP");

            if (check(Token_Class.AndOp))
            {
                Bol.children.Add(match(Token_Class.AndOp));
            }
            else if (check(Token_Class.OrOp))
            {
                Bol.children.Add(match(Token_Class.OrOp));
            }
            else
            {
                return null;
            }
            return Bol;
        }

        private Node Condition()
        {
            //Condition -> Identifier CondiotionOP Term
            Node Cond = new Node("Condition");

            if (check(Token_Class.Identifier))
            {
                Cond.children.Add(match(Token_Class.Identifier));
                Cond.children.Add(CondiotionOP());
                Cond.children.Add(Term());

                return Cond;
            }
            else
            {
                return null;
            }
            
        }

        private Node CondiotionOP()
        {
            //ConditonOP -> < | > | = | <>
            Node Co = new Node("ConditionOP");

            if (check(Token_Class.LessThanOp))
            {
                Co.children.Add(match(Token_Class.LessThanOp));
            }
            else if (check(Token_Class.GreaterThanOp))
            {
                Co.children.Add(match(Token_Class.GreaterThanOp));
            }
            else if (check(Token_Class.BooleanEqual))
            {
                Co.children.Add(match(Token_Class.BooleanEqual));
            }
            else if (check(Token_Class.NotEqualOp))
            {
                Co.children.Add(match(Token_Class.NotEqualOp));
            }

            return Co;
        }

        private Node if_Statement()
        {
            //if_Statement -> if CondationStatement then Statements ElseClause
            Node ifStat = new Node("if_Statement");

            if (check(Token_Class.If))
            {
                ifStat.children.Add(match(Token_Class.If));
                ifStat.children.Add(ConditionStatement());
                ifStat.children.Add(match(Token_Class.Then));
                ifStat.children.Add(Statements());
                ifStat.children.Add(ElseClause());

                return ifStat;
            }
            else
            {
                return null;
            }
        }

        private Node ElseClause()
        {
            //ElseClause -> ElseStatements End | End
            Node ElseC = new Node("ElseClause");

            ElseC.children.Add(ElseStatements());
            ElseC.children.Add(match(Token_Class.End));

            return ElseC;
        }

        private Node ElseStatements()
        {
            //ElseStatements -> elseif_Statement | else_Statement
            Node ES = new Node("ElseStatements");

            ES.children.Add(ElseIf());
            ES.children.Add(Else());

            return ES;
        }

        private Node Else()
        {
            throw new NotImplementedException();
        }

        private Node ElseIf()
        {
            Node ei = new Node("ElseIF");

            if(check(Token_Class.ElseIf))
            ei.children.Add()
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
                Errors.Error_List.Add("Expected to find " +
                    "Number or Identifier or FunctionCall "+
                    "and " + TokenStream[tokenIndex].lex.ToString() +
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
            }

            return Aop;
        }
        
    }

}
