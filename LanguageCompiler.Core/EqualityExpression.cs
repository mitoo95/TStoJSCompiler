
namespace TSCompiler.Core
{
    public class EqualityExpression : BinaryExpression
    {
        private readonly Token _operation;
        private readonly Dictionary<(ExpresionType, ExpresionType), ExpresionType> _typeRules;
        public EqualityExpression(Expresion leftExpression, Expresion rightExpression, Token operation) 
            : base(leftExpression, rightExpression)
        {
            _operation = operation;
            _typeRules = new Dictionary<(ExpresionType, ExpresionType), ExpresionType>
            {
                {(ExpresionType.Number, ExpresionType.Number), ExpresionType.Boolean},
                {(ExpresionType.String, ExpresionType.String), ExpresionType.Boolean},
                {(ExpresionType.Boolean, ExpresionType.Boolean), ExpresionType.Boolean},
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

            throw new ApplicationException($"Cannot apply operator '=' to operands of type {leftType} and {rightType}");
        }

        public override string GenerateCode() =>
        $"{this.LeftExpression.GenerateCode()} {_operation.Lexeme} {this.RightExpression.GenerateCode()}";
    }
}
