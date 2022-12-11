using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageCompiler.Core
{
    public class WhileStatement : Statement
    {
        public Expresion BooleanExpression { get; set; }
        public Statement Statement { get; set; }

        public WhileStatement(Expresion booleanExpression, Statement statement)
        {
            BooleanExpression = booleanExpression;
            Statement = statement;
        }
    }
}
