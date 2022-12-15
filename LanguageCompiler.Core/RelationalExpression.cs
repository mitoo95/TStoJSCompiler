
namespace TSCompiler.Core
{
    public class RelationalExpression : BinaryExpression
    {
        public Token Operation { get; set; }
        private readonly Dictionary<(ExpresionType, ExpresionType), ExpresionType> _typeRules;
        public RelationalExpression(Expresion leftExpression, Expresion rightExpression, Token operation) 
            : base(leftExpression, rightExpression)
        {
            Operation = operation;
            _typeRules = new Dictionary<(ExpresionType, ExpresionType), ExpresionType>
            {
                { (ExpresionType.Number, ExpresionType.Number), ExpresionType.Boolean},
            };

        }

        public override ExpresionType GetType()
        {
            var leftType = this.LeftExpression.GetType();
            var rightType = this.RightExpression.GetType();
            if (_typeRules.TryGetValue((leftType, rightType), out var resultType))
            {
                return resultType;
            }

            throw new ApplicationException($"Cannot apply operator '{Operation.Lexeme}' to operands of type {leftType} and {rightType}");
        }

        public override string GenerateCode() =>
        $"{this.LeftExpression.GenerateCode()} {this.Operation.Lexeme} {this.RightExpression.GenerateCode()}";
    }
}
