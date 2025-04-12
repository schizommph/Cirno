using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum TokenType
{
    USING,

    NUMBER, STRING, IDENTIFIER, BOOL, NOVA,

    PLUS, MINUS, STAR, SLASH, MOD,

    BANG, COMMA, DOT,

    EQUALS,

    EQUALS_EQUALS, BANG_EQUALS, LESS_THAN, GREATER_THAN, LESS_EQUALS, GREATER_EQUALS,
    AND, OR,

    WHILE, IF, ELIF, ELSE, FN, FOR, IN, CLASS, NEW,
    BREAK, RETURN, CONTINUE,

    ADD, POP,

    OPEN_PAREN, CLOSED_PAREN,
    OPEN_SQUARE, CLOSED_SQUARE,
    OPEN_CURLY, CLOSED_CURLY,
    COLON,
    DO, END,

    PRINT
}

namespace Cirno
{
    internal class Token
    {
        public int line { get; private set; }
        public object lexeme { get; private set; }
        public TokenType type { get; private set; }

        public Token(TokenType type, object lexeme, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.line = line;
        }
        public override string ToString()
        {
            return $"<Token {type} {lexeme}>";
        }
    }
}
