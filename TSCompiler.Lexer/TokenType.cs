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
        PlusPlus,
        Minus,
        MinusMinus,
        Mult,
        Division,
        Modulus,
        EOF,
        LessOrEqualThan,
        GreaterOrEqualThan,
        LessThan,
        GreaterThan,
        Equal,
        Id,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,
        StringKeyword,
        NumberConst,
        NumberKeyword,
        BooleanKeyword,
        VoidKeyword,
        NullKeyword,
        UndefinedKeyword,
        ForKeyword,
        TrueKeyword,
        FalseKeyword,
        LetKeyword,
        ClassKeyword,
        FunctionKeyword,
        VarKeyword,
        ConstKeyword
    }
}
