using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class ArithmeticExpression : BinaryExpression
    {
        public ArithmeticExpression(Expresion leftExpression, Expresion rightExpression, Token operation) 
            : base(leftExpression, rightExpression)
        {
        }

        public override ExpresionType GetType()
        {
            throw new NotImplementedException();
        }
    }
}
