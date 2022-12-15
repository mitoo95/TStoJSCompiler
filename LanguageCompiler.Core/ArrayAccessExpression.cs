using System;
using System.Linq.Expressions;

namespace TSCompiler.Core
{
	public class ArrayAccessExpression : Expresion
	{
        public ExpresionType Type { get; }
        public IdExpression Id { get; }
        public Expression Index { get; }

        public ArrayAccessExpression(ExpresionType type, Token token, IdExpression id, Expression index)
        {
            Type = type;
            Id = id;
            Index = index;
        }

        public override ExpresionType GetType()
        {
            return Type;
        }
    }
}

