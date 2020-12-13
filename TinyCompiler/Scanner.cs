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

                if (isWhiteSpace(CurrentChar))
                    continue;

              if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
              {
                    while ((j + 1) < SourceCode.Length && ((SourceCode[j + 1] >= 'A' && SourceCode[j + 1] <= 'z') || (SourceCode[j + 1] >= '0' && SourceCode[j + 1] <= '9'))) {
                        CurrentChar = SourceCode[++j];
                        CurrentLexeme += CurrentChar;
                    }
                    i = j;
                    FindTokenClass(CurrentLexeme);  
              }

              else if(CurrentChar == '\"')
              {
                    while ((j + 1) < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[++j];
                        CurrentLexeme += CurrentChar;
                        if (CurrentChar == '\"' || CurrentChar == '\n') break;
                    }
                    i = j;
                    FindTokenClass(CurrentLexeme);   
                }

                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    while ((j + 1) < SourceCode.Length && (SourceCode[j + 1] >= '0' && SourceCode[j + 1] <= '9' || SourceCode[j + 1] == '.'))
                    {
                        CurrentChar = SourceCode[++j];
                        CurrentLexeme += CurrentChar;
                    }
                    i = j;
                    FindTokenClass(CurrentLexeme);
                    
                }
                else if(CurrentChar == '/' && (j + 1) < SourceCode.Length && SourceCode[j + 1] == '*')
                {

                    while ((j + 1) < SourceCode.Length)
                    {
                         CurrentChar = SourceCode[++j];
                         CurrentLexeme += CurrentChar;
                         if (CurrentChar == '/') break;
                    }
                    i = j;
                    if (isComment(CurrentLexeme)) continue;                       
                }
                else
                {
                   if (CurrentChar == '<')
                    {
                        if ((j + 1) < SourceCode.Length && (SourceCode[j+1] == '>' || SourceCode[j + 1] == '='))
                        {
                            CurrentChar = SourceCode[++j];
                            CurrentLexeme += CurrentChar;
                        }        
                    }
                   else if (CurrentChar == '>')
                    {
                        if ((j + 1) < SourceCode.Length && SourceCode[j + 1] == '=')
                        {
                            CurrentChar = SourceCode[++j];
                            CurrentLexeme += CurrentChar;
                        }
                    }
                   else if (CurrentChar == ':')
                    {
                        if ((j + 1) < SourceCode.Length && SourceCode[j + 1] == '=')
                        {
                            CurrentChar = SourceCode[++j];
                            CurrentLexeme += CurrentChar;
                        }
                    }
                   else if (CurrentChar == '&')
                    {
                        if ((j + 1) < SourceCode.Length && SourceCode[j + 1] == '&')
                        {
                            CurrentChar = SourceCode[++j];
                            CurrentLexeme += CurrentChar;
                        }
                    }
                    else if (CurrentChar == '|')
                    {
                        if ((j + 1) < SourceCode.Length && SourceCode[j + 1] == '|')
                        {
                            CurrentChar = SourceCode[++j];
                            CurrentLexeme += CurrentChar;
                        }
                    }
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }
            }
                Tiny_Compiler.TokenStream = Tokens;
            }

        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            bool unknownString = true;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                unknownString = false;
                Tok.token_type = ReservedWords[Lex];
            }

            else if (isString(Lex))
            {
                unknownString = false;
                Tok.token_type = Token_Class.String;
            }

            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                unknownString = false;
                Tok.token_type = Token_Class.Identifier;         
            }

            //Is it a Constant?
            if (isConstant(Lex))
            {
                unknownString = false;
                Tok.token_type = Token_Class.Number;
            }

            //Is it an operator?
            if (Operators.ContainsKey(Lex))
            {
                unknownString = false;
                Tok.token_type = Operators[Lex];
            }
            if (unknownString)
            {
                Errors.Error_List.Add(Lex + "  Unknown String");
            }
            else
                Tokens.Add(Tok);
        }

        bool isWhiteSpace(char x)
        {
            if(x == ' ' || x == '\t' || x == '\n' || x == '\r')
            {
                return true;
            }
            return false;
        }

        bool isString(string lex)
        {
            bool isValid = false;
            if (lex[0] == '\"' )
            {
                if (lex[lex.Length - 1] == '\"')
                    isValid = true;              
                else
                    Errors.Error_List.Add(lex +"  Unclosed String");
            }            
            return isValid;           
        }

        bool isIdentifier(string lex)
        {
            bool isValid = true;

            if (!(lex[0] >= 'A' && lex[0] <= 'z')) return false;

            return isValid;
        }
        bool isConstant(string lex)
        {
            int noOfDots = 0;
            bool isValid = true;
            if (lex[0] > '9' || lex[0] < '0' && lex[0]!='.') return false;
            int ind = lex.IndexOf('.');
            if (ind != -1)
            {
                for (int i = 0; i < lex.Length; i++)
                {
                    if (lex[i] == '.')
                    {
                        noOfDots++;
                    }
                    if (noOfDots>1)
                    {
                        Errors.Error_List.Add(lex + "  Wrong Constant with multiple dots");
                        return false;
                    }
                   
                }
                string[] arr = lex.Split('.');
                if (arr[1].Length == 0)
                {
                    Errors.Error_List.Add(lex + "  Wrong Constant Format");
                    return false;
                }
                for (int i = 0; i < arr[0].Length; i++)
                {
                    char CurrentChar = arr[0][i];
                    if (!(CurrentChar >= '0' && CurrentChar <= '9'))
                    {
                        Errors.Error_List.Add(lex + "  Wrong Constant Format");
                        return false;
                    }
                        
                }
                for (int i = 0; i < arr[1].Length; i++)
                {
                    char CurrentChar = arr[1][i];
                  
                    if (!(CurrentChar >= '0' && CurrentChar <= '9'))
                    {
                        Errors.Error_List.Add(lex + "  Wrong Constant Format");
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < lex.Length; i++)
                {
                    char CurrentChar = lex[i];
                    if (!(CurrentChar >= '0' && CurrentChar <= '9'))
                    {
                        Errors.Error_List.Add(lex + "  Wrong Constant Format");
                        return false;
                    }
                }
            }
            return isValid;
        }

        bool isComment(string lex)
        {
            int len = lex.Length;
            bool isValid = false;
            if (lex[0] == '/' && lex[1] == '*' && lex[len - 2] == '*' && lex[len - 1] == '/')
                return true;
            Errors.Error_List.Add(lex + "  Unclosed Comment");
            return isValid;
        }
    }
        
}
