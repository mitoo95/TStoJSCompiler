using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class FunctionStatement : Statement
    {
        public Expresion FirstExpression { get; set; }
        public Expresion SecondExpression { get; set; }
        public Statement Statement { get; set; }

        public FunctionStatement(Expresion firstExpression, Expresion secondExpression, Statement statement)
        {
            FirstExpression = firstExpression;
            SecondExpression = secondExpression;
            Statement = statement;
        }
    }
}
