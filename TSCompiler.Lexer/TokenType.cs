﻿using System;
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
        PlusEqual,
        Minus,
        MinusMinus,
        MinusEqual,
        Mult,
        MultEqual,
        Division,
        DivisionEqual,
        Modulus,
        ModulusEqual,
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
        ConstKeyword,
        Equality,
        NotEquals,
        Not,
        And,
        BitwiseAnd,
        BitwiseAndEqual,
        Or,
        BitwiseOr,
        BitwiseOrEqual,
        BitwiseXor,
        BitwiseXorEqual,
        Colon,
        ColonEqual,
        Comma,
        InKeyword,
        OfKeyword,
        DoKeyword,
        BreakKeyword,
        ReturnKeyword,
        ContinueKeyword,
        Semicolon,
        LeftCurly,
        RightCurly,
        LeftParenthesis,
        RightParenthesis,
        LeftBracket,
        RightBracket,
    }
}
