using Interpreter.Exceptions;
using Interpreter.Grammar;
using Interpreter.Grammar.Expressions;

namespace Interpreter
{
    internal class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        private Token Peek => _tokens[_current];
        private bool IsAtEnd => Peek.Type == TokenType.EOF;
        private Token Previous => _tokens[_current - 1];

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public Expression Parse()
        {
            try
            {
                return Expression();
            }
            catch (ParseException)
            {
                // If a parse error occurs, synchronize and continue parsing
                Synchronize();
                return null; // Return null or an appropriate default value
            }
        }

        private Expression Expression()
        {
            return Equaluty();
        }

        private Expression Equaluty()
        {
            Expression expr = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token operatorToken = Previous;
                Expression right = Comparison();
                expr = new Binary(expr, operatorToken, right);
            }

            return expr;
        }

        private Expression Comparison()
        {
            Expression expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token operatorToken = Previous;
                Expression right = Term();
                expr = new Binary(expr, operatorToken, right);
            }
            return expr;
        }

        private Expression Term()
        {
            Expression expr = Factor();

            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token operatorToken = Previous;
                Expression right = Factor();
                expr = new Binary(expr, operatorToken, right);
            }

            return expr;
        }

        private Expression Factor()
        {
            Expression expr = Unary();

            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                Token operatorToken = Previous;
                Expression right = Unary();
                expr = new Binary(expr, operatorToken, right);
            }

            return expr;
        }

        private Expression Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token operatorToken = Previous;
                Expression right = Unary();
                return new Unary(operatorToken, right);
            }

            return Primary();
        }

        private Expression Primary()
        {
            if (Match(TokenType.FALSE)) return new Literal(false);
            if (Match(TokenType.TRUE)) return new Literal(true);
            if (Match(TokenType.NULL)) return new Literal(null);
            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Literal(Previous.Literal);
            }
            if (Match(TokenType.LEFT_PAREN))
            {
                Expression expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression."); // Consume the right parenthesis
                return new Grouping(expr);
            }

            throw new Exception($"Unexpected token: {Peek.Type}");
        }

        private void Synchronize()
        {
            Advance(); // Skip the current (bad) token

            while (!IsAtEnd)
            {
                // Lookahead token
                var type = Peek.Type;

                // These are considered "sync points" — likely starts of new statements
                if (type == TokenType.VAR ||
                    type == TokenType.FUNCTION ||
                    type == TokenType.PROCEDURE ||
                    type == TokenType.CONSTRUCTOR ||
                    type == TokenType.CLASS ||
                    type == TokenType.INTERFACE ||
                    type == TokenType.MODULE ||
                    type == TokenType.IF ||
                    type == TokenType.FOR ||
                    type == TokenType.WHILE ||
                    type == TokenType.SELECT ||
                    type == TokenType.RETURN ||
                    type == TokenType.END ||
                    type == TokenType.ELSE ||
                    type == TokenType.DEFAULT)
                {
                    return;
                }

                Advance(); // Skip token and keep scanning
            }
        }


        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
            {
                return Advance();
            }

            throw Error(Peek, message);
        }

        private ParseException Error(Token peek, string message)
        {
            Program.Error(peek, message);

            return new ParseException();
        }

        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();

                    return true;
                }
            }

            return false;
        }

        private Token Advance()
        {
            if (!IsAtEnd) _current++;

            return Previous;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd) return false;

            return Peek.Type == type;
        }
    }
}
