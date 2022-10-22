using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Lexer
{
    public readonly struct Position
    {
        public int AbsolutePos { get; }
        public int Line { get; }
        public int Column { get; }

        public Position(int absolute, int line, int column)
        {
            AbsolutePos = absolute;
            Line = line;
            Column = column;
        }

        public static Position Pos = new Position(0, 0, 0);

        public Position MovePointer(char @char)
        {
            return @char == '\n' ? new Position(AbsolutePos + 1, Line + 1, 1) : new Position(AbsolutePos + 1, Line, Column + 1);
        }
    }
}
