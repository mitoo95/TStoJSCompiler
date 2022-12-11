using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using TSCompiler.Core;
using TSCompiler.Lexer;

namespace TSCompiler.Parser
{
    public class Parser
    {
        public readonly IScanner scanner;
        private Token lookAhead;
        public Parser(IScanner scanner)
        {
            this.scanner = scanner;
            this.Move();
        }

        private void Move()
        {
            this.lookAhead = this.scanner.GetNextToken();
        }

        public /*Statement*/ void Parse()
        {
            /*return*/ Code();
        }

        private /*Statement*/ void Code()
        {
            Imports(); //no va en arbol
            Block();
            Main();
            //return statements;
            
        }

        private void Block()
        {
            
            if (this.lookAhead.TokenType == TokenType.VarKeyword || this.lookAhead.TokenType == TokenType.ConstKeyword || this.lookAhead.TokenType == TokenType.LetKeyword)
            {
                Declarations();
                Block();
            }
            if (this.lookAhead.TokenType == TokenType.Id || this.lookAhead.TokenType == TokenType.IfKeyword
            || this.lookAhead.TokenType == TokenType.WhileKeyword || this.lookAhead.TokenType == TokenType.ForKeyword
            || this.lookAhead.TokenType == TokenType.ReturnKeyword || this.lookAhead.TokenType == TokenType.BreakKeyword
            || this.lookAhead.TokenType == TokenType.ContinueKeyword || this.lookAhead.TokenType == TokenType.PlusPlus
            || this.lookAhead.TokenType == TokenType.MinusMinus || this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                Statements();
                Block();
            }
            //if(this.lookAhead.TokenType == TokenType.LineComment || this.lookAhead.TokenType == TokenType.BlockCommentStart)
            //{
            //    Comments();
            //    Block();
            //}Preguntar al ing el lunes


            //εps

        }

        private void Main()
        {
            if (this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                Match(TokenType.FunctionKeyword);
                Match(TokenType.MainKeyword);
                Match(TokenType.LeftParenthesis);
                Match(TokenType.RightParenthesis);
                Match(TokenType.LeftCurly);
                Block();
                Match(TokenType.RightCurly);
            }
        }

        private void Statements()
        {
            if (this.lookAhead.TokenType == TokenType.Id || this.lookAhead.TokenType == TokenType.IfKeyword
                || this.lookAhead.TokenType == TokenType.WhileKeyword || this.lookAhead.TokenType == TokenType.ForKeyword
                || this.lookAhead.TokenType == TokenType.ReturnKeyword || this.lookAhead.TokenType == TokenType.BreakKeyword
                || this.lookAhead.TokenType == TokenType.ContinueKeyword || this.lookAhead.TokenType == TokenType.PlusPlus
                || this.lookAhead.TokenType == TokenType.MinusMinus || this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                Statement();
                Statements();
            }
            //eps
            //revisar 
        }

        private void Statement()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Id:
                    Id_Statement();
                    //AssignationIncrementDecrementStatement();
                    break;
                case TokenType.WhileKeyword:
                    WhileStatement();
                    break;
                case TokenType.IfKeyword:
                    IfStatement();
                    break;
                case TokenType.ForKeyword:
                    return ForStatement();
                case TokenType.ReturnKeyword:
                    return ReturnStatement();
                case TokenType.BreakKeyword:
                    return BreakStatement();
                case TokenType.ContinueKeyword:
                    return ContinueStatement();
                case TokenType.PlusPlus:
                    return IncrementStatement();
                case TokenType.MinusMinus:
                    return DecrementStatement();
                case TokenType.FunctionKeyword:
                    return FunctionStatement();
            }
        }

        private void Id_Statement()
        {
            Match(TokenType.Id);
            Id_Statement_Prime();
        }

        private void Id_Statement_Prime()
        {
            if (this.lookAhead.TokenType == TokenType.Equal)
            {
                AssignationStatement();
            }
            if(this.lookAhead.TokenType == TokenType.PlusPlus)
            {
                IncrementStatement();
            }
            if(this.lookAhead.TokenType == TokenType.MinusMinus)
            {
                DecrementStatement();
            }
<<<<<<< Updated upstream

=======
            return null;
>>>>>>> Stashed changes
        }

        private void DecrementStatement()
        {
            Match(TokenType.MinusMinus);
<<<<<<< Updated upstream
            DecrementStatementPrime();
        }

        private void DecrementStatementPrime()
        {
            if (this.lookAhead.TokenType == TokenType.Id)
            {
                Match(TokenType.Id);
                Match(TokenType.Semicolon);
            }
            else
            {
                Match(TokenType.Semicolon);
            }
=======
            Match(TokenType.Semicolon);
>>>>>>> Stashed changes
        }

        private void IncrementStatement()
        {
            Match(TokenType.PlusPlus);
<<<<<<< Updated upstream
            IncrementStatementPrime();
        }

        private void IncrementStatementPrime()
        {
            if(this.lookAhead.TokenType == TokenType.Id)
            {
                Match(TokenType.Id);
                Match(TokenType.Semicolon);
            }
            else
            {
                Match(TokenType.Semicolon);
            }
        }

        private void AssignationStatement()
