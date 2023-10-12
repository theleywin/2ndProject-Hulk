
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

			if (Lexer.IsID(Current()))
			{
				if (Lexer.Key_Words.Contains(Current()))
				{
					throw new SyntaxError(Current(), "KeyWordID");
				}

				string functionId = Current();
				Next();
				if (Current() == "(")
				{
					Next();

					while (Lexer.index < Lexer.Tokens.Count && Current() != ")")
					{
						if (Lexer.IsID(Current()))
						{
							if (!functionArguments.Contains(Current()))
							{
								functionArguments.Add(Current());
								Next();
							}
							else
							{
								throw new FunctionsErrors(Current(), "DuplicateArgument");
							}
						}
						else
						{
							throw new SyntaxError("Missing ID", "Missing Token", "Function declaration", Lexer.Tokens[Lexer.index - 1]);
						}
						if (Current() != ",")
						{
							break;
						}
						else Next();
					}
					if (Current() == ")")
					{
						Next();
						if (Current() == "=>")
						{
							Next();
							while (Current() != ";" && Lexer.index < Lexer.Tokens.Count)
							{
								if (Lexer.IsID(Current()) && !Lexer.Key_Words.Contains(Current()) &&
									!functionArguments.Contains(Current()) && Current() != functionId && 
									!functionStore.ContainsKey(Current()) && !Let_in.idStore.ContainsKey(Current()))
								
								{
									throw new SyntaxError(Current(), "DoNotExistID");
								}
								functionExpression.Add(Current());
								Next();
							}

							if (Lexer.index < Lexer.Tokens.Count && Current() == ";")
							{
								functionExpression.Add(Current());
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
