using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
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
        static bool mainFuncExist = false;
        public Node Parse(List<Token> Tokens)
        {
            TokenStream = Tokens;
            root = new Node("Root");
            root.children.Add(program());

            if (!mainFuncExist)
                Errors.Error_List.Add("Your code doesn't contain a main()\n");

            return root;
        }
       bool check(Token_Class tc)
        {
            if (tokenIndex < TokenStream.Count  )
                if (TokenStream[tokenIndex].token_type == tc)
                {
                    return true;
                }
               

            return false;
        }

        
        public Node match(Token_Class ExpectedToken)
        {
            if (tokenIndex >=TokenStream.Count)
            {
                return null;
            }
            Token token = TokenStream[tokenIndex];
            if (token.token_type == ExpectedToken)
            {
                tokenIndex++;
                Node node = new Node(token.lex);
                return node;
            }
            else
            {
                //Errors.Error_List.Add("Expected to find "
                //        + ExpectedToken.ToString() + " and" +
                //        token.token_type.ToString() +
                //        " found\r\n"
                //        + " at " + tokenIndex.ToString() + "\n");
                tokenIndex++;
            }

            return null;
        }

        private Node program()
        {
            //Program -> Functions mainFunction
            Node prog = new Node("Program");

            prog.children.Add(Functions());
           // prog.children.Add(mainFunction());

            return prog;
        }

        private Node mainFunction()
        {
            //mainFunction -> Datatype Identifier
            Node main = new Node("mainFunction");
            mainFuncExist = true;
            main.children.Add(DataType());
            main.children.Add(match(Token_Class.Identifier));

            return main;
        }
        private Node Functions()
        {
            //Functions -> FunctionStatement FunctionStatementDash | FunctionStatementDash
            Node Functions = new Node("Functions");

            int tempIndex = tokenIndex;
            Node Temp;

            Temp = FunctionStatement();
            if (Temp != null)
            {
                Functions.children.Add(Temp);
                Functions.children.Add(FunctionStatementDash());
                return Functions;
            }
            else
            {
                tokenIndex = tempIndex;
                Functions.children.Add(FunctionStatementDash());
                return Functions;
            }

            

        }

        private Node FunctionStatementDash()
        {
            //FunctionStatmentDash -> FunctionStatment FunctionStatmentDash | E
            Node FunDash = new Node("FunctionStatementDash");
            int tempIndex = tokenIndex;
            Node Temp;

            if (check(Token_Class.INT) || check(Token_Class.Float) || check(Token_Class.String))
            {
                FunDash.children.Add(FunctionStatement());
                FunDash.children.Add(FunctionStatementDash());
                return FunDash;
            }
            
            return null;
            
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
            //FunctionDecleration -> DataType FunctionName FunctionParameter
            Node FunDec = new Node("FunctionDecleration");

            FunDec.children.Add(DataType());
            if (check(Token_Class.main))
            {
                mainFuncExist = true;
                FunDec.children.Add(match(Token_Class.main));
            }
            else
            {
                FunDec.children.Add(match(Token_Class.Identifier));
            }
            
            FunDec.children.Add(FunctionParameter());

            return FunDec;


        }

        private Node FunctionBody()
        {
            // FunctionBody -> { Statments RetunStatment }
            Node FunBody = new Node("FunctionBody");

                FunBody.children.Add(match(Token_Class.LBraces));
                FunBody.children.Add(Statements());
                FunBody.children.Add(ReturnStatements());
                FunBody.children.Add(match(Token_Class.RBraces));

                return FunBody;
        }

        private Node ReturnStatements()
        {
            //ReturnStatement -> return Expression ; 
            Node Rs = new Node("ReturnStatement");

                Rs.children.Add(match(Token_Class.Return));
                Rs.children.Add(Expression());
                Rs.children.Add(match(Token_Class.Semicolon));
                return Rs;
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
            //MoreStatements ->  Statement MoreStatements | E
            Node MS = new Node("MoreStatements");

            int tempIndex = tokenIndex;
            List<String> errorTemp = Errors.Error_List;
            Node temp = Statement();
            if (temp != null)
            {
                MS.children.Add(temp);
                MS.children.Add(MoreStatments());
            }
            else
            {
                tokenIndex = tempIndex;
                Errors.Error_List = errorTemp;
                return null;
            }
            
            return MS;
           
        }

        private Node Statement()
        {
            //Statement -> Assignment | Decleration | WriteStatement | ReadStatement | ReturnStatement | if_Statement | RepeatStatement | E
            Node statment = new Node("Statement");

            if (check(Token_Class.Return))
            {
                statment.children.Add(ReturnStatements());
            }

            else if (check(Token_Class.Repeat))
            {
                statment.children.Add(RepeatStatement());
            }

            else if (check(Token_Class.Identifier))
            {
                statment.children.Add(AssignmentStatment());
            }

            else if (check(Token_Class.Write))
            {
                statment.children.Add(Write());
            }

            else if (check(Token_Class.Read))
            {
                statment.children.Add(Read());
            }

            else if (check(Token_Class.If))
            {
                statment.children.Add(if_Statement());
            }

            else if (check(Token_Class.INT) || check(Token_Class.Float) || check(Token_Class.String))
            {
                statment.children.Add(DeclarStat());
            }
            else
                return null;

            return statment;
            

        }

        private Node RepeatStatement()
        {
            // RepeatStatements -> repeat Statements Until ConditionStatement
            Node Repeat = new Node("RepeatStatment");

                Repeat.children.Add(match(Token_Class.Repeat));
                Repeat.children.Add(Statements());
                Repeat.children.Add(match(Token_Class.Until));
                Repeat.children.Add(ConditionStatement());

                return Repeat;
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
            int tempindex = tokenIndex;
            Node temp = BoolOP();

            if (temp != null)
            {
                MC.children.Add(temp);
                MC.children.Add(Condition());
                MC.children.Add(MoreConditions());

            }
            else
                return null;
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

                Cond.children.Add(match(Token_Class.Identifier));
                Cond.children.Add(CondiotionOP());
                Cond.children.Add(Term());

                return Cond;
            
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
            else
                return null;
            return Co;
        }

        private Node if_Statement()
        {
            //if_Statement -> if CondationStatement then Statements ElseClause
            Node ifStat = new Node("if_Statement");

                ifStat.children.Add(match(Token_Class.If));
                ifStat.children.Add(ConditionStatement());
                ifStat.children.Add(match(Token_Class.Then));
                ifStat.children.Add(Statements());
                ifStat.children.Add(ElseClause());

                return ifStat;
        }

        private Node ElseClause()
        {
            //ElseClause -> ElseStatements End | End
            Node ElseC = new Node("ElseClause");
            int tempindex = tokenIndex;
            Node temp = ElseStatements();

            if (temp != null)
            {
                ElseC.children.Add(temp);
                ElseC.children.Add(match(Token_Class.End));
            }
            else
            {
                tokenIndex = tempindex;
                ElseC.children.Add(match(Token_Class.End));
            }
            

            return ElseC;
        }

        private Node ElseStatements()
        {
            //ElseStatements -> elseif_Statement | else_Statement
            Node ES = new Node("ElseStatements");
            int tempindex = tokenIndex;
            Node temp = ElseIf();

            if(temp != null)
            {
                ES.children.Add(temp);
            }
            else
            {
                temp = Else();
                if (temp != null)
                {
                    ES.children.Add(temp);
                }
                else
                {
                    return null;
                }
            }

            return ES;
        }

        private Node Else()
        {
            //Else -> else Statments end
            Node e = new Node("ElseStatement");

                e.children.Add(match(Token_Class.Else));
                e.children.Add(Statements());
                e.children.Add(match(Token_Class.End));
 
            return e;
        }

        private Node ElseIf()
        {
            //ElseIf -> elseif ConditionStatement then Statements ElseClause
            Node ei = new Node("ElseIfStatement");

                ei.children.Add(match(Token_Class.ElseIf));
                ei.children.Add(ConditionStatement());
                ei.children.Add(match(Token_Class.Then));
                ei.children.Add(Statements());
                ei.children.Add(ElseClause());

            return ei;
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
                //Errors.Error_List.Add("Expected to find Term and " +
                //         TokenStream[tokenIndex].lex.ToString() +
                //        " found\r\n" + " at " + tokenIndex.ToString() + "\n");
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
            else if (check(Token_Class.MinusOp))
            {
                Aop.children.Add(match(Token_Class.MinusOp));
            }
            else
            {
                //Error
                //Errors.Error_List.Add("Expected to find " +
                //    "Arthimatic Operator "+ "and " +
                //    TokenStream[tokenIndex].token_type + "Found ");
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
                eq.children.Add(Term_());
                return eq;
            }
            Node temp;
            int indexTemp = tokenIndex;
            List<String> errorTemp = Errors.Error_List;
            temp = Term();
            if (temp != null)
            {
                
                temp = Equation_();
                tokenIndex = indexTemp;
                Errors.Error_List = errorTemp;
                if (temp == null)
                {
                    return null;
                }
                else
                {
                    eq.children.Add(Term());
                    eq.children.Add(temp);
                }
            }
           
            return eq;
            
        }

        private Node Equation_()
        {
            //Equation_ -> Aop Factor Term_
            Node eq = new Node("Equation_");
            if (check(Token_Class.MultiplyOp) || check(Token_Class.DivideOp) || check(Token_Class.MinusOp) || check(Token_Class.PlusOp))
            {
                eq.children.Add(ArithamaticOperator());
                eq.children.Add(Factor());
                eq.children.Add(Term_());

            }
            else
            {
                return null;
            }

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
                return null;
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
            temp = Term();
            if (temp != null)
            {
                factor.children.Add(temp);
            }
            else
            {
                tokenIndex = indexTemp;
                Errors.Error_List = errorTemp;
                temp = Equation();
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
                temp = Equation();
               
                if (temp != null)
                {
                    exp.children.Add(temp);
                }
                else
                {
                    tokenIndex = indexTemp;
                    Errors.Error_List = errorTemp;
                    temp = Term();
                    if (temp != null)
                    {
                        exp.children.Add(temp);
                    }
                    else
                    {
                        tokenIndex = indexTemp;
                        Errors.Error_List = errorTemp;
                       // Errors.Error_List.Add("Expexted to find Expression but " +
                       //TokenStream[tokenIndex].token_type + "Found at " + tokenIndex);
                        return null;
                    }
                   
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
                //Errors.Error_List.Add("Expected to find DataType but " +
                //    TokenStream[tokenIndex].token_type + "Found at " + tokenIndex);
                return null;
            }
            return data;
        }

        Node DeclarStat()
        {
            //DeclarationStatement -> DataType Identifier Declaration_ ;
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
                    //Errors.Error_List.Add("Expected To find Expression or Endl but " +
                    //    TokenStream[tokenIndex].token_type + "Found at " + tokenIndex);
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
