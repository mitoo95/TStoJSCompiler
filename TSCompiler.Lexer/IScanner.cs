using TSCompiler.Core;

namespace TSCompiler.Lexer
{
    public interface IScanner
    {
        Token GetNextToken();
    }
}