using System;
using System.Text.RegularExpressions;

namespace Project_Hulk
{
    abstract class SystemErrors : Exception
    {
        public abstract void PrintError();
    }

    class LexicalError : SystemErrors
    {
        public string InvalidToken;
        public LexicalError(string InvalidToken)
        {
            this.InvalidToken = InvalidToken;
        }
        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"! LEXICAL ERROR: '{InvalidToken}' isn't a valid token.");
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class SyntaxError : SystemErrors
    {
        public string ProblemKind;
        public string? Problem;
        public string? ExpressionKind;
        public string Token;

        //diferentes constructores porque envío diferentes mensajes y no necesito todos los parametros
        public SyntaxError(string Problem, string ProblemKind, string ExpressionKind, string Token)
        {
            this.Problem = Problem;
            this.ProblemKind = ProblemKind;
            this.ExpressionKind = ExpressionKind;
            this.Token = Token;
        }
        public SyntaxError(string Token, string ProblemKind)
        {
            this.Token = Token;
            this.ProblemKind = ProblemKind;
        }
        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (ProblemKind == "Missing Token")
            {
                System.Console.WriteLine($"! SYNTAX ERROR: {Problem} in '{ExpressionKind}' expression after '{Token}' .");
            }
            else if (ProblemKind == "Invalid Token")
            {
                System.Console.WriteLine($"! SYNTAX ERROR: {Problem} '{Token}' in '{ExpressionKind}' expression");
            }
            else if (ProblemKind == "DoNotExistID")
            {
                System.Console.WriteLine($"! SYNTAX ERROR: The name '{Token}' doesn't exist in the current context");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (ProblemKind == "KeyWordID")
            {
                System.Console.WriteLine($"! SYNTAX ERROR: Invalid Id , the name '{Token}' it's a keyword from Hulk");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class SemanticError : SystemErrors
    {
        public string? ProblemKind;
        public string? Problem;
        public string? InvalidToken;
        public string? ExpectedToken;
        public string? LeftToken;
        public string? RightToken;


        //diferentes constructores porque envío diferentes mensajes y no necesito todos los parametros
        public SemanticError(string Problem, string ProblemKind, string InvalidToken)
        {
            this.Problem = Problem;
            this.InvalidToken = InvalidToken;
            this.ProblemKind = ProblemKind;
        }
        public SemanticError(string Problem, string ProblemKind, string LeftToken, string RightToken, string expectedToken, string InvalidToken)
        {
            this.Problem = Problem;
            this.LeftToken = LeftToken;
            this.RightToken = RightToken;
            this.ProblemKind = ProblemKind;
            this.InvalidToken = InvalidToken;
            this.ExpectedToken = expectedToken;
        }
        public SemanticError(string Problem, string ProblemKind, string ExpectedToken, string InvalidToken)
        {
            this.InvalidToken = InvalidToken;
            this.Problem = Problem;
            this.ExpectedToken = ExpectedToken;
            this.ProblemKind = ProblemKind;
        }
        public SemanticError(string InvalidToken, string ProblemKind)
        {
            this.InvalidToken = InvalidToken;
            this.ProblemKind = ProblemKind;
        }

        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (ProblemKind == "Incorrect Operator")
            {
                System.Console.WriteLine($"! SEMANTIC ERROR:{Problem} cannot be applied to operators of type '{InvalidToken}'");
            }
            else if (ProblemKind == "Incorrect Binary Expression")
            {
                System.Console.WriteLine($"! SEMANTIC ERROR: {Problem} cannot be used between '{LeftToken}' and '{RightToken}'");
            }
            else if (ProblemKind == "DuplicateArgument")
            {
                Console.WriteLine($"! SEMANTIC ERROR: The parameter name '{InvalidToken}' already exist");
            }
            else if (ProblemKind == "StackOverflow")
            {
                System.Console.WriteLine($"! SEMANTIC ERROR: Stack OverFlow Function {InvalidToken}.");
            }
            else if (ProblemKind == "ArgumentTypeError")
            {
                System.Console.WriteLine($"! SEMANTIC ERROR: {Problem} receives `{ExpectedToken}`, not `{InvalidToken}`.");
            }

            Console.ForegroundColor = ConsoleColor.Green;
        }

    }

    class FunctionsErrors : SystemErrors
    {
        public string ProblemKind;
        public string FunctionName;
        public string? InvalidToken;
        public string? ExpectedToken;
        public int? ArgumentsNameCount;
        public int? ArgumentsValueCount;

        //diferentes constructores porque envío diferentes mensajes y no necesito todos los parametros
        public FunctionsErrors(string FunctionName, string ProblemKind)
        {
            this.ProblemKind = ProblemKind;
            this.FunctionName = FunctionName;
        }

        public FunctionsErrors(string FunctionName, string ProblemKind, int ArgumentsNameCount, int ArgumentsValueCount)
        {
            this.ProblemKind = ProblemKind;
            this.FunctionName = FunctionName;
            this.ArgumentsNameCount = ArgumentsNameCount;
            this.ArgumentsValueCount = ArgumentsValueCount;
        }

        public FunctionsErrors(string FunctionName, string ProblemKind, string ExpectedToken, string InvalidToken)
        {
            this.ProblemKind = ProblemKind;
            this.InvalidToken = InvalidToken;
            this.FunctionName = FunctionName;
            this.ExpectedToken = ExpectedToken;
        }

        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (ProblemKind == "StackOverflow")
            {
                Console.WriteLine("! FUNCTION ERROR: Stack Overflow " + FunctionName);
            }
            else if (ProblemKind == "ArgumentsCountError")
            {
                System.Console.WriteLine($"! FUNCTION ERROR: Function '{FunctionName}' receives {ArgumentsNameCount} argument/s, not {ArgumentsValueCount}.");
            }
            else if (ProblemKind == "ArgumentTypeError")
            {
                System.Console.WriteLine($"! FUNCTION ERROR: Function '{FunctionName}' receives '{ExpectedToken}', not `{InvalidToken}`.");
            }
            else if (ProblemKind == "DuplicateArgument")
            {
                Console.WriteLine($"! FUNCTION ERROR: The parameter name '{InvalidToken}' already exist");
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class UnexpectedTokenError : SystemErrors
    {
        public string InvalidToken;
        public UnexpectedTokenError(string InvalidToken)
        {
            this.InvalidToken = InvalidToken;
        }
        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"! UNEXPECTED ERROR : Token '{InvalidToken}' wasn't expected");
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class DefaultError : SystemErrors
    {
        public string ProblemKind;
        public string? FunctionName;

        //diferentes constructores porque envío diferentes mensajes y no necesito todos los parametros
        public DefaultError(string ProblemKind)
        {
            this.ProblemKind = ProblemKind;
        }
        public DefaultError(string ProblemKind, string FunctionName)
        {
            this.ProblemKind = ProblemKind;
            this.FunctionName = FunctionName;
        }
        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (ProblemKind == "DivisionByZero")
            {
                System.Console.WriteLine("! DEFAULT ERROR: division by zero isn`t allowed");
            }
            else if (ProblemKind == "ErrorFunctionBody")
            {
                System.Console.WriteLine("! DEFAULT ERROR: Invalid Function Declaration.");
            }
            else if (ProblemKind == "StackOverflow")
            {
                Console.WriteLine("! DEFAULT ERROR: Stack Overflow on function" + FunctionName);
            }
            else if(ProblemKind == "NotABool")
            {
                Console.WriteLine("! DEFAULT ERROR: Invalid expression in an If-Else statement.");
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}