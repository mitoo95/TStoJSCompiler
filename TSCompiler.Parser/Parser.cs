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

        public Statement Parse()
        {
            return Code();
        }

        private Statement Code()
        {
            Imports(); //no va en arbol
            Block();
            Main();
            return new ReturnCode(Imports(), Block(), Main());
            
        }

        private Statement Block()
        {
            IdExpression id = null;
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
                id = new IdExpression(this.lookAhead.Lexeme, null);
                return new SequenceStatement(Statements(id), Block());
            }
            //if(this.lookAhead.TokenType == TokenType.LineComment || this.lookAhead.TokenType == TokenType.BlockCommentStart)
            //{
            //    Comments();
            //    Block();
            //}Preguntar al ing el lunes


            //εps
            return null;
        }

        private Statement Main()
        {
            if (this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                Match(TokenType.FunctionKeyword);
                Match(TokenType.MainKeyword);
                Match(TokenType.LeftParenthesis);
                Match(TokenType.RightParenthesis);
                Match(TokenType.LeftCurly);
                var stmt = Block();
                Match(TokenType.RightCurly);
                return stmt;
            }
            return null;
        }

        private Statement Statements(IdExpression id)
        {
            if (this.lookAhead.TokenType == TokenType.Id || this.lookAhead.TokenType == TokenType.IfKeyword
                || this.lookAhead.TokenType == TokenType.WhileKeyword || this.lookAhead.TokenType == TokenType.ForKeyword
                || this.lookAhead.TokenType == TokenType.ReturnKeyword || this.lookAhead.TokenType == TokenType.BreakKeyword
                || this.lookAhead.TokenType == TokenType.ContinueKeyword || this.lookAhead.TokenType == TokenType.PlusPlus
                || this.lookAhead.TokenType == TokenType.MinusMinus || this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                return new SequenceStatement(Statement(id), Statements(id));
            }
            return null;
            //eps
            //revisar 
        }

        private Statement Statement(IdExpression id)
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Id:
                    return Id_Statement(id);
                case TokenType.WhileKeyword:
                    return WhileStatement();
                case TokenType.IfKeyword:
                    return IfStatement();
                case TokenType.ForKeyword:
                    return ForStatement();
                case TokenType.ReturnKeyword:
                    return ReturnStatement();
                case TokenType.BreakKeyword:
                    return BreakStatement();
                case TokenType.ContinueKeyword:
                    return ContinueStatement();
                case TokenType.PlusPlus:
                    return IncrementStatement(id);
                case TokenType.MinusMinus:
                    return DecrementStatement(id);
                case TokenType.FunctionKeyword:
                    return FunctionStatement();
            }
            return null;
        }

        private Statement Id_Statement(IdExpression id)
        {
            Match(TokenType.Id);
            var statement = Id_Statement_Prime(id);
            return statement;
        }

        private Statement Id_Statement_Prime(IdExpression id)
        {
            if (this.lookAhead.TokenType == TokenType.Equal)
            {
                var stmt = AssignationStatement(id);
                return stmt;
            }
            if(this.lookAhead.TokenType == TokenType.PlusPlus)
            {
                IncrementStatement(id);
                return null;
            }
            if(this.lookAhead.TokenType == TokenType.MinusMinus)
            {
                DecrementStatement(id);
                return null;
            }
            return null;
        }

        private Statement DecrementStatement(IdExpression id)
        {
            Match(TokenType.MinusMinus);
            return null;
        }

        private Statement IncrementStatement(IdExpression id)
        {
            Match(TokenType.PlusPlus);
            return null;
        }

        private Statement AssignationStatement(IdExpression id)
        {
            return AssignationStatementPrime(id);
        }

        private Statement AssignationStatementPrime(IdExpression id)
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Equal:
                    return AssignEquals(id);
                case TokenType.BitwiseOrEqual:
                    Match(TokenType.BitwiseOrEqual);
                    var expr = LogicalOrExpression();
                    return new AssignationStatement(id, expr);
                case TokenType.BitwiseAndEqual:
                    Match(TokenType.BitwiseAndEqual);
                    expr = LogicalOrExpression();
                    return new AssignationStatement(id, expr);
                case TokenType.BitwiseXorEqual:
                    Match(TokenType.BitwiseXorEqual);
                    expr = LogicalOrExpression();
                    return new AssignationStatement(id, expr);
                case TokenType.PlusEqual:
                    Match(TokenType.PlusEqual);
                    expr = LogicalOrExpression();
                    return new AssignationStatement(id, expr);
                case TokenType.MinusEqual:
                    Match(TokenType.MinusEqual);
                    expr = LogicalOrExpression();
                    return new AssignationStatement(id, expr);
                case TokenType.DivisionEqual:
                    Match(TokenType.DivisionEqual);
                    expr = LogicalOrExpression();
                    return new AssignationStatement(id, expr);
                default:
                    Match(TokenType.MultEqual);
                    expr = LogicalOrExpression();
                    return new AssignationStatement(id, expr);
            }
        }

        private Statement AssignEquals(IdExpression id)
        {
            Match(TokenType.Equal);
            return AssignEqualsPrime(id);
        }

        private Statement AssignEqualsPrime(IdExpression id)
        {
            if(this.lookAhead.TokenType == TokenType.LeftBracket)
            {
                Match(TokenType.LeftBracket);
                Expression();
                Match(TokenType.RightBracket);
            }
            var expr = LogicalOrExpression();
            Match(TokenType.Semicolon);
            return new AssignationStatement(id, expr);
        }

        private Statement FunctionStatement()
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
            return null;
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
            var expr = Type();
            return expr;
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
        //pregunta
        private Statement ForStatement()
        {
            Match(TokenType.ForKeyword);
            Match(TokenType.LeftParenthesis);
            var expression = LogicalOrExpression();
            Match(TokenType.RightParenthesis);
            return new ForStatement(expression, Block());
        }

        private Statement IfStatement()
        {
            Match(TokenType.IfKeyword);
            Match(TokenType.LeftParenthesis);
            var expr = LogicalOrExpression();
            Match(TokenType.RightParenthesis);
            Match(TokenType.LeftCurly);
            var stmt = Block();
            Match(TokenType.RightCurly);
            if(this.lookAhead.TokenType != TokenType.ElseKeyword)
            {
                return new IfStatement(expr, stmt, null);
            }
            var elseStatement = IfStatementPrime(expr, stmt);
            return elseStatement;
        }

        private Statement IfStatementPrime(Expresion expr, Statement ifstmt)
        {
            if(this.lookAhead.TokenType == TokenType.ElseKeyword)
            {
                Match(TokenType.ElseKeyword);
                Match(TokenType.LeftCurly);
                var elsestmt = Block();
                Match(TokenType.RightCurly);
                return new IfStatement(expr, ifstmt, elsestmt);
            }
            return null;
            //eps
        }

        private Expresion LogicalOrExpression()
        {
            var expr = LogicalAndExpression();
            return LogicalOrExpressionPrime(expr);
        }

        private Expresion LogicalOrExpressionPrime(Expresion expr)
        {
            if(this.lookAhead.TokenType == TokenType.Or)
            {
                Match(TokenType.Or);
                expr = LogicalAndExpression();
                return LogicalOrExpressionPrime(expr);
            }
            return new LogicalOrExpression(expr, LogicalAndExpression());
        }

        private Expresion LogicalAndExpression()
        {
            var expr = EqualityExpression();
            return LogicalAndExpressionPrime(expr);
        }

        private Expresion LogicalAndExpressionPrime(Expresion expr)
        {
            if(this.lookAhead.TokenType == TokenType.And)
            {
                Match(TokenType.And);
                expr = EqualityExpression();
                return LogicalAndExpressionPrime(expr);
            }
            return new LogicalAndExpression(expr, EqualityExpression());
        }

        private Expresion EqualityExpression()
        {
            var expr = RelationalExpression();
            return EqualityExpressionPrime(expr);
        }

        private Expresion EqualityExpressionPrime(Expresion expr)
        {
            if(this.lookAhead.TokenType == TokenType.Equality)
            {
                Match(TokenType.Equality);
                expr = RelationalExpression();
                return EqualityExpressionPrime(expr);
            }
            if(this.lookAhead.TokenType == TokenType.NotEquals)
            {
                Match(TokenType.NotEquals);
                expr = RelationalExpression();
                return EqualityExpressionPrime(expr);
            }
            return new EqualityExpression(expr, RelationalExpression());
            //eps
        }

        private Expresion RelationalExpression()
        {
            var expr = Expression();
            return RelationalExpressionPrime(expr);
        }

        private Expresion RelationalExpressionPrime(Expresion expr)
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.GreaterThan:
                    var token = this.lookAhead;
                    Match(TokenType.GreaterThan);
                    expr = new RelationalExpression(expr, Expression(), token);
                    return expr;
                case TokenType.LessThan:
                    token = this.lookAhead;
                    Match(TokenType.LessThan);
                    expr = new RelationalExpression(expr, Expression(), token);
                    return expr;
                case TokenType.GreaterOrEqualThan:
                    token = this.lookAhead;
                    Match(TokenType.GreaterOrEqualThan);
                    expr = new RelationalExpression(expr, Expression(), token);
                    return expr;
                case TokenType.LessOrEqualThan:
                    token = this.lookAhead;
                    Match(TokenType.LessOrEqualThan);
                    expr = new RelationalExpression(expr, Expression(), token);
                    return expr;
                default:
                    return expr;
            }
        }

        private Expresion Expression()
        {
            var expr = Term();
            var token = this.lookAhead;
            return ExpressionPrime(expr, token);
        }

        private Expresion ExpressionPrime(Expresion expr, Token token)
        {
            if(this.lookAhead.TokenType == TokenType.Plus)
            {
                var plus = this.lookAhead;
                Match(TokenType.Plus);
                expr = Term();
                return ExpressionPrime(expr, plus);
            }
            if(this.lookAhead.TokenType == TokenType.Minus)
            {
                var minus = this.lookAhead;
                Match(TokenType.Minus);
                expr = Term();
                return ExpressionPrime(expr, minus);
            }
            return new ArithmeticExpression(expr, Term(), token);
        }

        private Expresion Term()
        {
            var expr = Factor();
            var token = this.lookAhead;
            return TermPrime(expr, token);
        }

        private Expresion TermPrime(Expresion expr, Token token)
        {
            if (this.lookAhead.TokenType == TokenType.Mult)
            {
                var mult = this.lookAhead;
                Match(TokenType.Mult);
                expr = Factor();
                return TermPrime(expr, mult);
            }
            if (this.lookAhead.TokenType == TokenType.Division)
            {
                var div = this.lookAhead;
                Match(TokenType.Division);
                expr = Factor();
                return TermPrime(expr, div);
            }
            return new ArithmeticExpression(expr, Factor(), token);
        }

        private Expresion Factor()
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
                    constant = new ConstantExpresion(ExpresionType.Boolean, token);
                    break;
                case TokenType.FalseKeyword:
                    token = this.lookAhead;
                    Match(TokenType.FalseKeyword);
                    constant = new ConstantExpresion(ExpresionType.Boolean, token);
                    break;
                default:
                    Match(TokenType.Id);
                    break;
            }
        }

        private Statement WhileStatement()
        {
            Match(TokenType.WhileKeyword);
            Match(TokenType.LeftParenthesis);
            var expr = LogicalOrExpression();
            Match(TokenType.RightParenthesis);
            var stmt = Block();
            return new WhileStatement(expr, stmt);
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

        private Statement Imports()
        {
            if(this.lookAhead.TokenType == TokenType.ImportKeyword)
            {
                Import();
                Imports();
            }
            return null;
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