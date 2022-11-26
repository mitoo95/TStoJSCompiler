﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageCompiler.Core
{
    public class SequenceStatement : Statement
    {
        public Statement Current { get; set; }
        public Statement Next { get; set; }

        public SequenceStatement(Statement current, Statement next)
        {
            Current = current;
            Next = next;
        }

        //statement actual y siguiente

    }
}
