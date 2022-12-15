
namespace TSCompiler.Core
{
	public class ConstantParam : ExpresionType
    {
        public ConstantParam(string lexeme, TokenType tokenType, ExpresionType expresionType, Token token) : base(lexeme, tokenType)
        {
            ExpresionType = expresionType;
            Token = token;
        }

        public string Lexeme { get; set; }

        public TokenType TokenType { get; set; }

        public ExpresionType ExpresionType { get; set; }

        public Token Token { get; set; }



    }
}

