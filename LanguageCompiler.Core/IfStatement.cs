using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class IfStatement : Statement
    {
        public Expresion BooleanExpression { get; set; }
        public Statement TrueStatement { get; set; }
        //propiedad del else, puede ser nulo en caso que no haya else
        public Statement? FalseStatement { get; set; }

        public IfStatement(Expresion booleanExpression, Statement trueStatement, Statement falseStatement)
        {
            BooleanExpression = booleanExpression;
            TrueStatement = trueStatement;
            FalseStatement = falseStatement;
        }
    }
}
