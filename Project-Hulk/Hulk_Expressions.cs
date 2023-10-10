using System;
using System.Text.RegularExpressions;

namespace ProjectHulk
{
    abstract class Expression
    {
        public string value;
        static public void Next()
        {
            Lexer.index++;
        }
        static public string ActualToken()
        {
            
            return Lexer.Tokens[Lexer.index];
        }
        public abstract void Evaluate();
    }
    abstract class BinaryExpressions : Expression
    {
        public Expression left;
        public bool LeftId = false;
        public Expression right;
        public bool RightId = false;

        public bool IsFunctionID(string id)
        {
            if (FunctionCall.functionsId.ContainsKey(id))
            {
                return true;
            }
            else return false;
        }
    }


    class Atom : Expression
    {
        //expresiones atómicas ( numbers , strings , bool )
        public override void Evaluate()
        {
            if (Lexer.IsNumber(ActualToken())) // numbers
            {
                string result = ActualToken();
                Next();
                value = result;
            }
            else if (ActualToken() == "-") // - numbers
            {
                Next();
                Expression num = new Atom();
                bool isIdfunction = false;
                if (FunctionCall.functionsId.ContainsKey(ActualToken())) isIdfunction = true;
                num.Evaluate();

                if (Lexer.TokenType(num.value) == "number")
                {
                    value = Convert.ToString(-1 * Convert.ToDouble(num.value));
                }
                else
                {
                    if (isIdfunction)
                    {
                        throw new SemanticError("Operator '-'", "ArgumentTypeError", Lexer.TokenType(num.value));
                    }
                    throw new SemanticError("Operator '-'", "Incorrect Operator", Lexer.TokenType(num.value));
                }
            }
            else if (Lexer.index < Lexer.Tokens.Count && ActualToken() == "(") // evalúa la expresión dentro del paréntesis
            {
                Next();
                Expression result = new BoolOperator();
                result.Evaluate();
                string s = result.value;
                if (Lexer.index < Lexer.Tokens.Count && ActualToken() == ")")
                {
                    Next();
                    value = s;
                }
                else
                {
                    throw new SyntaxError("Missing ' ) ", "Missing Token", "let-in", Lexer.Tokens[Lexer.index - 1]);
                }

            }
            else if (Lexer.index < Lexer.Tokens.Count && Regex.IsMatch(ActualToken(), @"(\u0022([^\u0022\\]|\\.)*\u0022)")) // strings
            {
                value = ActualToken();
                Next();
            }
            else if (Lexer.index < Lexer.Tokens.Count && ActualToken() == "let ") // let-in expressions
            {
                Next();
                Expression l = new Let_in();
                l.Evaluate();
                value = l.value;
            }
            else if (Lexer.index < Lexer.Tokens.Count && ActualToken() == "print") // print expression
            {
                Next();
                Expression p = new Print();
                p.Evaluate();
                value = p.value;
            }
            else if (Lexer.index < Lexer.Tokens.Count && MathExpressions.MathFunctions.Contains(ActualToken())) // Math function expression
            {
                Expression M = new MathExpressions(ActualToken());
                Next();
                M.Evaluate();
                value = M.value;

            }
            else if (Lexer.index < Lexer.Tokens.Count && FunctionCall.functionsId.ContainsKey(ActualToken())) // function variable 
            {
                string s = FunctionCall.functionsId[ActualToken()];
                Next();
                value = s;
            }
            else if (Lexer.index < Lexer.Tokens.Count && Let_in.idStore.ContainsKey(ActualToken())) // let-in variable
            {
                string s = Let_in.idStore[ActualToken()];
                Next();
                value = s;
            }
            else if (Lexer.index < Lexer.Tokens.Count && FunctionDeclaration.functionStore.ContainsKey(ActualToken())) // function call
            {
                int i = Lexer.index;
                Next();
                FunctionDeclaration.functionStore[Lexer.Tokens[i]].Evaluate();
                value = FunctionDeclaration.functionStore[Lexer.Tokens[i]].value;
            }
            else if (ActualToken() == "if") // If-Else expression
            {
                Next();
                Expression c = new IfElse();
                c.Evaluate();
                value = c.value;
            }
            else if (Lexer.index < Lexer.Tokens.Count && ActualToken() == "false") // bool false
            {
                Next();
                value = "false";
            }
            else if (Lexer.index < Lexer.Tokens.Count && ActualToken() == "true") // bool true
            {
                Next();
                value = "true";
            }

            //envio un error porque no es nada valido
            else
            {
                if (Lexer.IsID(ActualToken()))
                {
                    throw new SyntaxError(ActualToken(), "DoNotExistID");
                }
                else
                {
                    throw new UnexpectedTokenError(ActualToken());
                }
            }
        }
    }

    class HulkExpression : Expression
    {
        //Inicio de Expressión
        public override void Evaluate()
        {

             if (Lexer.Tokens.Count == 0)//  Expresion vacía
            {
                return;
            }

            else if (ActualToken() == "print")
            {
                Next();
                Expression printExp = new Print();
                printExp.Evaluate();
                value = printExp.value;
            }
            else if (ActualToken() == "let ")
            {
                Next();
                Expression letIn = new Let_in();
                letIn.Evaluate();
                value = letIn.value;
            }
            else if (ActualToken() == "if")
            {
                Next();
                Expression ifelse = new IfElse();
                ifelse.Evaluate();
                value = ifelse.value;
            }
            else if (ActualToken() == "function")
            {
                Next();
                Expression f = new FunctionDeclaration();
                f.Evaluate();
                value = "";
            }
            else if (FunctionDeclaration.functionStore.ContainsKey(ActualToken()))
            {

                int i = Lexer.index;
                Next();
                FunctionDeclaration.functionStore[Lexer.Tokens[i]].Evaluate();
                value = FunctionDeclaration.functionStore[Lexer.Tokens[i]].value;
                if (value != "" && value != null)
                {
                    if (Lexer.IsString(value))
                    {
                        Lexer.ConsolePrints.Add(value.Substring(1, value.Length - 2));// string value 
                    }
                    else
                    {
                        Lexer.ConsolePrints.Add(value);
                    }
                }
            }
            else // expresion atomica
            {
                Expression exp = new BoolOperator();
                exp.Evaluate();
                value = exp.value;
            }
        }
    }
}