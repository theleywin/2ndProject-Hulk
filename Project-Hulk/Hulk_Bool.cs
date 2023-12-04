using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Project_Hulk
{
	/// <summary>
	///  This Class represents Logic Operators such as '&' and '|'
	/// </summary>
	class BoolOperator : BinaryExpressions 
	{
		List<string> NextTokens = new List<string>(){")",";",",","in","else"};//Siguientes
		public BoolOperator()
		{
			this.left = new CompareOperations();

			this.right = new CompareOperations();
		}
		
		/// <summary>
		/// method for the '&' operator
		/// </summary>
		/// <param name="a">left token from '&'</param>
		/// <param name="b">right token from '&'</param>
		/// <returns></returns> 
		private static string And(string a , string b)
		{
			bool left = a == "true" ? true : false ;
			bool right = b == "true" ? true : false ;

			if(left && right)
			{
				return "true" ;
			}
			else return "false" ;
		}
		
		/// <summary>
		/// method for the '|' operator
		/// </summary>
		/// <param name="a">left token from '|' operator</param>
		/// <param name="b">right token from '|' operator</param>
		/// <returns></returns>
		private static string Or(string a , string b)
		{
			bool left = a == "true" ? true : false ;
			bool right = b == "true" ? true : false ;

			if(left || right)
			{
				return "true" ;
			}
			else return "false" ;
		}
		
       /// <summary>
       /// evaluates booleans expressions
       /// </summary>
		public override void Evaluate()
		{
			if(IsFunctionName(Current())) LeftId = true ;

			left.Evaluate();

			while(Lexer.index < Lexer.Tokens.Count)
			{
				if(Current() == "&")
				{
					Next();

					if(IsFunctionName( Current() )) RightId = true ;

					right.Evaluate();
					
					if(Lexer.KindOfToken(left.value) == "boolean" && Lexer.KindOfToken(right.value) == "boolean")
					{
						left.value = And(left.value , right.value);
					}
					else 
					{
						if(LeftId)
						{
							if(Lexer.KindOfToken(left.value) != "boolean")
							{
								throw new SemanticError("Operator ' & '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "boolean" , Lexer.KindOfToken(left.value) );
							}
						}
						else if(RightId)
						{
							if(Lexer.KindOfToken(right.value) != "boolean")
							{
								throw new SemanticError("Operator ' & '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "boolean" , Lexer.KindOfToken(right.value) );
							}
						}
						throw new SemanticError("Operator ' & '" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "boolean" , Lexer.GetInvalidToken(left.value , right.value , "boolean"));
					}
				}
				else if( Current() == "|" )
				{
					Next();
					if(IsFunctionName(Current())) RightId = true ;
					right.Evaluate();
					
					if(Lexer.KindOfToken(left.value) == "boolean" && Lexer.KindOfToken(right.value) == "boolean")
					{
						left.value = Or(left.value , right.value);
					}
					else 
					{
						if(LeftId)
						{
							if(Lexer.KindOfToken(left.value) != "boolean")
							{
								throw new SemanticError("Operator ' | '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "boolean" , Lexer.KindOfToken(left.value) );
							}
						}
						else if(RightId)
						{
							if(Lexer.KindOfToken(right.value) != "boolean")
							{
								throw new SemanticError("Operator ' | '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "boolean" , Lexer.KindOfToken(right.value) );
							}
						}
						throw new SemanticError("Operator ' | '" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "boolean" , Lexer.GetInvalidToken(left.value , right.value , "boolean"));
					}
				}
				else if(NextTokens.Contains(Current()))
				{
					value = left.value;
					break;
				}
				else 
				{
					throw new UnexpectedTokenError(Current());
				}
			}
		}
	}
     /// <summary>
     /// This Class represents Comparison Operators such as '==' '!=' '>' '<' '>=' '<='
     /// </summary>
	class CompareOperations : BinaryExpressions 
	{
		List<string> NextTokens = new List<string>(){")",";",",","in","else","&","|"};
		public CompareOperations()  //jerarqu�a
		{
			left = new SumExpression();

			right = new SumExpression();
		}

		
		private static string Equals(string a , string b)
		{
			return a == b ? "true" : "false";
		}
		private static string Different(string a , string b)
		{
			return a != b ? "true" : "false";
		}
		private static string Greater(string a , string b)
		{
			return double.Parse(a) > double.Parse(b) ? "true" : "false";
		}
		private static string Less(string a , string b)
		{
			return double.Parse(a) < double.Parse(b) ? "true" : "false";
		}
		private static string GreaterOrEqual(string a , string b)
		{
			return double.Parse(a) >= double.Parse(b) ? "true" : "false";
		}
		private static string LessOrEqual(string a , string b)
		{
			return double.Parse(a) <= double.Parse(b) ? "true" : "false";
		}

		public override void Evaluate()
		{   
			if(IsFunctionName( Current() )) LeftId = true ;

			left.Evaluate();

			while(Lexer.index < Lexer.Tokens.Count)
			{
				if( Current() == ">" )
				{
					Next();

					if(IsFunctionName( Current() )) RightId = true ;

					right.Evaluate();
					
					if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
					{
						left.value = Greater(left.value , right.value);
					}
					else 
					{
						if(LeftId)
						{
							if(Lexer.KindOfToken(left.value) != "number")
							{
								throw new SemanticError("Operator ' > '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
							}
						}
						else if(RightId)
						{
							if(Lexer.KindOfToken(right.value) != "number")
							{
								throw new SemanticError("Operator ' > '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
							}
						}
						throw new SemanticError("Operator ' > '" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number"));
						
					}
				}
				else if( Current() == "<" )
				{
					Next();

					if(IsFunctionName(Current())) RightId = true ;

					right.Evaluate();
				   
					if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
					{
						left.value = Less(left.value , right.value);
					}
					else 
					{
						if(LeftId)
						{
							if(Lexer.KindOfToken(left.value) != "number")
							{
								throw new SemanticError("Operator ' < '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
							}
						}
						else if(RightId)
						{
							if(Lexer.KindOfToken(right.value) != "number")
							{
								throw new SemanticError("Operator ' < '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
							}
						}
						throw new SemanticError("Operator ' < '" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number"));
					}
				}
				else if( Current() == "<=" )
				{
					Next();

					if(IsFunctionName( Current() )) RightId = true ;

					right.Evaluate();
					
					if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
					{
						left.value = LessOrEqual(left.value , right.value);
					}
					else 
					{
						if(LeftId)
						{
							if(Lexer.KindOfToken(left.value) != "number")
							{
								throw new SemanticError("Operator ' <= '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
							}
						}
						else if(RightId)
						{
							if(Lexer.KindOfToken(right.value) != "number")
							{
								throw new SemanticError("Operator ' <= '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
							}
						}
						throw new SemanticError("Operator ' <= '" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number"));
					}
				}
				else if( Current() == ">=" )
				{
					Next();

					if(IsFunctionName( Current() )) RightId = true ;

					right.Evaluate();
					
					if(Lexer.KindOfToken(left.value) == "number" && Lexer.KindOfToken(right.value) == "number")
					{
						left.value = GreaterOrEqual(left.value , right.value);
					}
					else 
					{
						if(LeftId)
						{
							if(Lexer.KindOfToken(left.value) != "number")
							{
								throw new SemanticError("Operator ' >= '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(left.value) );
							}
						}
						else if(RightId)
						{
							if(Lexer.KindOfToken(right.value) != "number")
							{
								throw new SemanticError("Operator ' >= '" , "ArgumentTypeError" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.KindOfToken(right.value) );
							}
						}
						throw new SemanticError("Operator ' >= '" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number"));
					}
				}
				else if( Current() == "==" )
				{
					Next();

					if(IsFunctionName(Current())) RightId = true ;

					right.Evaluate();
					
					if(Lexer.KindOfToken(left.value) == Lexer.KindOfToken(right.value))
					{
						left.value = Equals(left.value , right.value);
					}
					else
					{
						throw new SemanticError("Operator ' == '" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number"));
					}
				}
				else if( Current() == "!=" )
				{
					Next();

					if(IsFunctionName(Current())) RightId = true ;

					right.Evaluate();
					
					if(Lexer.KindOfToken(left.value) == Lexer.KindOfToken(right.value))
					{
						left.value = Different(left.value , right.value);
					}
					else
					{
						throw new SemanticError("Operator ' != '" , "Incorrect Binary Expression" , Lexer.KindOfToken(left.value) , Lexer.KindOfToken(right.value) , "number" , Lexer.GetInvalidToken(left.value , right.value , "number"));
					}
				}
				else if (NextTokens.Contains(Current()))
				{
					value = left.value ;
					break;
				}
				else 
				{
					throw new UnexpectedTokenError(Current());
				}
			} 
		}
	}   
}