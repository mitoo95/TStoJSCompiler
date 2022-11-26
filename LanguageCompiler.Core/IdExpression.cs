using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageCompiler.Core
{
    public class IdExpression
    {
        //nombre del identificador
        public string Name { get; set; }
        //que tipo es el identificador
        public ExpressionType Type { get; set; }

        public IdExpression(string name, ExpressionType type)
        {
            Name = name;
            Type = type;
        }

    }
}
