using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class DeclarationAssignation : Statement
    {
        public ExpresionType ExpresionType { get; set; }
        public Statement Statement { get; set; }

        public DeclarationAssignation(ExpresionType expresionType, Statement statement)
        {
            ExpresionType = expresionType;
            Statement = statement;
        }
    }
}
