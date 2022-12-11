using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public abstract class BinaryExpression : Expresion
    {

        public Expresion LeftExpression { get; set; }
        public Expresion RightExpression { get; set; }

        protected BinaryExpression(Expresion leftExpression, Expresion rightExpression)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
        }

    }
}
