using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class AssignationStatement : Statement
    {

        public IdExpression Id { get; set; }
        public Expresion Expression { get; set; }

        public AssignationStatement(IdExpression id, Expresion expression)
        {
            Id = id;
            Expression = expression;
        }

    }
}
