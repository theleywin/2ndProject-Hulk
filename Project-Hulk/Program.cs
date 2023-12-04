
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;


namespace Project_Hulk
{
	class Program
	{
		public static void Main(string[] args)
		{
			#region Pretty Print
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(" |__|   |  |   |     |/ \n |  | . |__| . |__ . |\\");
			Console.WriteLine();
			#endregion
			
			
		//	int debugg = 0;
			

            while (true)
			{
				Lexer.Restart();//limpia el imput y reinicia el indice 
				Console.Write("> ");

				string? input= Console.ReadLine();

				if (input == null) continue;

				if (input == "break") break;

				#region debugger
				//if (debug == 0)
				//{
				//	input = "function f (x)=> if (x==1)1 else x * f(x-1);";
				//	debug++;
				//}
				//else input = "f(99) ;";
				#endregion

				try
				{
					Lexer.Tokenizer(input);

					Expression result = new HulkExpression();
					result.Evaluate();

					if ((Lexer.index >= Lexer.Tokens.Count || Expression.Current() != ";") && Lexer.Tokens.Count != 0)
					{
						 throw new DefaultError("semicolon");
					}
					else
					{
						foreach (string Prints in Lexer.Prints)
						{
							Console.WriteLine(Prints);
						}
					}
				}
				catch (SystemErrors he)
				{
					he.PrintError();
				}
			}
		}
	}
}