
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Hulk
{
	class FunctionDeclaration : Expression
	{
		public static Dictionary<string, FunctionCall> FunctionStore = new Dictionary<string, FunctionCall>();
		public static Dictionary<string, int> FunctionStack = new Dictionary<string, int>();
		public List<string> FunctionArguments = new List<string>();

		public override void Evaluate()
		{
			List<string> FunctionExpression = new List<string>();

			if (Lexer.IsID(Current()))
			{
				if (Lexer.KeyWords.Contains(Current()))
				{
					throw new SyntaxError(Current(), "KeyWordID");
				}

				string functionName = Current();
				Next();
				if (Current() == "(")
				{
					Next();

					while (Lexer.index < Lexer.Tokens.Count && Current() != ")")
					{
						if (Lexer.IsID(Current()))
						{
							if (!FunctionArguments.Contains(Current()))
							{
								FunctionArguments.Add(Current());
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
								if (Lexer.IsID(Current()) && !Lexer.KeyWords.Contains(Current()) &&
									!FunctionArguments.Contains(Current()) && Current() != functionName && 
									!FunctionStore.ContainsKey(Current()) && !Let_in.StoreOfNames.ContainsKey(Current()))
								
								{
									throw new SyntaxError(Current(), "DoNotExistID");
								}
								FunctionExpression.Add(Current());
								Next();
							}

							if (Lexer.index < Lexer.Tokens.Count && Current() == ";")
							{
								FunctionExpression.Add(Current());
							}
							else return;

							if (FunctionStore.ContainsKey(functionName))
							{
								FunctionStore[functionName] = new FunctionCall(FunctionArguments, FunctionExpression, functionName);
							}
							else
							{
								FunctionStore.Add(functionName, new FunctionCall(FunctionArguments, FunctionExpression, functionName));
								FunctionStack.Add(functionName, 0);
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
