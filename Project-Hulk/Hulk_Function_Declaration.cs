
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHulk
{
	class FunctionDeclaration : Expression
	{
		public static Dictionary<string, FunctionCall> functionStore = new Dictionary<string, FunctionCall>();
		public static Dictionary<string, int> functionStack = new Dictionary<string, int>();
		public List<string> functionArguments = new List<string>();

		public override void Evaluate()
		{
			List<string> functionExpression = new List<string>();

			if (Lexer.IsID(ActualToken()))
			{
				if (Lexer.Key_Words.Contains(ActualToken()))
				{
					throw new SyntaxError(ActualToken(), "KeyWordID");
				}

				string functionId = ActualToken();
				Next();
				if (ActualToken() == "(")
				{
					Next();

					while (Lexer.index < Lexer.Tokens.Count && ActualToken() != ")")
					{
						if (Lexer.IsID(ActualToken()))
						{
							if (!functionArguments.Contains(ActualToken()))
							{
								functionArguments.Add(ActualToken());
								Next();
							}
							else
							{
								throw new FunctionsErrors(ActualToken(), "DuplicateArgument");
							}
						}
						else
						{
							throw new SyntaxError("Missing ID", "Missing Token", "Function declaration", Lexer.Tokens[Lexer.index - 1]);
						}
						if (ActualToken() != ",")
						{
							break;
						}
						else Next();
					}
					if (ActualToken() == ")")
					{
						Next();
						if (ActualToken() == "=>")
						{
							Next();
							while (ActualToken() != ";" && Lexer.index < Lexer.Tokens.Count)
							{
								if (Lexer.IsID(ActualToken()) && !Lexer.Key_Words.Contains(ActualToken()) && !functionArguments.Contains(ActualToken()) && ActualToken() != functionId && !functionStore.ContainsKey(ActualToken()) && !Let_in.idStore.ContainsKey(ActualToken()))
								{
									throw new SyntaxError(ActualToken(), "DoNotExistID");
								}
								functionExpression.Add(ActualToken());
								Next();
							}

							if (Lexer.index < Lexer.Tokens.Count && ActualToken() == ";")
							{
								functionExpression.Add(ActualToken());
							}
							else return;

							if (functionStore.ContainsKey(functionId))
							{
								functionStore[functionId] = new FunctionCall(functionArguments, functionExpression, functionId);
							}
							else
							{
								functionStore.Add(functionId, new FunctionCall(functionArguments, functionExpression, functionId));
								functionStack.Add(functionId, 0);
							}
						}
						else
						{
							throw new SyntaxError("Missing ' => ", "Missing Token", "Function Declaration", Lexer.Tokens[Lexer.index - 1]);
						}
					}
					else
					{
						throw new SyntaxError("Missing ' ) ", "Missing Token", "Function Declaration", Lexer.Tokens[Lexer.index - 1]);
					}
				}
			}
			else
			{
				throw new SyntaxError("Missing ID", "Missing Token", "let-in", Lexer.Tokens[Lexer.index - 1]);
			}
		}
	}
}
