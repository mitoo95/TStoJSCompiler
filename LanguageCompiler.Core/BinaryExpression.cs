
namespace TSCompiler.Core
{
    public abstract class BinaryExpression : Expresion
    {

        public Expresion LeftExpression { get; set; }
        public Expresion RightExpression { get; set; }

        protected BinaryExpression(Expresion leftExpression, Expresion rightExpression)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
        }

    }
}
