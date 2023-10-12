using System.Text.RegularExpressions;

namespace Project_Hulk
{
	class FunctionCall : Expression
    {
        private string FunctionName ;
        public List<string> ArgumentsName = new List<string>();
        public List<string> ArgumentsValue = new List<string>();
        public List<string> FunctionExpression = new List<string>();
        public static Dictionary<string , string > FunctionNames = new Dictionary<string, string>();

        public FunctionCall(List<string> ArgumentsName , List<string> FunctionExpression , string FunctionName)
        {
            this.ArgumentsName = ArgumentsName ;
            this.FunctionExpression = FunctionExpression;
            this.FunctionName = FunctionName;
        }
        public override void Evaluate()
        {
            if(FunctionDeclaration.FunctionStack[FunctionName] > 500)
            {   
                throw new FunctionsErrors(FunctionName , "StackOverflow");
            }
            else FunctionDeclaration.FunctionStack[FunctionName]++;

            if(Current() == "(")
            {
                
                Next();

                Expression parameter = new BoolOperator();

                ArgumentsValue.Clear();

                Dictionary<string , string> OriginalValues = new Dictionary<string, string>();
                while(Lexer.index < Lexer.Tokens.Count && Current() != ")")
                {
                    parameter.Evaluate();
                    ArgumentsValue.Add(parameter.value);
                    if(Current() != ",")
                    {
                        break;
                    }
                    else Next();
                }
                if(ArgumentsName.Count == ArgumentsValue.Count)
                {
                    for(int i = 0 ; i < ArgumentsName.Count ; i++)
                    {
                        if(FunctionNames.ContainsKey(ArgumentsName[i]))
                        {
                            OriginalValues.Add(ArgumentsName[i] , FunctionNames[ArgumentsName[i]]);
                            //Actualiza
                            FunctionNames[ArgumentsName[i]] = ArgumentsValue[i];
                        }
                        else
                        {
                            FunctionNames.Add(ArgumentsName[i] , ArgumentsValue[i]);
                        }
                    }
                }
                else 
                {
                    throw new FunctionsErrors(FunctionName , "ArgumentsCountError", ArgumentsName.Count , ArgumentsValue.Count );
                }
                
                List<string> OriginalsTokens = Lexer.Tokens;
                int OriginalIndex = Lexer.index;

                Lexer.Tokens = FunctionExpression;
                Lexer.index = 0;

                Expression F_errors = new BoolOperator();
                
                try
                {
                    F_errors.Evaluate();
                }
                catch(SemanticError S_errors)
                {
                    Lexer.Tokens = OriginalsTokens;
                    Lexer.index = OriginalIndex;
                    if(S_errors.ProblemKind == "ArgumentTypeError")
                    {
                        throw new FunctionsErrors( FunctionName , "ArgumentTypeError" , S_errors.ExpectedToken , S_errors.InvalidToken);
                    }
                    else throw S_errors ;
                }
                catch(SystemErrors ST_errors)
                {
                    Lexer.Tokens = OriginalsTokens;
                    Lexer.index = OriginalIndex;
                    throw ST_errors;
                }
                
               
                
                Lexer.Tokens = OriginalsTokens;
                Lexer.index = OriginalIndex;

                foreach(string s in FunctionNames.Keys)
                {
                    if(OriginalValues.ContainsKey(s))
                    {
                        FunctionNames[s] = OriginalValues[s];
                    }
                }

                if(Current() == ")")
                {
                    value = F_errors.value ;
                   // Console.WriteLine(  value);
                    Next();
                    ArgumentsValue.Clear();
                    FunctionDeclaration.FunctionStack[FunctionName]--;
                }
                else 
                {
                    throw new SyntaxError("Missing ' ) ' " , "Missing Token" , "Function Declaration" , Lexer.Tokens[Lexer.index - 1]);
                }
            }
        }
    }

    class Print : Expression
    {
        public override void Evaluate()
        {
            if(Current() == "(")
            {
                Next();

                if(Current() == ")")
                {
                    Lexer.index++;
                    Console.WriteLine();
                    return;
                }

                Expression PrintExpression = new BoolOperator();
                PrintExpression.Evaluate();

                if(Current() == ")")
                {
                    Next();
                    if(Lexer.IsString(PrintExpression.value))
                    {
                        Lexer.Prints.Add(PrintExpression.value.Substring( 1 , PrintExpression.value.Length - 2));
                    }
                    else 
                    {
                        Lexer.Prints.Add(PrintExpression.value);
                    }
                    value = PrintExpression.value;
                }
                else
                {
                    throw new SyntaxError("Missing ' ) '" , "Missing Token" , "print" , Lexer.Tokens[Lexer.index-1] );
                }
            }
            else
            {
                throw new SyntaxError("Missing ' ( '" , "Missing Token" , "print" , Lexer.Tokens[Lexer.index-1]);
            }
        }
    }
	
}