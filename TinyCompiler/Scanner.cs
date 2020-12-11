using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    ElseIf, End, Else, If, INT, Read, Then, Until, Write,
    Semicolon, LParanthesis, RParanthesis, LBraces, RBraces, Assignment, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp, AndOp, OrOp,
    Identifier, Number, Comma, Repeat, BooleanEqual, String, EndLine, Return, Float
}

namespace TinyCompiler
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }
    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("endl", Token_Class.EndLine);
            ReservedWords.Add("int", Token_Class.INT);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("return", Token_Class.Return);

            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LBraces);
            Operators.Add("}", Token_Class.RBraces);
            Operators.Add(":=", Token_Class.Assignment);
            Operators.Add("=", Token_Class.BooleanEqual);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if (CurrentChar >= 'A' && CurrentChar <= 'z') 
                {
                    
                }

                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {

                }
                else if (CurrentChar == '/')
                {

                }
                else
                {

                }

                Tiny_Compiler.TokenStream = Tokens;
            }
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            string temp = Lex.ToUpper();
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(temp))
            {
                Tok.token_type = ReservedWords[temp];
            }

            //Is it an identifier?
            if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identifier;
            }

            //Is it a Constant?
            if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Number;
            }

            //Is it an operator?

            Tokens.Add(Tok);
        }
        bool isIdentifier(string lex)
        {
            bool isValid = true;

            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;
           
            return isValid;
        }
    }
}
