using TSCompiler.Core;

namespace TSCompiler.Core
{
    public class ExpresionType : IEquatable<ExpresionType>
    {

        public string Lexeme { get; set; }

        public TokenType TokenType { get; set; }

        public ExpresionType(string lexeme, TokenType tokenType)
        {
            Lexeme = lexeme;
            TokenType = tokenType;
        }

        public static ExpresionType Number => new ExpresionType("number", TokenType.BasicType);
        public static ExpresionType Boolean => new ExpresionType("boolean", TokenType.BasicType);
        public static ExpresionType String => new ExpresionType("string", TokenType.BasicType);
        public static ExpresionType Undefined => new ExpresionType("undefined", TokenType.BasicType);

        public bool Equals(ExpresionType? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return false;
            }

            return Lexeme == other.Lexeme && TokenType == other.TokenType;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return false;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Type)obj);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Lexeme, TokenType).GetHashCode();
        }

        public override string ToString()
        {
            return Lexeme;
        }
    }
}
