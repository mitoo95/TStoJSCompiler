﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class ArrayType : ExpresionType
    {
        public ExpresionType Of { get; }
        private readonly int _size;
        public ArrayType(string lexeme, TokenType tokenType, ExpresionType of, int size) 
            : base(lexeme, tokenType)
        {
            Of = of;
            _size = size;
        }

        
    }
}