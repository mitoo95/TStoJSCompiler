using System;
namespace TSCompiler.Core
{
	public class FunctionCall : Statement
	{
        public List<ExpresionType> Params { get; set; }

        public FunctionCall(List<ExpresionType> @params)
		{

			Params = @params;
		}
	}
}