=======
            Match(TokenType.Semicolon);
            //que retornamos aqui?
        }

        private Statement AssignationStatement(IdExpression id)
>>>>>>> Stashed changes
        {
            AssignationStatementPrime();
        }

        private void AssignationStatementPrime()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Equal:
                    AssignEquals();
                    break;
                case TokenType.BitwiseOrEqual:
                    Match(TokenType.BitwiseOrEqual);
                    LogicalOrExpression();
                    break;
                case TokenType.BitwiseAndEqual:
                    Match(TokenType.BitwiseAndEqual);
                    LogicalOrExpression();
                    break;
                case TokenType.BitwiseXorEqual:
                    Match(TokenType.BitwiseXorEqual);
                    LogicalOrExpression();
                    break;
                case TokenType.PlusEqual:
                    Match(TokenType.PlusEqual);
                    LogicalOrExpression();
                    break;
                case TokenType.MinusEqual:
                    Match(TokenType.MinusEqual);
                    LogicalOrExpression();
                    break;
                case TokenType.DivisionEqual:
                    Match(TokenType.DivisionEqual);
                    LogicalOrExpression();
                    break;
                default:
                    Match(TokenType.MultEqual);
                    LogicalOrExpression();
                    break;
            }
        }

        private void AssignEquals()
        {
            Match(TokenType.Equal);
            AssignEqualsPrime();
        }

        private void AssignEqualsPrime()
        {
            if(this.lookAhead.TokenType == TokenType.LeftBracket)
            {
                Match(TokenType.LeftBracket);
                Expression();
                Match(TokenType.RightBracket);
            }
            LogicalOrExpression();
            Match(TokenType.Semicolon);
        }

        private void FunctionStatement()
        {
            if(this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                Match(TokenType.FunctionKeyword);
                Match(TokenType.Id);
                Match(TokenType.LeftParenthesis);
                var expr = Params();
                Match(TokenType.RightParenthesis);
                Match(TokenType.Colon);
                var expr2 = Type();
                Match(TokenType.LeftCurly);
                var stmt = Block();
                Match(TokenType.RightCurly);
                return new FunctionStatement(expr, expr2, stmt);
            }
            //WIP
        }

        private List<Expresion> Params()
        {
            var expressions = new List<Expresion>();
            expressions.Add(Param()); 
            expressions.AddRange(Params());
            return expressions;
        }

        private List<Expresion> ParamsPrime()
        {
            var expressions = new List<Expresion>();
            if(this.lookAhead.TokenType == TokenType.Comma)
            {
                Match(TokenType.Comma);
                expressions = Params();

            }
            return expressions;
        }

        private Expresion Param()
        {
            Match(TokenType.Id);
            Match(TokenType.Colon);
            Type();
        }

        private Statement ContinueStatement()
        {
            Match(TokenType.ContinueKeyword);
            Match(TokenType.Semicolon);
            return null;
        }

        private Statement BreakStatement()
        {
            Match(TokenType.BreakKeyword);
            Match(TokenType.Semicolon);
            return null;
        }

        private Statement ReturnStatement()
        {
            Match(TokenType.ReturnKeyword);
            var expr = LogicalOrExpression();
            Match(TokenType.Semicolon);
            return new ReturnStatement(expr);
            //WIP
        }

<<<<<<< Updated upstream
        private void ForStatement()
=======
        //pregunta
        private Statement ForStatement()
