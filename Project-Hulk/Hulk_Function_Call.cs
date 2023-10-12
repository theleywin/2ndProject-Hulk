using System.Text.RegularExpressions;

namespace ProjectHulk
{
	class FunctionCall : Expression
    {
        private string functionName ;
        public List<string> argumentsId = new List<string>();
        public List<string> argumentsValue = new List<string>();
        public List<string> functionExpression = new List<string>();
        public static Dictionary<string , string > functionsId = new Dictionary<string, string>();

        public FunctionCall(List<string> argumentsId , List<string> functionExpression , string functionName)
        {
            this.argumentsId = argumentsId ;
            this.functionExpression = functionExpression;
            this.functionName = functionName;
        }
        public override void Evaluate()
        {
            if(FunctionDeclaration.functionStack[functionName] > 500)
            {   
                throw new FunctionsErrors(functionName , "StackOverflow");
            }
            else FunctionDeclaration.functionStack[functionName]++;

            if(Current() == "(")
            {
                
                Next();

                Expression parameter = new BoolOperator();

                argumentsValue.Clear();

                Dictionary<string , string> Original_values = new Dictionary<string, string>();
                while(Lexer.index < Lexer.Tokens.Count && Current() != ")")
                {
                    parameter.Evaluate();
                    argumentsValue.Add(parameter.value);
                    if(Current() != ",")
                    {
                        break;
                    }
                    else Next();
                }
                if(argumentsId.Count == argumentsValue.Count)
                {
                    for(int i = 0 ; i < argumentsId.Count ; i++)
                    {
                        if(functionsId.ContainsKey(argumentsId[i]))
                        {
                            Original_values.Add(argumentsId[i] , functionsId[argumentsId[i]]);
                            //Actualiza
                            functionsId[argumentsId[i]] = argumentsValue[i];
                        }
                        else
                        {
                            functionsId.Add(argumentsId[i] , argumentsValue[i]);
                        }
                    }
                }
                else 
                {
                    throw new FunctionsErrors(functionName , "ArgumentsCountError", argumentsId.Count , argumentsValue.Count );
                }
                
                List<string> originalsTokens = Lexer.Tokens;
                int originalIndex = Lexer.index;

                Lexer.Tokens = functionExpression;
                Lexer.index = 0;

                Expression FE = new BoolOperator();
                
                try
                {
                    FE.Evaluate();
                }
                catch(SemanticError se)//Catch argument type error
                {
                    Lexer.Tokens = originalsTokens;
                    Lexer.index = originalIndex;
                    if(se.ProblemType == "ArgumentTypeError")
                    {
                        throw new FunctionsErrors( functionName , "ArgumentTypeError" , se.ExpectedToken , se.BadToken);
                    }
                    else throw se ;
                }
                catch(HulkErrors he)
                {
                    Lexer.Tokens = originalsTokens;
                    Lexer.index = originalIndex;
                    throw he;
                }
                
               
                
                Lexer.Tokens = originalsTokens;
                Lexer.index = originalIndex;

                foreach(string s in functionsId.Keys)
                {
                    if(Original_values.ContainsKey(s))
                    {
                        functionsId[s] = Original_values[s];
                    }
                }

                if(Current() == ")")
                {
                    value = FE.value ;
                   // Console.WriteLine(  value);
                    Next();
                    argumentsValue.Clear();
                    FunctionDeclaration.functionStack[functionName]--;
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

                Expression printExp = new BoolOperator();
                printExp.Evaluate();

                if(Current() == ")")
                {
                    Next();
                    if(Lexer.IsString(printExp.value))
                    {
                        Lexer.Prints.Add(printExp.value.Substring( 1 , printExp.value.Length - 2));
                    }
                    else 
                    {
                        Lexer.Prints.Add(printExp.value);
                    }
                    value = printExp.value;
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