
namespace TSCompiler.Core
{
    public class DeclarationAssignation : Statement
    {
        public ExpresionType ExpresionType { get; set; }
        public Statement Statement { get; set; }

        public DeclarationAssignation(ExpresionType expresionType, Statement statement)
        {
            ExpresionType = expresionType;
            Statement = statement;
        }

        public override void ValidateSemantic()
        {
            /*throw new NotImplementedException();*/
        }

        public override string GenerateCode()
        {
            return "DeclarationAssignation";
        }
    }
}
