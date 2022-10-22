using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Lexer
{
    public enum TokenType
    {
        Plus,
        Minus,
        Mult,
        EOF,
        LessOrEqualThan,
        LessThan,
        Equal,
        Id,
        IfKeyword,
        ElseKeyword,
        IntKeyword,
        FloatKeyword,
        StringKeyword,
        IntConst,
        FloatConst
    }
}
