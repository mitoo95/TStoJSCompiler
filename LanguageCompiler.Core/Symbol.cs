
namespace TSCompiler.Core
{
    public class Symbol
    {
        public IdExpression Id { get; set; }

        public Symbol(IdExpression id)
        {
            Id = id;
        }
    }
}
