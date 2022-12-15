﻿
namespace TSCompiler.Core
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
        ArrayKeyword,
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
        SingleQuote,
        LineComment,
        BlockCommentStart,
        BlockCommentEnd,
        ArrowFunction,
        ImportKeyword,
        FromKeyword,
        ModuleKeyword,
        MainKeyword,
        BasicType,
        ComplexType,
        VarType,
        Console,
        Log,
        Dot,
        StringConst,
    }
}
