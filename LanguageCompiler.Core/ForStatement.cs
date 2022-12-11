using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class ForStatement : Statement
    {
        public Expresion BooleanExpression { get; set; }

        public Statement Statement { get; set; }

        public ForStatement(Expresion binaryExpression, Statement statement)
        {
            BooleanExpression = binaryExpression;
            Statement = statement;
        }
    }
}
