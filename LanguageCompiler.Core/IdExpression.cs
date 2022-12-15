
namespace TSCompiler.Core
{
    public class IdExpression : Expresion
    {
        public string Name { get; set; }
        public ExpresionType Type { get; set; }

        public IdExpression(string name, ExpresionType type)
        {
            Name = name;
            Type = type;
        }

        public override ExpresionType GetType()
        {
            return Type;
        }

        public override string GenerateCode() =>
            this.Name;
    }
}
