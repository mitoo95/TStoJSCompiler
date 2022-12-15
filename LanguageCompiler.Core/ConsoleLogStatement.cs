
namespace TSCompiler.Core
{
	public class ConsoleLogStatement : Statement
	{
        public List<ExpresionType> Expressions { get; set; }

        public ConsoleLogStatement(List<ExpresionType> expressions)
        {
            Expressions = expressions;
            this.ValidateSemantic();
        }

        public override void ValidateSemantic()
        {
            /*if (this.Expressions.Any(x => x.GetType() != ExpresionType.String))
            {
                throw new ApplicationException("Cannot implicitly convert all print parameters to string");
            }*/
        }

        public override string GenerateCode() 
        {
            return "console.log()"; 
        }/*=>
         $"console.log({string.Join("<<", this.Expressions.Select(x => x.GenerateCode()))};";*/
    }
}

