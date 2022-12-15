
namespace TSCompiler.Core
{
    public class ConstantExpresion : Expresion
    {
        public ExpresionType Type { get; set; }
        public Token Token { get; set; }

        public ConstantExpresion(ExpresionType type, Token token)
        {
            Type = type;
            Token = token;
        }

        public override ExpresionType GetType()
        {
            return Type;
        }

        public override string GenerateCode() => this.Token.Lexeme.Replace("\'", "\"");
    }
}
