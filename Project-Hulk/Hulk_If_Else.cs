using System.Text.RegularExpressions;

namespace Project_Hulk
{
    class IfElse : Expression
    {
        public override void Evaluate()
        {
            if(Current() == "(" )
            {
                Next();
                
                Expression BoolExpression = new BoolOperator();
                BoolExpression.Evaluate();
                
                if(!Lexer.IsBool(BoolExpression.value))
                {
                    throw new DefaultError("NotABool");
                }
                if(Current() == ")" )
                {
                    Next();
            
                    if(BoolExpression.value == "true")
                    {

                        Expression TrueExpression = new BoolOperator();
                        TrueExpression.Evaluate();
                        
                        if(Current() == "else")
                        {
                            value = TrueExpression.value ;
                            
                            while(Lexer.index < Lexer.Tokens.Count - 1 && Current() != ";" && Current() != ")")
                            {
                                Next();
                            }
                        }
                        else throw new SyntaxError("Missing ' else ' " , "Missing Token" , "if-else" , Lexer.Tokens[Lexer.index - 1]);
                    }
                    else if(BoolExpression.value == "false")
                    {
                        while(Lexer.index < Lexer.Tokens.Count - 1  && Current() != "else" ) 
                        {
                            Next();
                        }
                        if(Current() == "else")
                        {
                            Next();

                            Expression FalseExpression = new BoolOperator();
                            FalseExpression.Evaluate(); 

                            value = FalseExpression.value ;
                        }
                    }
                }
                else
                {
                    throw new SyntaxError("Missing ' ) ' " , "Missing Token" , "if-else" , Lexer.Tokens[Lexer.index - 1]);
                }
            }
            else 
            {
                throw new SyntaxError("Missing ' ( ' " , "Missing Token" , "if-else" , Lexer.Tokens[Lexer.index - 1]);
            }
        }
    }
}