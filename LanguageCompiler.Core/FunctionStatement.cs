using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class FunctionStatement : Statement
    {
        public List<ExpresionType>? FirstExpression { get; set; }
        public ExpresionType SecondExpression { get; set; }
        public Statement Statement { get; set; }

        public FunctionStatement(List<ExpresionType>? firstExpression, ExpresionType secondExpression, Statement statement)
        {
            FirstExpression = firstExpression;
            SecondExpression = secondExpression;
            Statement = statement;
        }
    }
}
