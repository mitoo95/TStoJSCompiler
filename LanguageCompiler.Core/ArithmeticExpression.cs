
namespace TSCompiler.Core
{
    public class ArithmeticExpression : BinaryExpression
    {
        private readonly Dictionary<(ExpresionType, ExpresionType, TokenType), ExpresionType> _typeRules;
        public Token Operation { get; set; }
        public ArithmeticExpression(Expresion leftExpression, Expresion rightExpression, Token operation) 
            : base(leftExpression, rightExpression)
        {
            Operation = operation;
            _typeRules = new Dictionary<(ExpresionType, ExpresionType, TokenType), ExpresionType>
            {
                {(ExpresionType.Number, ExpresionType.Number, TokenType.Plus), ExpresionType.Number},
                {(ExpresionType.Number, ExpresionType.Number, TokenType.Minus), ExpresionType.Number},
                {(ExpresionType.Number, ExpresionType.Number, TokenType.Mult), ExpresionType.Number},
                {(ExpresionType.Number, ExpresionType.Number, TokenType.Division), ExpresionType.Number},

                {(ExpresionType.String, ExpresionType.String, TokenType.Plus), ExpresionType.String},
                {(ExpresionType.String, ExpresionType.Number, TokenType.Plus), ExpresionType.String},
                {(ExpresionType.Number, ExpresionType.String, TokenType.Plus), ExpresionType.String},
            };
        }

        public override ExpresionType GetType()
        {
            var leftType = this.LeftExpression.GetType();
            var rightType = this.RightExpression.GetType();
            if (_typeRules.TryGetValue((leftType, rightType, Operation.TokenType), out var resultType))
            {
                return resultType;
            }

            throw new ApplicationException($"Cannot apply operator '{Operation.Lexeme}' to operands of type {leftType} and {rightType}");
        }

        public override string GenerateCode() =>
        $"{this.LeftExpression.GenerateCode()} {this.Operation.Lexeme} {this.RightExpression.GenerateCode()}";
    }
}
