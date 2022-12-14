using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageCompiler.Core
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string? Lexeme { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public override string ToString()
        {
            return $"Lexema: {Lexeme}, Tipo: {TokenType}, Fila: {Line}, Columna: {Column}";
        }
    }
}
