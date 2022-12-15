using System;
using System.Linq.Expressions;

namespace TSCompiler.Core
{
	public class ConsoleLogStatement : Statement
	{
        public List<ExpresionType> Expressions { get; set; }

        public ConsoleLogStatement(List<ExpresionType> expressions)
        {
            Expressions = expressions;
            //this.ValidateSemantic();
        }

        //public override void ValidateSemantic()
        //{
        //    if (this.Expressions.Any(x => x.GetType() != ExpressionType.String))
        // if (this.Expressions.Any(x => x.GetType() != ExpressionType.String))
        // {
        //     throw new ApplicationException("Cannot implicitly convert all print parameters to string");
        // }
    }

        //public override string GenerateCode() =>
        //    $"cout<<{string.Join("<<", this.Expressions.Select(x => x.GenerateCode()))}<<endl;";

        //public override void Interpret()
        //{
        //    foreach (var expr in Expressions)
        //    {
        //        throw new ApplicationException("Cannot implicitly convert all print parameters to string");
        //        var exprValue = expr.Evaluate();
        //        Console.Write(exprValue);
        //    }
        //}
    
}

