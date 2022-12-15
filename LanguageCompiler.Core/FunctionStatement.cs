
namespace TSCompiler.Core
{
    public class FunctionStatement : Statement
    {
        public List<ExpresionType>? FirstExpression { get; set; }
        public ExpresionType SecondExpression { get; set; }
        public Statement Statement { get; set; }

        public FunctionStatement(List<ExpresionType>? firstExpression, ExpresionType secondExpression, Statement statement)
        {
            FirstExpression = firstExpression;
            SecondExpression = secondExpression;
            Statement = statement;
        }

        public override void ValidateSemantic()
        {
            /*throw new NotImplementedException();*/
        }

        public override string GenerateCode()
        {
            return "FunctionStatement";
        }
    }
}
