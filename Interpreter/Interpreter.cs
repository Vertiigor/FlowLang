using Interpreter.Grammar;
using Interpreter.Grammar.Expressions;

namespace Interpreter
{
    internal class Interpreter
    {
        public static object? Evaluate(Expression expr) => expr switch
        {
            Literal(var value) => value,

            Grouping(var inner) => Evaluate(inner),

            Unary(var op, var right) => EvaluateUnary(op, Evaluate(right)),

            Binary(var left, var op, var right) => op.Type switch
            {
                TokenType.AND => IsTruthy(Evaluate(left)) ? Evaluate(right) : false,
                TokenType.OR => IsTruthy(Evaluate(left)) ? true : Evaluate(right),
                _ => EvaluateBinary(Evaluate(left), op, Evaluate(right))
            },

            _ => throw new Exception("Unknown expression")
        };

        private static object? EvaluateUnary(Token op, object? right)
        {
            return op.Type switch
            {
                TokenType.MINUS => -(double)(right ?? 0),
                TokenType.BANG or TokenType.NOT => !IsTruthy(right),
                _ => throw new Exception($"Unknown unary operator: {op.Type}")
            };
        }

        private static object? EvaluateBinary(object? left, Token op, object? right)
        {
            return op.Type switch
            {
                TokenType.PLUS => Add(left, right),
                TokenType.MINUS => (double)left - (double)right,
                TokenType.STAR => (double)left * (double)right,
                TokenType.SLASH => (double)left / (double)right,
                TokenType.REMAINDER => (double)left % (double)right,
                TokenType.EQUAL_EQUAL => Equals(left, right),
                TokenType.BANG_EQUAL => !Equals(left, right),
                TokenType.GREATER => (double)left > (double)right,
                TokenType.LESS => (double)left < (double)right,
                TokenType.GREATER_EQUAL => (double)left >= (double)right,
                TokenType.LESS_EQUAL => (double)left <= (double)right,
                _ => throw new Exception($"Unknown binary operator: {op.Type}")
            };
        }

        private static bool IsTruthy(object? value)
        {
            return value switch
            {
                null => false,
                bool b => b,
                _ => true
            };
        }

        private static object Add(object? a, object? b)
        {
            if (a is int ai && b is int bi) return ai + bi;
            if (a is double ad && b is double bd) return ad + bd;
            if (a is string sa && b is string sb) return sa + sb;
            if (a is string s && b != null) return s + b.ToString();
            if (a != null && b is string s2) return a.ToString() + s2;
            throw new Exception("Operands must be two numbers or strings");
        }
    }
}
