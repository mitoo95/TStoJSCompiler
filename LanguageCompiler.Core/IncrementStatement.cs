using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class IncrementStatement : Statement
    {
        public IncrementStatement(IdExpression id)
        {
            Id = id;
        }

        public IdExpression Id { get; set; }

    }
}
