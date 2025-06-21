using Interpreter.Grammar.Expressions;

namespace Interpreter
{
    internal class AstPrinter
    {
        public static string Print(Interpreter.Grammar.Expression expr) => expr switch
        {
            Binary(var left, var op, var right) =>
                Parenthesize(op, left, right),

            Grouping(var inner) =>
                Parenthesize(new Token(TokenType.GROUP, "group", string.Empty, 0), inner),

            Literal(var value) =>
                value?.ToString() ?? "nil",

            Unary(var op, var right) =>
                Parenthesize(op, right),

            _ => throw new Exception("Unknown expression")
        };

        private static string Parenthesize(Token name, params Interpreter.Grammar.Expression[] exprs)
        {
            var parts = exprs.Select(Print);

            return $"({name.Lexeme} {string.Join(" ", parts)})";
        }
    }
}
