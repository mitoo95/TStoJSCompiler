using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageCompiler.Core
{
    public class ReturnCode : Statement
    {
        public Statement Block { get; set; }
        public Statement Main { get; set; }
        public Statement Imports { get; set; }

        public ReturnCode(Statement imports, Statement block, Statement main)
        {
            Imports = imports;
            Block = block;
            Main = main;
        }
    }
}
