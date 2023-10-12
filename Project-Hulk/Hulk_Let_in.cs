using System.Text.RegularExpressions;

namespace ProjectHulk
{
	class Let_in : Expression
    {
        public static Dictionary< string , string> StoreOfNames = new Dictionary<string, string>();

        public override void Evaluate()
        {
            while(Lexer.index < Lexer.Tokens.Count)
            {
                if(Lexer.IsID(Current()))
                {
                    if(Lexer.KeyWords.Contains(Current()))
                    {
                        throw new SyntaxError(Current() , "KeyWordID" );
                    }

                    string name = Current();
                    Next();

                    if(Current() == "=")
                    {
                        Next();
                        if(Current() == "in" || Current() == ",")
                        {
                            throw new SyntaxError("Missing Expression" ,  "Missing Token" , "let-in" , $"variable {Lexer.Tokens[Lexer.index - 2]}" );
                        }

                        Expression Value = new BoolOperator();
                        Value.Evaluate();
                            
                        string NameValue = Value.value;

                        if(StoreOfNames.ContainsKey(name))
                        {
                            StoreOfNames[name] = NameValue;
                        }
                        else if(FunctionCall.FunctionNames.ContainsKey(name))
                        {
                            FunctionCall.FunctionNames[name] = NameValue;
                        }
                        else StoreOfNames.Add(name , NameValue );
                            
                    }
                    else
                    {
                        throw new SyntaxError("Missing ' = '" , "Missing Token" , "let-in" ,Lexer.Tokens[Lexer.index - 1] );
                    }  
                }
                else
                {
                    throw new SyntaxError("Missing ID" , "Missing Token" , "let-in" , Lexer.Tokens[Lexer.index - 1]);
                }            

                if(Current() == ",")
                {
                    Next();
                }
                else if(Current() == "in")
                {
                    Next();
                    break;
                }
                else if (Regex.IsMatch(Current() , @"^[a-zA-Z]+\w*$"))
                {
                    throw new SyntaxError("Missing ' , '" , "Missing Token" , "let_in" , Lexer.Tokens[Lexer.index - 1]);
                }
                else 
                {
                    throw new SyntaxError("Invalid Token" , "Invalid Token" , "let-in" , Current());
                }

            }    

            bool parenthesis = false;
            if(Current() == "(")
            {
                Lexer.index++;
                parenthesis = true ;
            }
        
            Expression LetInExpression = new BoolOperator();
            LetInExpression.Evaluate();

            string result = LetInExpression.value ;
           
            if(result == null)
            {
                return;
            }

            if(parenthesis)
            {
                if(Current() == ")")
                {
                    Lexer.index++;
                    value = result;
                    StoreOfNames.Clear();
                }
                else 
                {
                    throw new SyntaxError("Missing ' ) " , "Missing Token" , "let-in" , Lexer.Tokens[Lexer.index - 1] );
                }
            }
            else
            {
                value = result;
                StoreOfNames.Clear();
            }
        }  
    }
}