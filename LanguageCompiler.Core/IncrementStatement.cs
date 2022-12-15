
namespace TSCompiler.Core
{
    public class IncrementStatement : Statement
    {
        public IdExpression Id { get; set; }
        public IncrementStatement(IdExpression id)
        {
            Id = id;
        }

        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override string GenerateCode() => $"{this.Id.GenerateCode()}++";
    }
}
