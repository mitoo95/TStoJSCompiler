
namespace TSCompiler.Core
{
    public class ReturnCode : Statement
    {
        public Statement Block { get; set; }
        public Statement Main { get; set; }
        public Statement Imports { get; set; }

        public ReturnCode(Statement imports, Statement block, Statement main)
        {
            Imports = imports;
            Block = block;
            Main = main;
        }

        public override void ValidateSemantic()
        {
            /*throw new NotImplementedException();*/
        }

        public override string GenerateCode() => $"{this.Imports.GenerateCode()} {Environment.NewLine} {this.Block.GenerateCode()}" +
            $"{Environment.NewLine} {this.Main.GenerateCode()}";
    }
}
