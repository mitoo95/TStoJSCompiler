
namespace TSCompiler.Core
{
    public abstract class Statement
    {
        public abstract void ValidateSemantic();

        public abstract string GenerateCode();
    }
}
