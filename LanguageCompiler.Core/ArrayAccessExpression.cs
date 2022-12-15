
namespace TSCompiler.Core
{
	public class ArrayAccessExpression : Expresion
	{
        public ExpresionType Type { get; }
        public IdExpression Id { get; }
        public Expresion Index { get; }

        public ArrayAccessExpression(ExpresionType type, Token token, IdExpression id, Expresion index)
        {
            Type = type;
            Id = id;
            Index = index;
        }

        public override ExpresionType GetType()
        {
            return Type;
        }

        public override string GenerateCode() =>
        $"{this.Id.Name}[{this.Index.GenerateCode()}]";
    }
}

