using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class ConstantExpresion : Expresion
    {
        public ExpresionType Type { get; set; }
        public Token Token { get; set; }

        public ConstantExpresion(ExpresionType type, Token token)
        {
            Type = type;
            Token = token;
        }

        public override ExpresionType GetType()
        {
            return Type;
        }
    }
}
