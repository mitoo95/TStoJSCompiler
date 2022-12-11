using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class ReturnStatement : Statement
    {
        public Expresion? Expression { get; set; }

        public ReturnStatement(Expresion? expression)
        {
            Expression = expression;
        }
    }
}
