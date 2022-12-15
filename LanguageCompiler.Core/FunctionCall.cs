
namespace TSCompiler.Core
{
	public class FunctionCall : Statement
	{
        public List<ExpresionType> Params { get; set; }

        public FunctionCall(List<ExpresionType> @params)
		{

			Params = @params;
		}

        public override void ValidateSemantic()
        {
            /*throw new NotImplementedException();*/
        }

        public override string GenerateCode()
        {
            return "FunctionCall";
        }
    }
}

