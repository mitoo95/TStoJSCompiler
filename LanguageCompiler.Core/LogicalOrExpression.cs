using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class LogicalOrExpression : BinaryExpression
    {
        private readonly Dictionary<(ExpresionType, ExpresionType), ExpresionType> _typeRules;
        public LogicalOrExpression(Expresion leftExpression, Expresion rightExpression)
            : base(leftExpression, rightExpression)
        {
            _typeRules = new Dictionary<(ExpresionType, ExpresionType), ExpresionType>
            {
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

            throw new ApplicationException($"Cannot apply operator '||' to operands of type {leftType} and {rightType}");
        }

        public override string GenerateCode() =>
        $"{this.LeftExpression.GenerateCode()} || {this.RightExpression.GenerateCode()}";
    }
}
