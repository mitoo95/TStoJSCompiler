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
        public ExpresionType Type { get; set; }

        public IdExpression(string name, ExpresionType type)
        {
            Name = name;
            Type = type;
        }
    }
}
