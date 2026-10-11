using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoolInterpreter {
    internal class Scanner {
        private readonly String source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;  // First character of current lexeme
        private int current = 0;  // Current character of the lexeme we are considering
        private int line = 1;

        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType> {
            ["and"] = TokenType.AND,
            ["class"] = TokenType.CLASS,
            ["else"] = TokenType.ELSE,
            ["false"] = TokenType.FALSE,
            ["for"] = TokenType.FOR,
            ["fun"] = TokenType.FUN,
            ["if"] = TokenType.IF,
            ["nil"] = TokenType.NIL,
            ["or"] = TokenType.OR,
            ["print"] = TokenType.PRINT,
            ["return"] = TokenType.RETURN,
            ["super"] = TokenType.SUPER,
            ["this"] = TokenType.THIS,
            ["true"] = TokenType.TRUE,
            ["var"] = TokenType.VAR,
            ["while"] = TokenType.WHILE,
        };

        public Scanner(String source) {
            this.source = source;
        }

        // Helper methods
        private bool isAtEnd() {
            return current >= source.Length;
        }
        private char advance() {
            current++;
            return source[current - 1];
        }

        // Only advances to the next character if the current character equals expected
        private bool match(char expected) {
            if (isAtEnd()) {
                return false;
            }
            if (expected == source[current]) {
                current++;
                return true;
            }
            return false;
        }
        private void addToken(TokenType type) {
            addToken(type, null);
        }
        private void addToken(TokenType type, Object? literal) {
            String text = source.Substring(start, current - start);  // Do we add 1?
            tokens.Add(new Token(type, text, literal, line));
        }

        private char peek() {
            if (isAtEnd()) return '\0';
            return source[current];
        }
        // Peeks 1 character ahead of the character at current
        private char peekNext() {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }
        private bool isDigit(char c) {
            if (c >= '0' && c <= '9') return true;
            return false;
        }
        private bool isAlpha(char c) {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }
        private bool isAlphaNumeric(char c) {
            return isAlpha(c) || isDigit(c);
        }

        private void scanToken() {
            char c = advance();
            switch (c) {
                // Single character tokens
                case '(':
                    addToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    addToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    addToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    addToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    addToken(TokenType.COMMA);
                    break;
                case '.':
                    addToken(TokenType.DOT);
                    break;
                case '-':
                    addToken(TokenType.MINUS);
                    break;
                case '+':
                    addToken(TokenType.PLUS);
                    break;
                case ';':
                    addToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    addToken(TokenType.STAR);
                    break;

                // Two character lexemes
                case '!':
                    addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;

                // Longer lexemes
                case '/':
                    if (match('/')) {  // If two slashes, then its a comment
                        // Comments go until the end of the line
                        while (peek() != '\n' && !isAtEnd()) {
                            advance();
                        }
                    } else {
                        addToken(TokenType.SLASH);
                    }
                    break;
                case '"':
                    parseString();
                    break;
                // Whitespaces
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;
                case '\n':
                    line++;
                    break;

                default:
                    if (isDigit(c)) {  // Number literal
                        number();
                    } else if (isAlpha(c)) {  // Assume any character starting with letter or underscore is an identifier
                        identifier();
                    } else {
                        Program.error(line, "Unexpected character");
                    }
                    break;
            }
        }

        private void parseString() {
            while (peek() != '"' && !isAtEnd()) {
                if (peek() == '\n') line++;
                advance();
            }

            if (isAtEnd()) {
                Program.error(line, "Unterminated string.");
                return;
            }

            advance();  // Closing "
            String value = source.Substring(start + 1, current - start - 1);
            addToken(TokenType.STRING, value);
        }
        private void number() {
            while (isDigit(peek())) advance();

            if (peek() == '.' && isDigit(peekNext())) {
                // Consume '.'
                advance();
                while (isDigit(peek())) advance();
            }

            addToken(TokenType.NUMBER, double.Parse(source.Substring(start, current - start)));
        }
        private void identifier() {
            while (isAlphaNumeric(peek())) advance();

            string text = source.Substring(start, current - start);
            TokenType type = TokenType.IDENTIFIER;
            if (keywords.TryGetValue(text, out TokenType strType)) {
                type = strType;
            }
            addToken(type);
        }
        public List<Token> scanTokens() {
            while (current < source.Length) {
                start = current;
                scanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }
    }
}
