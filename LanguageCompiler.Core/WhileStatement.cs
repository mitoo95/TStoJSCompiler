
namespace TSCompiler.Core
{
    public class WhileStatement : Statement
    {
        public Expresion BooleanExpression { get; set; }
        public Statement Statement { get; set; }

        public WhileStatement(Expresion booleanExpression, Statement statement)
        {
            BooleanExpression = booleanExpression;
            Statement = statement;
            this.ValidateSemantic();
        }

        public override void ValidateSemantic()
        {
            var exprType = this.BooleanExpression.GetType();
            if (exprType != ExpresionType.Boolean)
            {
                throw new ApplicationException($"Cannot implicitly convert '{exprType}' to bool");
            }
        }
        public override string GenerateCode() =>
        $"while({this.BooleanExpression.GenerateCode()}){{ {Environment.NewLine} {this.Statement.GenerateCode()} {Environment.NewLine}}}";
    }
}
