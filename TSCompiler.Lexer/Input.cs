using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Lexer
{
    public readonly struct Input
    {
        public string Source { get; }
        public int Length { get; }
        public Position Position { get; }

        public Input(string source) : this(source, Position.Pos, source.Length)
        {
            Source = source;
        }
        public Input(string source, Position pos, int len)
        {
            Source = source;
            Length = len;
            Position = pos;
        }
        public Result<char> NextChar()
        {
            if (Length == 0)
            {
                return Result.Empty<char>(this);
            }

            var @char = Source[Position.AbsolutePos];
            return Result.Value(@char, new Input(Source, Position.MovePointer(@char), Length - 1));
        }
    }
}
