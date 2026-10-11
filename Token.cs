using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CoolInterpreter {
    enum TokenType {
        // Single-character tokens.
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,
        // One or two character tokens.
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,
        // Literals.
        IDENTIFIER, STRING, NUMBER,
        // Keywords.
        AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
        PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,
        EOF
    }
    internal class Token {
        readonly TokenType type;
        readonly String lexeme;
        readonly Object? literal;
        readonly int line;
        public Token(TokenType type, String lexeme, Object? literal, int line) {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }
        public override string ToString() {
            return type + " " + lexeme + " " + literal;
        }
    }
}
