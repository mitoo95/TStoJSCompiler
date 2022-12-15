using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class BinaryParameter : ExpresionType
    {
        public IdExpression Id { get; set; }
        public ExpresionType ExpresionType { get; set; }
        public BinaryParameter(string lexeme, TokenType tokenType, IdExpression id, ExpresionType expresionType) 
            : base(lexeme, tokenType)
        {
            Id = id;
            ExpresionType = expresionType;
        }
    }
}
