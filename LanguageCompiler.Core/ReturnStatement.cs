
namespace TSCompiler.Core
{
    public class ReturnStatement : Statement
    {
        public Expresion? Expression { get; set; }

        public ReturnStatement(Expresion? expression)
        {
            Expression = expression;
        }

        public override void ValidateSemantic()
        {
            /*throw new NotImplementedException();*/
        }

        public override string GenerateCode() => $"return {this.Expression.GenerateCode()};";
    }
}
