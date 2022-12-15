

namespace TSCompiler.Core
{
    public abstract class Expresion
    {
        public abstract ExpresionType GetType();

        public abstract string GenerateCode();
    }
}
