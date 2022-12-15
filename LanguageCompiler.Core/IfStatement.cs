
namespace TSCompiler.Core
{
    public class IfStatement : Statement
    {
        public Expresion BooleanExpression { get; set; }
        public Statement TrueStatement { get; set; }
        //propiedad del else, puede ser nulo en caso que no haya else
        public Statement? FalseStatement { get; set; }

        public IfStatement(Expresion booleanExpression, Statement trueStatement, Statement falseStatement)
        {
            BooleanExpression = booleanExpression;
            TrueStatement = trueStatement;
            FalseStatement = falseStatement;
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

        public override string GenerateCode()
        {
            var code =
                $"if({this.BooleanExpression.GenerateCode()}){{{Environment.NewLine} {this.TrueStatement?.GenerateCode()} {Environment.NewLine} }}";
            if (FalseStatement is null)
            {
                return code;
            }

            code += $"else {{{Environment.NewLine} {this.FalseStatement.GenerateCode()}  {Environment.NewLine}}}";
            return code;
        }
    }
}
