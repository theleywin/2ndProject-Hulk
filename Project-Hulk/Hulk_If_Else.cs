using System.Text.RegularExpressions;

namespace ProjectHulk
{
    class IfElse : Expression
    {
        public override void Evaluate()
        {
            if( ActualToken() == "(" )
            {
                Next();

                Expression booleanExpression = new BoolOperator();
                booleanExpression.Evaluate();

                if( ActualToken() == ")" )
                {
                    Next();
            
                    if(booleanExpression.value == "true")
                    {

                        Expression trueExp = new BoolOperator();
                        trueExp.Evaluate();
                        
                        if(ActualToken() == "else")
                        {
                            value = trueExp.value ;
                            
                            while(Lexer.index < Lexer.Tokens.Count - 1 && ActualToken() != ";" )
                            {
                                Next();
                            }
                        }
                        else throw new SyntaxError("Missing ' else ' " , "Missing Token" , "if-else" , Lexer.Tokens[Lexer.index - 1]);
                    }
                    else if(booleanExpression.value == "false")
                    {
                        while(Lexer.index < Lexer.Tokens.Count - 1  && ActualToken() != "else" ) 
                        {
                            Next();
                        }
                        if(ActualToken() == "else")
                        {
                            Next();

                            Expression falseExp = new BoolOperator();
                            falseExp.Evaluate(); 

                            value = falseExp.value ;
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