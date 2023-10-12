using System.Text.RegularExpressions;

namespace Project_Hulk
{
    class SumExpression : BinaryExpressions // aqui entran  "+"  "-" "@" 
    {   
       private List<string> NextTokens = new List<string>(){";", ")" ,"in",",",">","<","else","<","<=",">=","&","|","==","!="};
        
        public SumExpression() //jerarquía aritmética
        {
            this.left = new MultExpression();

            this.right = new MultExpression();
        }
        

        
        private static string Sum( string a , string b)
        {
            double result = double.Parse(a) + double.Parse(b);

            return Convert.ToString(result);  
        }
        private static string Subtract( string a , string b)
        {
            double result = double.Parse(a) - double.Parse(b);

            return Convert.ToString(result);  
        }

       

        public override void Evaluate()
        {
            if(Current() == "!")
            {
                Next();

                left = new SumExpression(); 
                if(IsFunctionName(Current())) LeftId = true ;
                left.Evaluate();

                if(left.value == "true")
                {
                    left.value = "false";
                }
                else if(left.value == "false")
                {
                    left.value = "true";
                }
                else 
                {
                    if(LeftId)
                    {
                        if(Lexer.KindOfToken(left.value) != "boolean")
                        {
                            throw new SemanticError("Operator '!'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value));
                        }
                    }
                    throw new SemanticError("Operator '!'" , "Incorrect Operator" , Lexer.KindOfToken(left.value));
                }
            }
            else 
            {
                if(IsFunctionName(Current())) LeftId = true ;
                left.Evaluate();
            }
        
            while(Lexer.index < Lexer.Tokens.Count)
            {

                if(Current() == "+")
                {
                    
                    Next();

                    if(IsFunctionName(Current())) RightId = true ;
                    right.Evaluate();

                    if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
                    {
                        left.value = Sum(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.KindOfToken(left.value) != "number")
                            {
                                throw new SemanticError("Operator '+'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.KindOfToken(right.value) != "number")
                            {
                                throw new SemanticError("Operator '+'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '+'" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number") ) ;
                    }
                }
                else if(Current() == "-")
                {
                    Next();

                    if(IsFunctionName(Current())) RightId = true ;
                    right.Evaluate();

                    if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
                    {
                        left.value = Subtract(left.value , right.value);
                    }
                    else 
                    {
                       if(LeftId)
                        {
                            if(Lexer.KindOfToken(left.value) != "number")
                            {
                                throw new SemanticError("Operator '-'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.KindOfToken(right.value) != "number")
                            {
                                throw new SemanticError("Operator '-'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '-'" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number") ) ;
                    }
                }
                else if(Current() == "@")
                {
                    Next();

                    if(Lexer.IsString(left.value))
                    {
                        left.value = left.value.Substring( 0 , left.value.Length - 1);
                    }
                    Expression literal = new BoolOperator();
                    literal.Evaluate();
                    if(Lexer.IsString(literal.value))
                    {
                        literal.value = literal.value.Substring( 1 , literal.value.Length - 1 );
                    }

                    value = left.value + Convert.ToString(literal.value);
                    return ;
                    
                }
                else if (NextTokens.Contains(Current()))
                {
                    value = Convert.ToString(left.value);
                    break;
                }
                else 
                {
                    throw new UnexpectedTokenError(Current());
                }
            }
        }
    }

    class MultExpression : BinaryExpressions  // aqui entran  "*"  "/"  "%" 
    {
        private List<string> NextTokens = new List<string>(){";", ")" ,"in",",",">","<","else","<","<=",">=","&","|","==","!=","@","+","-"};
      
        public MultExpression() //jerarquia aritmética
        {
            this.left = new PowExpression();

            this.right = new PowExpression();
        }

          private static string Multiply( string a , string b)
        {
            double result = double.Parse(a) * double.Parse(b);

            return Convert.ToString(result);  
        }
        private static string Division( string a , string b)
        {
            double result = double.Parse(a) / double.Parse(b);

            return Convert.ToString(result);  
        }
        private static string Module( string a , string b)
        {
            double result = double.Parse(a) % double.Parse(b);

            return Convert.ToString(result);  
        }


        public override void Evaluate()
        {
            if(IsFunctionName(Current())) LeftId = true ;
            left.Evaluate();

            while(Lexer.index < Lexer.Tokens.Count)
            {

                if(Current() == "*")
                {   
                    Next();
                    if(IsFunctionName(Current())) RightId = true ;
                    right.Evaluate();
                    
                    if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
                    {
                        left.value = Multiply(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.KindOfToken(left.value) != "number")
                            {
                                throw new SemanticError("Operator '*'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.KindOfToken(right.value) != "number")
                            {
                                throw new SemanticError("Operator '*'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '*'" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number") ) ;
    
                    }
                }
                else if(Current() == "/")
                {
                    Next();
                    if(IsFunctionName(Current())) RightId = true ;
                    right.Evaluate(); 
                    
                    if(right.value == "0")
                    {
                        throw new DefaultError("DivisionByZero");
                    }

                    if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
                    {
                        left.value = Division(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.KindOfToken(left.value) != "number")
                            {
                                throw new SemanticError("Operator '/'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.KindOfToken(right.value) != "number")
                            {
                                throw new SemanticError("Operator '/'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '/'" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number") ) ;
                    }
                }
                else if(Current() == "%")
                {   
                    Next();
                    if(IsFunctionName(Current())) RightId = true ;
                    right.Evaluate(); 

                    if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
                    {
                        left.value = Module(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.KindOfToken(left.value) != "number")
                            {
                                throw new SemanticError("Operator '%'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.KindOfToken(right.value) != "number")
                            {
                                throw new SemanticError("Operator '%'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '%'" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number") ) ;
                        
                    }
                    
                }
                else if(NextTokens.Contains(Current()))
                {
                    //Siguientes
                    value = Convert.ToString(left.value);
                    break;
                }
                else 
                {
                    throw new UnexpectedTokenError(Current());
                }

            }    
        }
    }

    class PowExpression : BinaryExpressions // aqui solo entra "^" 
    {
        private List<string> NextTokens = new List<string>(){";", ")" ,"in",",",">","<","else","<=",">=","&","|","==","!=","@","+","-","*","/","%"};
        public PowExpression() //jerarquía aritmética
        {
            this.left = new Atom();

            this.right = new Atom();
        }

        private static string Pow( string a , string b)
        {
            double result = Math.Pow(double.Parse(a) , double.Parse(b)) ;
            return Convert.ToString(result);  
        }

        public override void Evaluate()
        {
            if( IsFunctionName(Current())) LeftId = true ;
            left.Evaluate();
            
            while(Lexer.index < Lexer.Tokens.Count)
            {
                if(Current() == "^")
                {
                    Next();
                    if( IsFunctionName(Current())) RightId = true ;
                    right.Evaluate();
                    
                    if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
                    {
                        left.value = Pow(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.KindOfToken(left.value) != "number")
                            {
                                throw new SemanticError("Operator '^'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.KindOfToken(right.value) != "number")
                            {
                                throw new SemanticError("Operator '^'" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '^'" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number") ) ;
                    }
                }
                else if(NextTokens.Contains(Current()))
                {
                    value =  Convert.ToString(left.value);
                    break;
                }
                else 
                {
                    throw new UnexpectedTokenError(Current());
                }
            }    
        }
    }
    
}