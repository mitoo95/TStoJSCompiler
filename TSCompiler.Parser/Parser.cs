using LanguageCompiler.Core;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
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
            var import = Imports();
            var block = Block();
            var main = Main();

            return new ReturnCode(import,block,main);
        }

        private Statement Block()
        {
            
            if (this.lookAhead.TokenType == TokenType.VarKeyword || this.lookAhead.TokenType == TokenType.ConstKeyword || this.lookAhead.TokenType == TokenType.LetKeyword)
            {
                //ignorar por los momentos
                Declarations();
                Block();
            }
            if (this.lookAhead.TokenType == TokenType.Id || this.lookAhead.TokenType == TokenType.IfKeyword
            || this.lookAhead.TokenType == TokenType.WhileKeyword || this.lookAhead.TokenType == TokenType.ForKeyword
            || this.lookAhead.TokenType == TokenType.ReturnKeyword || this.lookAhead.TokenType == TokenType.BreakKeyword
            || this.lookAhead.TokenType == TokenType.ContinueKeyword || this.lookAhead.TokenType == TokenType.PlusPlus
            || this.lookAhead.TokenType == TokenType.MinusMinus || this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                var statements = Statements();
                var block = Block();

                return new SequenceStatement(statements, block);
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
                var statement = Block();
                Match(TokenType.RightCurly);
                return statement;
            }
                return null;
        }

        private Statement Statements()
        {
            if (this.lookAhead.TokenType == TokenType.Id || this.lookAhead.TokenType == TokenType.IfKeyword
                || this.lookAhead.TokenType == TokenType.WhileKeyword || this.lookAhead.TokenType == TokenType.ForKeyword
                || this.lookAhead.TokenType == TokenType.ReturnKeyword || this.lookAhead.TokenType == TokenType.BreakKeyword
                || this.lookAhead.TokenType == TokenType.ContinueKeyword || this.lookAhead.TokenType == TokenType.PlusPlus
                || this.lookAhead.TokenType == TokenType.MinusMinus || this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                var current = Statement();
                var next = Statements();
                return new SequenceStatement(current, next);
            }
            return null;
        }

        private Statement Statement()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Id:
                    var statement = Id_Statement();
                    return statement;
                case TokenType.WhileKeyword:
                    return WhileStatement();
                case TokenType.IfKeyword:
                    return IfStatement();
                case TokenType.ForKeyword:
                    ForStatement();
                    break;
                case TokenType.ReturnKeyword:
                    ReturnStatement();
                    break;
                case TokenType.BreakKeyword:
                    BreakStatement();
                    break;
                case TokenType.ContinueKeyword:
                    ContinueStatement();
                    break;
                case TokenType.PlusPlus:
                    IncrementStatement();
                    break;
                case TokenType.MinusMinus:
                    DecrementStatement();
                    break;
                case TokenType.FunctionKeyword:
                    FunctionStatement();
                    break;
            }
        }

        private Statement Id_Statement()
        {
            IdExpression id = new IdExpression(this.lookAhead.Lexeme, null);
            Match(TokenType.Id);
            var statement = Id_Statement_Prime(id);
            return statement;
        }

        private Statement Id_Statement_Prime(IdExpression id)
        {
            if (this.lookAhead.TokenType == TokenType.Equal)
            {
                var statement = AssignationStatement(id);
                return statement;
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
        }

        private Statement DecrementStatement(IdExpression id)
        {
            Match(TokenType.MinusMinus);
            DecrementStatementPrime(id);
            return null;
        }

        private Statement DecrementStatementPrime(IdExpression id)
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
            return null;
        }

        //WIP
        private Statement IncrementStatement(IdExpression id)
        {
            Match(TokenType.PlusPlus);
            IncrementStatementPrime(id);
            return null;
            //que retornamos aqui?
        }

        private Statement IncrementStatementPrime(IdExpression id)
        {
            //++id;
            //preguntar
            //WIP
            if(this.lookAhead.TokenType == TokenType.Id)
            {
                Match(TokenType.Id);
                Match(TokenType.Semicolon);
                //return IncrementStatement(id);
            }
            //id++;
            else
            {
                Match(TokenType.Semicolon);
                //return IncrementStatement(id);
            }
        }

        private Statement AssignationStatement(IdExpression id)
        {
            var statement = AssignationStatementPrime(id);
            return statement;
        }

        private Statement AssignationStatementPrime(IdExpression id)
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Equal:
                    return AssignEquals(id);
                case TokenType.BitwiseOrEqual:
                    Match(TokenType.BitwiseOrEqual);
                    var expression = LogicalOrExpression();
                    return new AssignationStatement(id, expression);
                case TokenType.BitwiseAndEqual:
                    Match(TokenType.BitwiseAndEqual);
                    expression = LogicalOrExpression();
                    return new AssignationStatement(id, expression);
                case TokenType.BitwiseXorEqual:
                    Match(TokenType.BitwiseXorEqual);
                    expression = LogicalOrExpression();
                    return new AssignationStatement(id, expression);
                case TokenType.PlusEqual:
                    Match(TokenType.PlusEqual);
                    expression = LogicalOrExpression();
                    return new AssignationStatement(id, expression);
                case TokenType.MinusEqual:
                    Match(TokenType.MinusEqual);
                    expression = LogicalOrExpression();
                    return new AssignationStatement(id, expression);
                case TokenType.DivisionEqual:
                    Match(TokenType.DivisionEqual);
                    expression = LogicalOrExpression();
                    return new AssignationStatement(id, expression);
                default:
                    Match(TokenType.MultEqual);
                    expression = LogicalOrExpression();
                    return new AssignationStatement(id, expression);
            }
            return null;
        }

        private Statement AssignEquals(IdExpression id)
        {
            Match(TokenType.Equal);
            var statement = AssignEqualsPrime(id);
            return statement;
        }

        private Statement AssignEqualsPrime(IdExpression id)
        {
            if(this.lookAhead.TokenType == TokenType.LeftBracket)
            {
                Match(TokenType.LeftBracket);
                Expression();
                Match(TokenType.RightBracket);
            }
            var expression = LogicalOrExpression();
            Match(TokenType.Semicolon);
            return new AssignationStatement(id, expression);
        }

        private void FunctionStatement()
        {
            if(this.lookAhead.TokenType == TokenType.FunctionKeyword)
            {
                Match(TokenType.FunctionKeyword);
                Match(TokenType.Id);
                Match(TokenType.LeftParenthesis);
                Params();
                Match(TokenType.RightParenthesis);
                Match(TokenType.Colon);
                Type();
                Match(TokenType.LeftCurly);
                Block();
                Match(TokenType.RightCurly);
            }
            //WIP
        }

        private void Params()
        {
            Param();
            ParamsPrime();
        }

        private void ParamsPrime()
        {
            if(this.lookAhead.TokenType == TokenType.Comma)
            {
                Match(TokenType.Comma);
                Params();
            }
            //eps
        }

        private void Param()
        {
            Match(TokenType.Id);
            Match(TokenType.Colon);
            Type();
        }

        private void ContinueStatement()
        {
            Match(TokenType.ContinueKeyword);
            Match(TokenType.Semicolon);
        }

        private void BreakStatement()
        {
            Match(TokenType.BreakKeyword);
            Match(TokenType.Semicolon);
        }

        private void ReturnStatement()
        {
            Match(TokenType.ReturnKeyword);
            LogicalOrExpression();
            Match(TokenType.Semicolon);
            //WIP
        }

        //pregunta
        private void ForStatement()
        {
            Match(TokenType.ForKeyword);
            Match(TokenType.LeftParenthesis);
            LogicalOrExpression();
            Match(TokenType.RightParenthesis);
            Block();
        }

        private Statement IfStatement()
        {
            Match(TokenType.IfKeyword);
            Match(TokenType.LeftParenthesis);
            var expression = LogicalOrExpression();
            Match(TokenType.RightParenthesis);
            Match(TokenType.LeftCurly);
            //true statement, no branch a else
            var trueStatement = Block();
            Match(TokenType.RightCurly);
            //si se va a else, retorna un false statement, de lo contrario retorna nulo
            var falseStatement = IfStatementPrime();
            //esta buena esta logica?
            return new IfStatement(expression, trueStatement, falseStatement);
        }

        private Statement IfStatementPrime()
        {
            if(this.lookAhead.TokenType == TokenType.ElseKeyword)
            {
                Match(TokenType.ElseKeyword);
                Match(TokenType.LeftCurly);
                var falseStatement = Block();
                Match(TokenType.RightCurly);
                return falseStatement;
            }
            //eps
            return null;
        }

        private Expresion LogicalOrExpression()
        {
            var expression = LogicalAndExpression();
            LogicalOrExpressionPrime(expression);
        }

        private Expresion LogicalOrExpressionPrime(Expresion expression)
        {
            if(this.lookAhead.TokenType == TokenType.Or)
            {
                Match(TokenType.Or);
                expression = new LogicalOrExpression(expression,null);
                LogicalOrExpressionPrime();
            }
            //eps
        }

        private Expresion LogicalAndExpression()
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
        }

        private void Factor()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LeftParenthesis:
                    Match(TokenType.LeftParenthesis);
                    Expression();
                    Match(TokenType.RightParenthesis);
                    break;
                case TokenType.NumberConst:
                    Match(TokenType.NumberConst);
                    break;
                case TokenType.TrueKeyword:
                    Match(TokenType.TrueKeyword);
                    break;
                case TokenType.FalseKeyword:
                    Match(TokenType.FalseKeyword);
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
            var expression = LogicalOrExpression();
            Match(TokenType.RightParenthesis);
            var statement = Block();
            return new WhileStatement(expression, statement);
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

        private void Type()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.NumberKeyword:
                    Match(TokenType.NumberKeyword);
                    TypePrime();
                    break;
                case TokenType.BooleanKeyword:
                    Match(TokenType.BooleanKeyword);
                    TypePrime();
                    break;
                default:
                    Match(TokenType.StringKeyword);
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