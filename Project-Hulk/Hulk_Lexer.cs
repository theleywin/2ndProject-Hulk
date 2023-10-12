using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;



namespace ProjectHulk
{
    class Lexer
    {
        public static List<string> Tokens = new List<string>();

        public static List<string> Prints = new List<string>();

        public static  NumberFormatInfo kind = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;

        public static string separator = kind.NumberDecimalSeparator; //testeando me di cuenta q los numeros los coge 
																	  //con "," en windows y con "." en mac
		public static int index = 0 ;
 
        public static List<string> Key_Words  = new List<string>()
        {"print" , "let " , "in", "function" , "if" , "else" , "true" , "false" ,"sin" , "cos" , "sqrt" , "rand" , "exp" , "log" , "PI" , "E" };
       


        public static void Restart()
        {
            foreach(string id in FunctionDeclaration.functionStack.Keys)
            {
                FunctionDeclaration.functionStack[id] = 0;
            }
            Prints.Clear();
            index = 0;
            Tokens.Clear();
        }
        public static void Tokenizer(string input)
        {
            input = Regex.Replace(input , @"\s+" , " ");
        
            Regex AllTokens = new(@"\d+$|\d+[\.,]{1}\d+|\+|\-|\*|\^|/|%|\(|\)|(=>)|(>=)|(<=)|<[=]{0}|>[=]{0}|!=|;|,|let |={1,2}|function|if|else|!|\&|\||true|false|(\u0022([^\u0022\\]|\\.)*\u0022)|@|\w+|[^\(\)\+\-\*/\^%<>=!&\|,;\s]+");
            Regex ValidTokens = new(@"^\d+$|^\d+[\.,]{1}\d+$|\+|\-|\*|\^|/|%|\(|\)|(=>)|(>=)|(<=)|<[=]{0}|>[=]{0}|!=|;|,|let |={1,2}|function|if|else|!|\&|\||true|false|(\u0022([^\u0022\\]|\\.)*\u0022)|@|^[a-zA-Z]+\w*$");
            
            List<Match> AllTok = AllTokens.Matches(input).ToList() ;
            
            
            
            foreach(Match m in AllTok )
            {
                if( ValidTokens.IsMatch (m.Value) )
                {
                    Tokens.Add(m.Value) ;
                }
                else 
                {
                    throw new LexicalError(m.Value);
                }
            }
        }
        public static bool IsNumber(string Token)
        {
            if (separator == ".") 
            {
                return Regex.IsMatch(Token, @"^-{0,1}\d+$|^-{0,1}\d+\.{1}\d+E(\+|-)\d+$|^∞$|^-{0,1}\d+\.{1}\d+$") || Token == Convert.ToString(double.IsPositiveInfinity) ? true : false;
            }
            else
            {
				return Regex.IsMatch(Token, @"^-{0,1}\d+$|^-{0,1}\d+,{1}\d+E(\+|-)\d+$|^∞$|^-{0,1}\d+,{1}\d+$") || Token == Convert.ToString(double.IsPositiveInfinity) ? true : false;
			}
        }
        public static bool IsString(string Token)
        {
            return Regex.IsMatch(Token , @"(\u0022([^\u0022\\]|\\.)*\u0022)") ? true : false ;
        }
        public static bool IsBool(string Token)
        {
           return Regex.IsMatch(Token , @"^true$|^false$") ?  true : false ;
        }
        public static bool IsID(string Token)
        {
            return Regex.IsMatch( Token , @"^[a-zA-Z]+\w*$") ? true : false ;
        }
        public static string TokenType(string Token)
        {
            if(IsNumber(Token))
            {
                return "number" ;
            }
            else if(IsString(Token))
            {
                return "string" ;
            }
            else if(IsBool(Token))
            {
                return "boolean" ;
            }
            else if (IsID(Token))
            {
                return "ID" ;
            }
            else 
            {
                System.Console.WriteLine(Token);
                return "unexpected token"; //quitar luego
            }
        }

        public static string GetIncorrectToken(string a , string b , string expectedToken)
        {
            if(a != expectedToken )
            {
                return TokenType(a) ;
            }
            else return TokenType(b) ;            
        }
        

    }
}