

namespace TSCompiler.Core
{
	public class ArrayAssignationStatement : Statement
	{
        public IdExpression Id { get; }

        public Expresion Index { get; }

        public Expresion Expression { get; }

        private readonly Expresion _access;

        public ArrayAssignationStatement(ArrayAccessExpression access, Expresion expression)
        {
            Id = access.Id;
            Index = access.Index;
            Expression = expression;
            _access = access;
            this.ValidateSemantic();
        }
        public override void ValidateSemantic()
        {
            if (_access.GetType() is Array || Expression.GetType() is Array)
            {
                throw new ApplicationException($"Type {Expression.GetType()} is not assignable to {Id.GetType()}");
            }
            if (_access.GetType() != Expression.GetType())
            {
                throw new ApplicationException($"Type {Expression.GetType()} is not assignable to {Id.GetType()}");
            }
        }

        public override string GenerateCode() =>
       $"{this.Id.GenerateCode()}[{this.Index.GenerateCode()}] = {this.Expression.GenerateCode()};";
    }
}

