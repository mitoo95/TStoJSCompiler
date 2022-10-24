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
                        return CreateToken(TokenType.IntConst, input.Position.Column, input.Position.Line, lexeme.ToString());
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
                    return CreateToken(TokenType.FloatConst, input.Position.Column, input.Position.Line, lexeme.ToString());
                }

                switch (currentChar)
                {
                    case '+':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.Plus, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '-':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.Minus, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '*':
                        lexeme.Append(currentChar);
                        return CreateToken(TokenType.Mult, input.Position.Column, input.Position.Line, lexeme.ToString());
                    case '<':
                        lexeme.Append(currentChar);
                        var nextChar = PeekNextChar();
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
                        return CreateToken(TokenType.Equal, input.Position.Column, input.Position.Line, lexeme.ToString());
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