>>>>>>> Stashed changes
        {
            Match(TokenType.ForKeyword);
            Match(TokenType.LeftParenthesis);
            var expression = LogicalOrExpression();
            Match(TokenType.RightParenthesis);
            return new ForStatement(expression, Block());
        }

        private void IfStatement()
        {
            Match(TokenType.IfKeyword);
            Match(TokenType.LeftParenthesis);
            LogicalOrExpression();
            Match(TokenType.RightParenthesis);
            Match(TokenType.LeftCurly);
            Block();
            Match(TokenType.RightCurly);
            IfStatementPrime();
        }

        private void IfStatementPrime()
        {
            if(this.lookAhead.TokenType == TokenType.ElseKeyword)
            {
                Match(TokenType.ElseKeyword);
                Match(TokenType.LeftCurly);
                Block();
                Match(TokenType.RightCurly);
            }
            //eps
        }

        private void LogicalOrExpression()
        {
            LogicalAndExpression();
            LogicalOrExpressionPrime();
        }

        private void LogicalOrExpressionPrime()
        {
            if(this.lookAhead.TokenType == TokenType.Or)
            {
                Match(TokenType.Or);
                LogicalAndExpression();
                LogicalOrExpressionPrime();
            }
            //eps
        }

        private void LogicalAndExpression()
        {
            EqualityExpression();
            LogicalAndExpressionPrime();
        }

        private void LogicalAndExpressionPrime()
        {
            if(this.lookAhead.TokenType == TokenType.And)
            {
                Match(TokenType.And);
                EqualityExpression();
                LogicalAndExpressionPrime();
            }
            //eps
        }

        private void EqualityExpression()
        {
            RelationalExpression();
            EqualityExpressionPrime();
        }

        private void EqualityExpressionPrime()
        {
            if(this.lookAhead.TokenType == TokenType.Equality)
            {
                Match(TokenType.Equality);
                RelationalExpression();
                EqualityExpressionPrime();
            }
            if(this.lookAhead.TokenType == TokenType.NotEquals)
            {
                Match(TokenType.NotEquals);
                RelationalExpression();
                EqualityExpressionPrime();
            }
            //eps
        }

        private void RelationalExpression()
        {
            Expression();
            RelationalExpressionPrime();
        }

        private void RelationalExpressionPrime()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.GreaterThan:
                    Match(TokenType.GreaterThan);
                    Expression();
                    break;
                case TokenType.LessThan:
                    Match(TokenType.LessThan);
                    Expression();
                    break;
                case TokenType.GreaterOrEqualThan:
                    Match(TokenType.GreaterOrEqualThan);
                    Expression();
                    break;
                case TokenType.LessOrEqualThan:
                    Match(TokenType.LessOrEqualThan);
                    Expression();
                    break;
                default:
                    //eps
                    break;
            }
        }

        private void Expression()
        {
            Term();
            ExpressionPrime();
        }

        private void ExpressionPrime()
        {
            if(this.lookAhead.TokenType == TokenType.Plus)
            {
                Match(TokenType.Plus);
                Term();
                ExpressionPrime();
            }
            if(lookAhead.TokenType == TokenType.Minus)
            {
                Match(TokenType.Minus);
                Term();
                ExpressionPrime();
            }
            //eps
        }

        private void Term()
        {
            Factor();
            TermPrime();
        }

        private void TermPrime()
        {
            if (this.lookAhead.TokenType == TokenType.Mult)
            {
                Match(TokenType.Mult);
                Factor();
                TermPrime();
            }
            if (lookAhead.TokenType == TokenType.Division)
            {
                Match(TokenType.Division);
                Factor();
                TermPrime();
            }
            //eps
        }

        private void Factor()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LeftParenthesis:
                    Match(TokenType.LeftParenthesis);
                    var expr = Expression();
                    Match(TokenType.RightParenthesis);
                    return expr;
                case TokenType.NumberConst:
                    var token = this.lookAhead;
                    Match(TokenType.NumberConst);
                    var constant = new ConstantExpresion(ExpresionType.Number, token);
                    break;
                case TokenType.TrueKeyword:
                    token = this.lookAhead;
                    Match(TokenType.TrueKeyword);
                    var constant = new ConstantExpresion(ExpresionType.Boolean, token);
                    break;
                case TokenType.FalseKeyword:
                    token = this.lookAhead;
                    Match(TokenType.FalseKeyword);
                    var constant = new ConstantExpresion(ExpresionType.Boolean, token);
                    break;
                default:
                    Match(TokenType.Id);
                    break;
            }
        }

        private void WhileStatement()
        {
            Match(TokenType.WhileKeyword);
            Match(TokenType.LeftParenthesis);
            LogicalOrExpression();
            Match(TokenType.RightParenthesis);
            Block();
        }

        private void Declarations()
        {
            if(this.lookAhead.TokenType == TokenType.VarKeyword || this.lookAhead.TokenType == TokenType.ConstKeyword || this.lookAhead.TokenType == TokenType.LetKeyword)
            {
                Declaration();
                Declarations();
            }
            //eps
        }

        private void Declaration()
        {
            VarDeclaration();
            Match(TokenType.Id);
            DeclarationPrime();
        }

        private void DeclarationPrime()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Colon:
                    Match(TokenType.Colon);
                    Type();
                    DeclarationPrimePrime();
                    break;

                default:
                    Match(TokenType.Equal);
                    Match(TokenType.LeftParenthesis);
                    Params();
                    Match(TokenType.RightParenthesis);
                    Match(TokenType.Colon);
                    Type();
                    Match(TokenType.ArrowFunction);
                    Match(TokenType.LeftCurly);
                    Block();
                    Match(TokenType.RightCurly);
                    break;
            }
        }

        private void DeclarationPrimePrime()
        {
            if(this.lookAhead.TokenType == TokenType.LeftBracket)
            {
                Match(TokenType.LeftBracket);
                Expression();
                Match(TokenType.RightBracket);
                Match(TokenType.Semicolon);
            }
            if(this.lookAhead.TokenType == TokenType.Equal)
            {
                AssignEquals();
            }
            else
            {
                Match(TokenType.Semicolon);
            }
        }

        private Expresion Type()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.NumberConst:
                    var token = this.lookAhead;
                    Match(TokenType.NumberConst);
                    var constant = new ConstantExpresion(ExpresionType.Number, token);
                    TypePrime();
                    break;
                case TokenType.BooleanKeyword:
                    token = this.lookAhead;
                    Match(TokenType.BooleanKeyword);
                    constant = new ConstantExpresion(ExpresionType.Boolean, token);
                    TypePrime();
                    break;
                default:
                    token = this.lookAhead;
                    Match(TokenType.StringKeyword);
                    constant = new ConstantExpresion(ExpresionType.String, token);
                    TypePrime();
                    break;
            }
        }

        private void TypePrime()
        {
            if(this.lookAhead.TokenType == TokenType.LeftBracket)
            {
                Match(TokenType.LeftBracket);
                Factor(); //length array
                Match(TokenType.RightBracket);
                TypePrime();
            }
            //eps
        }

        private void VarDeclaration()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.VarKeyword:
                    Match(TokenType.VarKeyword);
                    break;
                case TokenType.LetKeyword:
                    Match(TokenType.LetKeyword);
                    break;
                default:
                    Match(TokenType.ConstKeyword);
                    break;
            }
        }

        private void Comments()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LineComment:
                    Match(TokenType.LineComment);
                    //text? WIP
                    break;
                default:
                    Match(TokenType.BlockCommentStart);
                    //text? WIP
                    Match(TokenType.BlockCommentEnd);
                    break;
            }
            Comment();
            Comments();
        }

        private void Comment()
        {
            throw new NotImplementedException();
        }

        private void Imports()
        {
            if(this.lookAhead.TokenType == TokenType.ImportKeyword)
            {
                Import();
                Imports();
            }
            //eps
        }

        private void Import()
        {
            Match(TokenType.ImportKeyword);
            Match(TokenType.Id);
            Match(TokenType.FromKeyword);
            Match(TokenType.SingleQuote);
            Match(TokenType.ModuleKeyword);
            Match(TokenType.SingleQuote);
            Match(TokenType.Semicolon);
        }



        private void Match(TokenType tokenType)
        {
            if(this.lookAhead.TokenType != tokenType)
            {
                throw new ApplicationException($"Syntax Error. Expected {tokenType} but found {this.lookAhead.TokenType}.");
            }
            Move();
        }
    }
}