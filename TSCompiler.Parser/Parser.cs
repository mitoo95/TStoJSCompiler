using System.Linq.Expressions;
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
            ContextManager.Push();
            var code = new ReturnCode(Imports(), Block(), Main());
            ContextManager.Pop();
            return code;
            
        }

        private Statement Block()
        {
            IdExpression id = null;
            if (this.lookAhead.TokenType == TokenType.VarKeyword || this.lookAhead.TokenType == TokenType.ConstKeyword || this.lookAhead.TokenType == TokenType.LetKeyword)
            {
                return new SequenceStatement(Declarations(id), Block());
                /*Declarations(ref id);
                Block();*/
            }
            if (this.lookAhead.TokenType == TokenType.Id || this.lookAhead.TokenType == TokenType.IfKeyword
            || this.lookAhead.TokenType == TokenType.WhileKeyword || this.lookAhead.TokenType == TokenType.ForKeyword
            || this.lookAhead.TokenType == TokenType.ReturnKeyword || this.lookAhead.TokenType == TokenType.BreakKeyword
            || this.lookAhead.TokenType == TokenType.ContinueKeyword || this.lookAhead.TokenType == TokenType.PlusPlus
            || this.lookAhead.TokenType == TokenType.MinusMinus || this.lookAhead.TokenType == TokenType.FunctionKeyword
            || this.lookAhead.TokenType == TokenType.Console)
            {
                return new SequenceStatement(Statements(id), Block());
            }
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
                || this.lookAhead.TokenType == TokenType.MinusMinus || this.lookAhead.TokenType == TokenType.FunctionKeyword
                || this.lookAhead.TokenType == TokenType.Console)
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
                    return FunctionStatement(id);
                case TokenType.Console:
                    return ConsoleLogStatement(id);
            }
            return null;
        }

        private Statement Id_Statement(IdExpression id)
        {
            var token = this.lookAhead;
            id = new IdExpression(token.Lexeme, null);
            Match(TokenType.Id);
            ContextManager.Put(id.Name, id);
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
                var stmt = IncrementStatement(id);
                return stmt;
            }
            if(this.lookAhead.TokenType == TokenType.MinusMinus)
            {
                var stmt = DecrementStatement(id);
                return stmt;
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

        private Statement FunctionStatement(IdExpression id)
        {
            if(this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                Match(TokenType.FunctionKeyword);
                var token = this.lookAhead;
                id = new IdExpression(token.Lexeme, null);
                Match(TokenType.Id);
                ContextManager.Put(id.Name, id);
                Match(TokenType.LeftParenthesis);
                var expr = Params(id);
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

        private List<ExpresionType> Params(IdExpression id)
        {
            var expressions = new List<ExpresionType>();
            expressions.Add(Param(id)); 
            expressions.AddRange(ParamsPrime(id));
            return expressions;
        }

        private List<ExpresionType> ParamsPrime(IdExpression id)
        {
            var expressions = new List<ExpresionType>();
            if(this.lookAhead.TokenType == TokenType.Comma)
            {
                Match(TokenType.Comma);
                expressions = Params(id);

            }
            return expressions;
        }

        private ExpresionType Param(IdExpression id)
        {
            var token = this.lookAhead;
            id = new IdExpression(token.Lexeme, null);
            Match(TokenType.Id);
            Match(TokenType.Colon);
            var expr = Type();
            return new BinaryParameter(id.Name, TokenType.BasicType, id, expr);
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
                    return new ConstantExpresion(ExpresionType.Number, token);
                case TokenType.TrueKeyword:
                    token = this.lookAhead;
                    Match(TokenType.TrueKeyword);
                    return new ConstantExpresion(ExpresionType.Boolean, token);
                case TokenType.FalseKeyword:
                    token = this.lookAhead;
                    Match(TokenType.FalseKeyword);
                    return new ConstantExpresion(ExpresionType.Boolean, token);
                case TokenType.NullKeyword:
                    token = this.lookAhead;
                    Match(TokenType.NullKeyword);
                    return new ConstantExpresion(ExpresionType.Null, token);
                case TokenType.Id:
                    token = this.lookAhead;
                    Match(TokenType.Id);
                    var id = ContextManager.Get(token.Lexeme).Id;
                    if (id.Type is not ArrayType)
                    {
                        return id;
                    }
                    Match(TokenType.LeftBracket);
                    var index = LogicalOrExpression();
                    Match(TokenType.RightBracket);
                    id.Type = ((ArrayType)id.GetType()).Of;
                    return id;
                case TokenType.Not:
                    token = this.lookAhead;
                    Match(TokenType.Not);
                    return new ConstantExpresion(ExpresionType.Not, token);
                default:
                    return null;
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

        private Statement Declarations(IdExpression id)
        {
            if(this.lookAhead.TokenType == TokenType.VarKeyword || this.lookAhead.TokenType == TokenType.ConstKeyword || this.lookAhead.TokenType == TokenType.LetKeyword)
            {
                return new SequenceStatement(Declaration(id), Declarations(id));
                /*Declaration(ref id);
                Declarations(ref id);*/
            }
            return null;
            //eps
        }

        private Statement Declaration(IdExpression id)
        {
            var exprType = VarDeclaration();
            var token = this.lookAhead;
            id = new IdExpression(token.Lexeme, null);
            Match(TokenType.Id);
            ContextManager.Put(id.Name, id);
            return new DeclarationAssignation(exprType, DeclarationPrime(id));
        }

        private Statement DeclarationPrime(IdExpression id)
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Colon:
                    Match(TokenType.Colon);
                    var type = Type();
                    var stmt = DeclarationPrimePrime(id);
                    return new DeclarationAssignation(type, stmt);
                default:
                    Match(TokenType.Equal);
                    if (this.lookAhead.TokenType == TokenType.LeftParenthesis)
                    {
                        Match(TokenType.LeftParenthesis);
                        var @params = Params(id);
                        Match(TokenType.RightParenthesis);
                        Match(TokenType.Colon);
                        type = Type();
                        Match(TokenType.ArrowFunction);
                        Match(TokenType.LeftCurly);
                        stmt = Block();
                        Match(TokenType.RightCurly);
                        return new FunctionStatement(@params, type, stmt);
                    }
                    var assign = AssignEqualsPrime(id);
                    return assign;
                    
            }
        }

        private Statement DeclarationPrimePrime(IdExpression id)
        {
            if(this.lookAhead.TokenType == TokenType.LeftBracket)
            {
                Match(TokenType.LeftBracket);
                var expr = Expression();
                Match(TokenType.RightBracket);
                Match(TokenType.Semicolon);
                return new AssignationStatement(id, expr);
            }
            if(this.lookAhead.TokenType == TokenType.Equal)
            {
                return AssignEquals(id);
            }
            else
            {
                Match(TokenType.Semicolon);
                return null;
            }
        }

        private ExpresionType Type()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.NumberKeyword:
                    Match(TokenType.NumberKeyword);
                    return ExpresionType.Number;
                case TokenType.BooleanKeyword:
                    Match(TokenType.BooleanKeyword);
                    return ExpresionType.Boolean;
                case TokenType.UndefinedKeyword:
                    Match(TokenType.UndefinedKeyword);
                    return ExpresionType.Undefined;
                case TokenType.NullKeyword:
                    Match(TokenType.NullKeyword);
                    return ExpresionType.Null;
                case TokenType.ArrayKeyword:
                    
                    Match(TokenType.ArrayKeyword);
                    Match(TokenType.LessThan);
                    var type = Type();
                    Match(TokenType.GreaterThan);
                    Match(TokenType.LeftBracket);
                    var size = this.lookAhead;
                    Match(TokenType.NumberConst);
                    Match(TokenType.RightBracket);
                    return new ArrayType("[]", TokenType.ComplexType, type, int.Parse(size.Lexeme));
                default:
                    Match(TokenType.StringKeyword);
                    return ExpresionType.String;
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

        private ExpresionType VarDeclaration()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.VarKeyword:
                    Match(TokenType.VarKeyword);
                    return ExpresionType.Var;
                case TokenType.LetKeyword:
                    Match(TokenType.LetKeyword);
                    return ExpresionType.Let;
                default:
                    Match(TokenType.ConstKeyword);
                    return ExpresionType.Const;
            }
        }

        private Statement ConsoleLogStatement(IdExpression id)
        {
            
            Match(TokenType.Console);
            Match(TokenType.Dot);
            Match(TokenType.Log);
            Match(TokenType.LeftParenthesis);
            var @params = Params(id);
            Match(TokenType.RightParenthesis);
            Match(TokenType.Semicolon);
            return new ConsoleLogStatement(@params);
            
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