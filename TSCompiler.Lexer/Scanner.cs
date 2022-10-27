using System.Text;

namespace TSCompiler.Lexer
{
    public class Scanner
    {
        private Input input;
        private readonly Dictionary<string, TokenType> keywords;

        public Scanner(Input input)
        {
            this.input = input;
            this.keywords = new Dictionary<string, TokenType>
            {
                {"number", TokenType.NumberKeyword },
                {"string", TokenType.StringKeyword },
                {"boolean", TokenType.BooleanKeyword },
                {"void", TokenType.VoidKeyword },
                {"null", TokenType.NullKeyword },
                {"undefined", TokenType.UndefinedKeyword },
                {"if", TokenType.IfKeyword },
                {"else", TokenType.ElseKeyword },
                {"while", TokenType.WhileKeyword },
                {"for", TokenType.ForKeyword },
                {"true", TokenType.TrueKeyword },
                {"false", TokenType.FalseKeyword },
                {"let", TokenType.LetKeyword },
                {"var", TokenType.VarKeyword },
                {"const", TokenType.ConstKeyword },
                {"class", TokenType.ClassKeyword },
                {"function", TokenType.FunctionKeyword },
                {"break", TokenType.BreakKeyword },
                {"return", TokenType.ReturnKeyword },
                {"continue", TokenType.ContinueKeyword },
                {"in", TokenType.InKeyword },
                {"of", TokenType.OfKeyword },
                {"do", TokenType.DoKeyword },
            };
        }

        public Token GetNextToken()
        {
            var lexeme = new StringBuilder();
            var currentChar = GetNextChar();

            while (true)
            {
                while (char.IsWhiteSpace(currentChar) || currentChar == '\n')
                {
                    currentChar = GetNextChar();
                }

                if (char.IsLetter(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();
                    while (char.IsLetterOrDigit(currentChar))
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }

                    if (this.keywords.ContainsKey(lexeme.ToString()))
                    {
                        return CreateToken(this.keywords[lexeme.ToString()], input.Position.Column, input.Position.Line, lexeme.ToString());
                    }
                    return CreateToken(TokenType.Id, input.Position.Column, input.Position.Line, lexeme.ToString());
                }
                else if (char.IsDigit(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();

                    while (char.IsDigit(currentChar))
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }

                    if (currentChar != '.')
                    {
                        return CreateToken(TokenType.NumberConst, input.Position.Column, input.Position.Line, lexeme.ToString());
                    }

                    currentChar = GetNextChar();
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();

                    while (char.IsDigit(currentChar))
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }

                    return CreateToken(TokenType.NumberConst, input.Position.Column, input.Position.Line, lexeme.ToString());
                }

                switch (currentChar)
                {
                    case '+':
                        lexeme.Append(currentChar);
                        var nextChar = PeekNextChar();
                        if (nextChar == '+')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.PlusPlus, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.PlusEqual, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.Plus, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '-':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '-')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.MinusMinus, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.MinusEqual, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.Minus, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '*':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.MultEqual, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.Mult, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '/':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.DivisionEqual, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.Division, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '<':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.LessOrEqualThan, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.LessThan, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '>':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.GreaterOrEqualThan, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.GreaterThan, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '=':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.Equality, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.Equal, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '!':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.NotEquals, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.Not, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '&':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '&')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.And, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.BitwiseAndEqual, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.BitwiseAnd, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '|':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '|')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.Or, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.BitwiseOrEqual, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.BitwiseOr, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '^':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.BitwiseXorEqual, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.BitwiseXor, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '%':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.ModulusEqual, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.Modulus, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case ':':
                        lexeme.Append(currentChar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return CreateToken(TokenType.ColonEqual, input.Position.Column, input.Position.Line, lexeme.ToString());
                        }
                        return CreateToken(TokenType.Colon, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case ',':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.Comma, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case ';':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.Semicolon, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '{':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.LeftCurly, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '}':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.RightCurly, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '(':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.LeftParenthesis, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case ')':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.RightParenthesis, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '[':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.LeftBracket, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case ']':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.RightBracket, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '\0':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.EOF, input.Position.Column, input.Position.Line, lexeme.ToString());
                    default:
                        throw new ApplicationException($"El lexema {lexeme} es invalido en la fila: {input.Position.Line} columna: {input.Position.Column}");
                }
            }
        }

        private char GetNextChar()
        {
            var next = input.NextChar();
            input = next.Reminder;
            return next.Value;
        }

        private char PeekNextChar()
        {
            var next = input.NextChar();
            return next.Value;
        }

        private Token CreateToken(TokenType _Token, int _Column, int _Line, string _Lexeme)
        {
            return new Token
            {
                TokenType = _Token,
                Column = _Column,
                Line = _Line,
                Lexeme = _Lexeme.ToString()
            };
        }
    }
}