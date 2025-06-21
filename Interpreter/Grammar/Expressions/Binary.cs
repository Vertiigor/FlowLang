using Interpreter.Grammar;

namespace Interpreter.Grammar.Expressions
{
    public record Binary(Expression Left, Token Operator, Expression Right) : Interpreter.Grammar.Expression;
}
