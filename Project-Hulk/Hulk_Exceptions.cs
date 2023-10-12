using System;
using System.Text.RegularExpressions;

namespace ProjectHulk
{
    abstract class HulkErrors : Exception
    {
        public abstract void PrintError();
    }

    class LexicalError : HulkErrors
    {
        public string InvalidToken;
        public LexicalError(string BadToken)
        {
            this.InvalidToken = BadToken;
        }
        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"! LEXICAL ERROR: '{InvalidToken}' isn't a valid token.");
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class SyntaxError : HulkErrors
    {
        public string? Problem;
        public string ProblemType;
        public string? ExpressionType;
        public string Token;

        //diferentes constructores porque envío diferentes mensajes y no necesito todos los parametros
        public SyntaxError(string Problem, string ProblemType, string ExpressionType, string Token)
        {
            this.Problem = Problem;
            this.ProblemType = ProblemType;
            this.ExpressionType = ExpressionType;
            this.Token = Token;
        }
        public SyntaxError(string Token, string ProblemType)
        {
            this.Token = Token;
            this.ProblemType = ProblemType;
        }
        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (ProblemType == "Missing Token")
            {
                System.Console.WriteLine($"! SYNTAX ERROR: {Problem} in '{ExpressionType}' expression after '{Token}' .");
            }
            else if (ProblemType == "Invalid Token")
            {
                System.Console.WriteLine($"! SYNTAX ERROR: {Problem} '{Token}' in '{ExpressionType}' expression");
            }
            else if (ProblemType == "DoNotExistID")
            {
                System.Console.WriteLine($"! SYNTAX ERROR: The name '{Token}' doesn't exist in the current context");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (ProblemType == "KeyWordID")
            {
                System.Console.WriteLine($"! SYNTAX ERROR: Invalid Id , the name '{Token}' it's a keyword from Hulk");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class SemanticError : HulkErrors
    {
        public string? ProblemType;
        public string? Problem;
        public string? BadToken;
        public string? ExpectedToken;
        public string? LeftToken;
        public string? RightToken;


        //diferentes constructores porque envío diferentes mensajes y no necesito todos los parametros
        public SemanticError(string Problem, string ProblemType, string BadToken)
        {
            this.Problem = Problem;
            this.BadToken = BadToken;
            this.ProblemType = ProblemType;
        }
        public SemanticError(string Problem, string ProblemType, string LeftToken, string RightToken, string expectedToken, string BadToken)
        {
            this.Problem = Problem;
            this.LeftToken = LeftToken;
            this.RightToken = RightToken;
            this.ProblemType = ProblemType;
            this.BadToken = BadToken;
            this.ExpectedToken = expectedToken;
        }
        public SemanticError(string Problem, string ProblemType, string expectedToken, string BadToken)
        {
            this.BadToken = BadToken;
            this.Problem = Problem;
            this.ExpectedToken = expectedToken;
            this.ProblemType = ProblemType;
        }
        public SemanticError(string BadToken, string ProblemType)
        {
            this.BadToken = BadToken;
            this.ProblemType = ProblemType;
        }

        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (ProblemType == "Incorrect Operator")
            {
                System.Console.WriteLine($"SEMANTIC ERROR:{Problem} cannot be applied to operators of type '{BadToken}'");
            }
            else if (ProblemType == "Incorrect Binary Expression")
            {
                System.Console.WriteLine($"SEMANTIC ERROR: {Problem} cannot be used between '{LeftToken}' and '{RightToken}'");
            }
            else if (ProblemType == "DuplicateArgument")
            {
                Console.WriteLine($"SEMANTIC ERROR: The parameter name '{BadToken}' already exist");
            }
            else if (ProblemType == "StackOverflow")
            {
                System.Console.WriteLine($"SEMANTIC ERROR: Stack OverFlow Function {BadToken}.");
            }
            else if (ProblemType == "ArgumentTypeError")
            {
                System.Console.WriteLine($"SEMANTIC ERROR: {Problem} receives `{ExpectedToken}`, not `{BadToken}`.");
            }

            Console.ForegroundColor = ConsoleColor.Green;
        }

    }

    class FunctionsErrors : HulkErrors
    {
        public string functionName;
        public string ProblemType;
        public int? ArgumentsIdCount;
        public int? ArgumentsValueCount;
        public string? BadToken;
        public string? expectedToken;

        //diferentes constructores porque envío diferentes mensajes y no necesito todos los parametros
        public FunctionsErrors(string functionName, string ProblemType)
        {
            this.functionName = functionName;
            this.ProblemType = ProblemType;
        }

        public FunctionsErrors(string functionName, string ProblemType, int ArgumentsIdCount, int ArgumentsValueCount)
        {
            this.functionName = functionName;
            this.ProblemType = ProblemType;
            this.ArgumentsIdCount = ArgumentsIdCount;
            this.ArgumentsValueCount = ArgumentsValueCount;
        }

        public FunctionsErrors(string functionName, string ProblemType, string expectedToken, string BadToken)
        {
            this.functionName = functionName;
            this.ProblemType = ProblemType;
            this.expectedToken = expectedToken;
            this.BadToken = BadToken;
        }

        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (ProblemType == "StackOverflow")
            {
                Console.WriteLine("FUNCTION ERROR: Stack Overflow " + functionName);
            }
            else if (ProblemType == "ArgumentsCountError")
            {
                System.Console.WriteLine($"FUNCTION ERROR: Function '{functionName}' receives {ArgumentsIdCount} argument/s, not {ArgumentsValueCount}.");
            }
            else if (ProblemType == "ArgumentTypeError")
            {
                System.Console.WriteLine($"FUNCTION ERROR: Function '{functionName}' receives '{expectedToken}', not `{BadToken}`.");
            }
            else if (ProblemType == "DuplicateArgument")
            {
                Console.WriteLine($"FUNCTION ERROR: The parameter name '{BadToken}' already exist");
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class UnexpectedTokenError : HulkErrors
    {
        public string BadToken;
        public UnexpectedTokenError(string BadToken)
        {
            this.BadToken = BadToken;
        }
        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"! UNEXPECTED ERROR : Token '{BadToken}' wasn't expected");
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class DefaultError : HulkErrors
    {
        public string ProblemType;
        public string? functionName;

        //diferentes constructores porque envío diferentes mensajes y no necesito todos los parametros
        public DefaultError(string ProblemType)
        {
            this.ProblemType = ProblemType;
        }
        public DefaultError(string ProblemType, string functionName)
        {
            this.ProblemType = ProblemType;
            this.functionName = functionName;
        }
        public override void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (ProblemType == "DivisionByZero")
            {
                System.Console.WriteLine("! DEFAULT ERROR: division by zero isn`t allowed");
            }
            else if (ProblemType == "ErrorFunctionBody")
            {
                System.Console.WriteLine("! DEFAULT ERROR: Invalid Function Declaration.");
            }
            else if (ProblemType == "StackOverflow")
            {
                Console.WriteLine("! DEFAULT ERROR: Stack Overflow " + functionName);
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}