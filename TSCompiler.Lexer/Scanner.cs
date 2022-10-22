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
                {"if", TokenType.IfKeyword },
                {"else", TokenType.ElseKeyword },
                {"int", TokenType.IntKeyword },
                {"float", TokenType.FloatKeyword },
                {"string", TokenType.StringKeyword },
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
                        return new Token
                        {
                            TokenType = this.keywords[lexeme.ToString()],
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    }

                    return new Token
                    {
                        TokenType = TokenType.Id,
                        Column = input.Position.Column,
                        Line = input.Position.Line,
                        Lexeme = lexeme.ToString()
                    };
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
                        return new Token
                        {
                            TokenType = TokenType.IntConst,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
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
                    return new Token
                    {
                        TokenType = TokenType.FloatConst,
                        Column = input.Position.Column,
                        Line = input.Position.Line,
                        Lexeme = lexeme.ToString()
                    };
                }

                switch (currentChar)
                {
                    case '+':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.Plus,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '-':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.Minus,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '*':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.Mult,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '<':
                        lexeme.Append(currentChar);
                        var nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {
                                TokenType = TokenType.LessOrEqualThan,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.LessThan,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '=':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.Equal,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '\0':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.EOF,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };

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
    }
}