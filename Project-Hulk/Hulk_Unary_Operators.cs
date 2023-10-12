using System.Data.SqlTypes;
using System.Text.RegularExpressions ;
namespace Project_Hulk
{
    class UnaryExpressions : Expression
    {
        public string mathExp ;
        public static List<string> MathMethods = new List<string>(){ "cos" , "sin" , "sqrt" , "exp" , "log" , "rand" , "PI" , "E"};
        List<string> arguments = new List<string>();
        public UnaryExpressions(string mathExp)
        {
            this.mathExp = mathExp;
        }

        public override void Evaluate()
        {
            if(mathExp == "PI")
            {
               value = Convert.ToString(Math.PI);
               return ;
            }
            else if (mathExp == "E")
            {
                value = Convert.ToString(Math.E);
                return ;
            }
            if(Current() == "(")
            {   
                Next();

                while(Lexer.index < Lexer.Tokens.Count && Current() != ")")
                {
                    Expression e = new BoolOperator();
                    e.Evaluate();
                    arguments.Add(e.value);
                    if(Current() == ",")
                    {
                        Next();
                    }
                    else if(Current() == ")")
                    {
                        break ;
                    }
                    else throw new SyntaxError("Missing ' , '" , "Missing Token" , "Math function" , Lexer.Tokens[Lexer.index - 1]);
                }
                if(Current() == ")")
                {
                    if(mathExp == "sqrt")
                    {
                        sqrt();
                    }
                    else if(mathExp == "sin")
                    {
                        sin();
                    }
                    else if(mathExp == "cos")
                    {
                        cos();
                    }
                    else if(mathExp == "exp")
                    {
                        exp();
                    }
                    else if(mathExp == "log")
                    {
                        log();
                    }
                    else if(mathExp == "rand")
                    {
                        rand();
                    }

                    Next();
                }
                else throw new SyntaxError("Missing ' ) '" , "Missing Token" , "Math function" , Lexer.Tokens[Lexer.index-1]);
            }
            else throw new SyntaxError("Missing ' ( '" , "Missing Token" , "Math function" , Lexer.Tokens[Lexer.index-1]);
        }

        
        public void sqrt()
        {
            if(arguments.Count == 1)
            {
                if(Lexer.IsNumber(arguments[0]))
                {
                    double result = Math.Sqrt(Convert.ToDouble(arguments[0]));
                    value = Convert.ToString(result);
                }
                else throw new FunctionsErrors("sqrt" , "ArgumentTypeError" , "number" , Lexer.KindOfToken(arguments[0]));
            }
            else throw new FunctionsErrors("sqrt" , "ArgumentsCountError" , 1 , arguments.Count );
        }
        public void sin()
        {
            if(arguments.Count == 1)
            {
                if(Lexer.IsNumber(arguments[0]))
                {
                    double result = Math.Sin(Convert.ToDouble(arguments[0]));
                    value = Convert.ToString(result);
                }
                else throw new FunctionsErrors("sin" , "ArgumentTypeError" , "number" , Lexer.KindOfToken(arguments[0]));
            }
            else throw new FunctionsErrors("sin" , "ArgumentsCountError" , 1 , arguments.Count );
        }
        public void cos()
        {
           if(arguments.Count == 1)
            {
                if(Lexer.IsNumber(arguments[0]))
                {
                    double result = Math.Cos(Convert.ToDouble(arguments[0]));
                    value = Convert.ToString(result);
                }
                else throw new FunctionsErrors("cos" , "ArgumentTypeError" , "number" , Lexer.KindOfToken(arguments[0]));
            }
            else throw new FunctionsErrors("cos" , "ArgumentsCountError" , 1 , arguments.Count );

        }
        public void exp()
        {
            if(arguments.Count == 1)
            {
                if(Lexer.IsNumber(arguments[0]))
                {
                    double result = Math.Cos(Convert.ToDouble(arguments[0]));
                    value = Convert.ToString(result);
                }
                else throw new FunctionsErrors("exp" , "ArgumentTypeError" , "number" , Lexer.KindOfToken(arguments[0]));
            }
            else throw new FunctionsErrors("exp" , "ArgumentsCountError" , 1 , arguments.Count );
        }
        public void log()
        {
            if(arguments.Count == 2)
            {
                if(Lexer.IsNumber(arguments[0]))
                {
                    double logBase = Convert.ToDouble(arguments[0]);
                    if(Lexer.IsNumber(arguments[1]))
                    {
                        double n = Convert.ToDouble(arguments[1]);
                        double result = Math.Log(n , logBase);
                        value = Convert.ToString(result) ;
                    }
                    else throw new FunctionsErrors("log" , "ArgumentTypeError" , "number" , Lexer.KindOfToken(arguments[0]));
                }
                else throw new FunctionsErrors("log" , "ArgumentTypeError" , "number" , Lexer.KindOfToken(arguments[0]));
            }
            else throw new FunctionsErrors("log" , "ArgumentsCountError" , 2 , arguments.Count );
        }
        public void rand()
        {
            if(arguments.Count == 0)
            {
                Random r = new Random() ;
                double result = r.NextDouble();
                value = Convert.ToString(result);
            }
            else throw new FunctionsErrors("rand" , "ArgumentsCountError" , 0 , arguments.Count );
        }
        public void PI()
        {
            value = Convert.ToString(Math.PI) ;
        }

        public void E()
        {
            value = Convert.ToString(Math.E) ;
        }
        
        
    }
}