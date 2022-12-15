using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCompiler.Core
{
    public class DecrementStatement : Statement
    {
        public IdExpression Id { get; set; }

        public DecrementStatement(IdExpression id)
        {
            Id = id;
        }

        public override void ValidateSemantic()
        {
            /*throw new NotImplementedException();*/
        }

        public override string GenerateCode() => $"{this.Id.GenerateCode()}--";
    }
}
