using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class LogicalAndExpression : BinaryExpression
    {
        public LogicalAndExpression(Expresion leftExpression, Expresion rightExpression) 
            : base(leftExpression, rightExpression)
        {
        }

        public override ExpresionType GetType()
        {
            throw new NotImplementedException();
        }
    }
}
