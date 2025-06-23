namespace Interpreter
{
    internal enum TokenType
    {
        // Single-character tokens
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR, REMAINDER,

        // One or two character tokens
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        // Literals
        IDENTIFIER, STRING, INTEGER, FLOAT, DOUBLE, BOOL, CHAR, DATE, DECIMAL, BYTE, NUMBER,

        // Keywords
        VAR, AS, INT, DOUBLE_KW, STRING_KW, FLOAT_KW, BOOL_KW,
        CONSTRUCTOR, CONST, MEMBER, IF, FOR, THEN, WHILE, ELSE, END,
        MODULE, CLASS, STRUCT, FUNCTION, SELECT, CHAR_KW, TO, UNTIL,
        DEFAULT, CASE, NULL, AND, OR, TRUE, FALSE, NOT,
        PRIVATE, PUBLIC, PROTECTED, INTERFACE, IMPORT, INHERITS,
        IMPLEMENTS, WITH, BASE, ARRAY, THIS, FOREACH, IN,
        RETURN, DO, STEP, PROCEDURE, BREAK, CONTINUE,

        GROUP,

        // End of file
        EOF
    }

    internal class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public object Literal { get; }
        public int Line { get; }

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public override string ToString()
        {
            return $"Type: {Type}\tLexeme: {Lexeme}\tLiteral: {Literal}";
        }
    }
}
