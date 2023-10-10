using System.Text.RegularExpressions;

namespace ProjectHulk
{
    class Let_in : Expression
    {
        public static Dictionary< string , string> idStore = new Dictionary<string, string>();

        public override void Evaluate()
        {
            while(Lexer.index < Lexer.Tokens.Count)
            {
                if(Lexer.IsID(ActualToken()))
                {
                    if(Lexer.Key_Words.Contains(ActualToken()))
                    {
                        throw new SyntaxError(ActualToken() , "KeyWordID" );
                    }

                    string id = ActualToken();
                    Next();

                    if(ActualToken() == "=")
                    {
                        Next();
                        if(ActualToken() == "in" || ActualToken() == ",")
                        {
                            throw new SyntaxError("Missing Expression" ,  "Missing Token" , "let-in" , $"variable {Lexer.Tokens[Lexer.index - 2]}" );
                        }

                        Expression Value = new BoolOperator();
                        Value.Evaluate();
                            
                        string idValue = Value.value;

                        if(idStore.ContainsKey(id))
                        {
                            idStore[id] = idValue;
                        }
                        else if(FunctionCall.functionsId.ContainsKey(id))
                        {
                            FunctionCall.functionsId[id] = idValue;
                        }
                        else idStore.Add(id , idValue );
                            
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

                if(ActualToken() == ",")
                {
                    Next();
                }
                else if(ActualToken() == "in")
                {
                    Next();
                    break;
                }
                else if (Regex.IsMatch(ActualToken() , @"^[a-zA-Z]+\w*$"))
                {
                    throw new SyntaxError("Missing ' , '" , "Missing Token" , "let_in" , Lexer.Tokens[Lexer.index - 1]);
                }
                else 
                {
                    throw new SyntaxError("Invalid Token" , "Invalid Token" , "let-in" , ActualToken());
                }

            }    

            bool parenthesis = false;
            if(ActualToken() == "(")
            {
                Lexer.index++;
                parenthesis = true ;
            }
        
            Expression letInExp = new BoolOperator();
            letInExp.Evaluate();

            string result = letInExp.value ;
           
            if(result == null)
            {
                return;
            }

            if(parenthesis)
            {
                if(ActualToken() == ")")
                {
                    Lexer.index++;
                    value = result;
                    idStore.Clear();
                }
                else 
                {
                    throw new SyntaxError("Missing ' ) " , "Missing Token" , "let-in" , Lexer.Tokens[Lexer.index - 1] );
                }
            }
            else
            {
                value = result;
                idStore.Clear();
            }
        }  
    }
}