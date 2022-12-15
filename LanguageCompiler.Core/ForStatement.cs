
namespace TSCompiler.Core
{
    public class ForStatement : Statement
    {
        public Statement Declaration { get; set; }
        public Expresion BooleanExpression { get; set; }
        public Statement Statement { get; set; }
        public Statement Block { get; set; }


        public ForStatement(Statement declaration, Expresion booleanExpression, Statement statement, Statement block)
        {
            Declaration = declaration;
            BooleanExpression = booleanExpression;
            Statement = statement;
            Block = block;
        }

        public override void ValidateSemantic()
        {
            /*throw new NotImplementedException();*/
        }

        public override string GenerateCode() =>
        $"for({this.Declaration.GenerateCode()};{this.BooleanExpression.GenerateCode()};{this.Statement.GenerateCode()})" +
            $"{{{Environment.NewLine} {this.Block.GenerateCode()} {Environment.NewLine}}}";
    }
}
