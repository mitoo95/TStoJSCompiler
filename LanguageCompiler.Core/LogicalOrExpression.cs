using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageCompiler.Core
{
    public class LogicalOrExpression : BinaryExpression
    {
        public LogicalOrExpression(Expresion leftExpression, Expresion rightExpression) 
            : base(leftExpression, rightExpression)
        {

        }
    }
}
