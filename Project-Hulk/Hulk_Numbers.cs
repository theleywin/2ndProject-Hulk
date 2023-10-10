using System.Text.RegularExpressions;

namespace ProjectHulk
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
            if(ActualToken() == "!")
            {
                Next();

                left = new SumExpression(); 
                if(IsFunctionID(ActualToken())) LeftId = true ;
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
                        if(Lexer.TokenType(left.value) != "boolean")
                        {
                            throw new SemanticError("Operator '!'" , "ArgumentTypeError" , Lexer.TokenType(left.value));
                        }
                    }
                    throw new SemanticError("Operator '!'" , "Incorrect Operator" , Lexer.TokenType(left.value));
                }
            }
            else 
            {
                if(IsFunctionID(ActualToken())) LeftId = true ;
                left.Evaluate();
            }
        
            while(Lexer.index < Lexer.Tokens.Count)
            {

                if(ActualToken() == "+")
                {
                    
                    Next();

                    if(IsFunctionID(ActualToken())) RightId = true ;
                    right.Evaluate();

                    if(Lexer.TokenType(left.value) == "number" && Lexer.TokenType(right.value) == "number")
                    {
                        left.value = Sum(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.TokenType(left.value) != "number")
                            {
                                throw new SemanticError("Operator '+'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.TokenType(right.value) != "number")
                            {
                                throw new SemanticError("Operator '+'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '+'" , "Incorrect Binary Expression" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.GetIncorrectToken(left.value , right.value , "number") ) ;
                    }
                }
                else if(ActualToken() == "-")
                {
                    Next();

                    if(IsFunctionID(ActualToken())) RightId = true ;
                    right.Evaluate();

                    if(Lexer.TokenType(left.value) == "number" && Lexer.TokenType(right.value) == "number")
                    {
                        left.value = Subtract(left.value , right.value);
                    }
                    else 
                    {
                       if(LeftId)
                        {
                            if(Lexer.TokenType(left.value) != "number")
                            {
                                throw new SemanticError("Operator '-'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.TokenType(right.value) != "number")
                            {
                                throw new SemanticError("Operator '-'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '-'" , "Incorrect Binary Expression" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.GetIncorrectToken(left.value , right.value , "number") ) ;
                    }
                }
                else if(ActualToken() == "@")
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
                else if (NextTokens.Contains(ActualToken()))
                {
                    value = Convert.ToString(left.value);
                    break;
                }
                else 
                {
                    throw new UnexpectedTokenError(ActualToken());
                }
            }
        }
    }

    class MultExpression : BinaryExpressions  // aqui entran  "*"  "/"  "%" 
    {
        private List<string> NextTokens = new List<string>(){";", ")" ,"in",",",">","<","else","<","<=",">=","&","|","==","!=","@","+","-"};
      
        public MultExpression() //jerarquia aritmética
        {
            this.left = new PowerExpression();

            this.right = new PowerExpression();
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
        private static string Modulo( string a , string b)
        {
            double result = double.Parse(a) % double.Parse(b);

            return Convert.ToString(result);  
        }


        public override void Evaluate()
        {
            if(IsFunctionID(ActualToken())) LeftId = true ;
            left.Evaluate();

            while(Lexer.index < Lexer.Tokens.Count)
            {

                if(ActualToken() == "*")
                {   
                    Next();
                    if(IsFunctionID(ActualToken())) RightId = true ;
                    right.Evaluate();
                    
                    if(Lexer.TokenType(left.value) == "number" && Lexer.TokenType(right.value) == "number")
                    {
                        left.value = Multiply(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.TokenType(left.value) != "number")
                            {
                                throw new SemanticError("Operator '*'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.TokenType(right.value) != "number")
                            {
                                throw new SemanticError("Operator '*'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '*'" , "Incorrect Binary Expression" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.GetIncorrectToken(left.value , right.value , "number") ) ;
    
                    }
                }
                else if(ActualToken() == "/")
                {
                    Next();
                    if(IsFunctionID(ActualToken())) RightId = true ;
                    right.Evaluate(); 
                    
                    if(right.value == "0")
                    {
                        throw new DefaultError("DivisionByZero");
                    }

                    if(Lexer.TokenType(left.value) == "number" && Lexer.TokenType(right.value) == "number")
                    {
                        left.value = Division(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.TokenType(left.value) != "number")
                            {
                                throw new SemanticError("Operator '/'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.TokenType(right.value) != "number")
                            {
                                throw new SemanticError("Operator '/'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '/'" , "Incorrect Binary Expression" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.GetIncorrectToken(left.value , right.value , "number") ) ;
                    }
                }
                else if(ActualToken() == "%")
                {   
                    Next();
                    if(IsFunctionID(ActualToken())) RightId = true ;
                    right.Evaluate(); 

                    if(Lexer.TokenType(left.value) == "number" && Lexer.TokenType(right.value) == "number")
                    {
                        left.value = Modulo(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.TokenType(left.value) != "number")
                            {
                                throw new SemanticError("Operator '%'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.TokenType(right.value) != "number")
                            {
                                throw new SemanticError("Operator '%'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '%'" , "Incorrect Binary Expression" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.GetIncorrectToken(left.value , right.value , "number") ) ;
                        
                    }
                    
                }
                else if(NextTokens.Contains(ActualToken()))
                {
                    //Siguientes
                    value = Convert.ToString(left.value);
                    break;
                }
                else 
                {
                    throw new UnexpectedTokenError(ActualToken());
                }

            }    
        }
    }

    class PowerExpression : BinaryExpressions // aqui solo entra "^" 
    {
        private List<string> NextTokens = new List<string>(){";", ")" ,"in",",",">","<","else","<=",">=","&","|","==","!=","@","+","-","*","/","%"};
        public PowerExpression() //jerarquía aritmética
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
            if( IsFunctionID(ActualToken())) LeftId = true ;
            left.Evaluate();
            
            while(Lexer.index < Lexer.Tokens.Count)
            {
                if(ActualToken() == "^")
                {
                    Next();
                    if( IsFunctionID(ActualToken())) RightId = true ;
                    right.Evaluate();
                    
                    if(Lexer.TokenType(left.value) == "number" && Lexer.TokenType(right.value) == "number")
                    {
                        left.value = Pow(left.value , right.value);
                    }
                    else 
                    {
                        if(LeftId)
                        {
                            if(Lexer.TokenType(left.value) != "number")
                            {
                                throw new SemanticError("Operator '^'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(left.value) );
                            }
                        }
                        else if(RightId)
                        {
                            if(Lexer.TokenType(right.value) != "number")
                            {
                                throw new SemanticError("Operator '^'" , "ArgumentTypeError" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.TokenType(right.value) );
                            }
                        }
                        throw new SemanticError("Operator '^'" , "Incorrect Binary Expression" , Lexer.TokenType(left.value) , Lexer.TokenType(right.value) , "number" , Lexer.GetIncorrectToken(left.value , right.value , "number") ) ;
                    }
                }
                else if(NextTokens.Contains(ActualToken()))
                {
                    value =  Convert.ToString(left.value);
                    break;
                }
                else 
                {
                    throw new UnexpectedTokenError(ActualToken());
                }
            }    
        }
    }
    
}