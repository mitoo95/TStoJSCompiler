using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class ForStatement : Statement
    {
        public Statement Declaration { get; set; }
        public Expresion BooleanExpression { get; set; }
        public Statement Statement { get; set; }
        public Statement Block { get; set; }


        public ForStatement(Statement declaration, Expresion booleanExpression, Statement statement, Statement block)
        {
            Declaration = declaration;
            BooleanExpression = booleanExpression;
            Statement = statement;
            Block = block;
        }
    }
}
