namespace Cirno
{
    internal class Lexer
    {
        int currentLine;
        int currentIndex;
        char currentChar;

        string source;

        List<Token> tokens;
        public Lexer(String source)
        {
            this.currentLine = 1;
            this.currentChar = '\0';
            this.currentIndex = -1;
            this.source = source;
            this.tokens = new List<Token>();

            Advance();
        }

        public List<Token> Lex()
        {
            while(this.currentChar != '\0')
            {
                if (Char.IsDigit(currentChar))
                {
                    GetNumber();
                }
                else if (Char.IsLetter(currentChar))
                {
                    GetIdentifier();
                }
                else
                {
                    switch (this.currentChar)
                    {
                        case '\n':
                        case ' ':
                        case '\t':
                        case '\r':
                            break;
                        case '+':
                            tokens.Add(new Token(TokenType.PLUS, this.currentChar, currentLine));
                            break;
                        case '-':
                            tokens.Add(new Token(TokenType.MINUS, this.currentChar, currentLine));
                            break;
                        case '*':
                            tokens.Add(new Token(TokenType.STAR, this.currentChar, currentLine));
                            break;
                        case '/':
                            tokens.Add(new Token(TokenType.SLASH, this.currentChar, currentLine));
                            break;
                        case '%':
                            tokens.Add(new Token(TokenType.MOD, this.currentChar, currentLine));
                            break;
                        case '=':
                            if (Peak() == '=')
                            {
                                tokens.Add(new Token(TokenType.EQUALS_EQUALS, "==", currentLine));
                                Advance();
                            }
                            else tokens.Add(new Token(TokenType.EQUALS, this.currentChar, currentLine));
                            break;
                        case '!':
                            if (Peak() == '=')
                            {
                                tokens.Add(new Token(TokenType.BANG_EQUALS, "!=", currentLine));
                                Advance();
                            }
                            else tokens.Add(new Token(TokenType.BANG, this.currentChar, currentLine));
                            break;
                        case '<':
                            if (Peak() == '=')
                            {
                                tokens.Add(new Token(TokenType.LESS_EQUALS, "<=", currentLine));
                                Advance();
                            }
                            else tokens.Add(new Token(TokenType.LESS_THAN, this.currentChar, currentLine));
                            break;
                        case '>':
                            if (Peak() == '=')
                            {
                                tokens.Add(new Token(TokenType.GREATER_EQUALS, ">=", currentLine));
                                Advance();
                            }
                            else tokens.Add(new Token(TokenType.GREATER_THAN, this.currentChar, currentLine));
                            break;
                        case '(':
                            tokens.Add(new Token(TokenType.OPEN_PAREN, this.currentChar, currentLine));
                            break;
                        case ')':
                            tokens.Add(new Token(TokenType.CLOSED_PAREN, this.currentChar, currentLine));
                            break;
                        case '[':
                            tokens.Add(new Token(TokenType.OPEN_SQUARE, this.currentChar, currentLine));
                            break;
                        case ']':
                            tokens.Add(new Token(TokenType.CLOSED_SQUARE, this.currentChar, currentLine));
                            break;
                        case ',':
                            tokens.Add(new Token(TokenType.COMMA, this.currentChar, currentLine));
                            break;
                        case '"':
                            GetString();
                            break;
                        case '#':
                            GetComment();
                            break;
                        default:
                            ErrorManager.AddError(new Error($"Symbol \"{currentChar}\" is not recognized.", currentLine, ErrorType.SymbolNotRecognized, ErrorSafety.Fatal));
                            break;
                    }
                }
                Advance();
            }

            return tokens;
        }

        void GetNumber()
        {
            string num = "" + this.currentChar;
            bool isFloat = false;
            while(Char.IsDigit(Peak()) || Peak() == '.')
            {
                if(Peak() == '.')
                {
                    if (isFloat)
                    {
                        ErrorManager.AddError(new Error($"Number cannot have multiple periods.", currentLine, ErrorType.NumberCannotHaveMultiplePeriods, ErrorSafety.Fatal));
                        break;
                    }
                    isFloat = true;
                }
                num += Advance();
            }
            tokens.Add(new Token(TokenType.NUMBER, (float)Convert.ToDouble(num), currentLine));
        }
        void GetString()
        {
            string str = "";
            while (Peak() != '\"')
            {
                if(Peak() == '\\')
                    str += Advance();

                str += Advance();
            }
            Advance();
            tokens.Add(new Token(TokenType.STRING, str, currentLine));
        }
        void GetComment()
        {
            string comment = "";
            while (Peak() != '\n' && Peak() != '\0')
            {
                comment += Advance();
            }
            Advance();
        }
        void GetIdentifier()
        {
            string iden = "" + this.currentChar;
            while (Char.IsLetterOrDigit(Peak()) ||
                   Peak() == '_' ||
                   Peak() == '$')
            {
                iden += Advance();
            }
            switch (iden)
            {
                /*case "print":
                    tokens.Add(new Token(TokenType.PRINT, iden, currentLine));
                    return;*/
                case "do":
                    tokens.Add(new Token(TokenType.DO, iden, currentLine));
                    return;
                case "end":
                    tokens.Add(new Token(TokenType.END, iden, currentLine));
                    return;
                case "and":
                    tokens.Add(new Token(TokenType.AND, iden, currentLine));
                    return;
                case "or":
                    tokens.Add(new Token(TokenType.OR, iden, currentLine));
                    return;
                case "while":
                    tokens.Add(new Token(TokenType.WHILE, iden, currentLine));
                    return;
                case "if":
                    tokens.Add(new Token(TokenType.IF, iden, currentLine));
                    return;
                case "elif":
                    tokens.Add(new Token(TokenType.ELIF, iden, currentLine));
                    return;
                case "else":
                    tokens.Add(new Token(TokenType.ELSE, iden, currentLine));
                    return;
                case "fn":
                    tokens.Add(new Token(TokenType.FN, iden, currentLine));
                    return;
                case "true":
                    tokens.Add(new Token(TokenType.BOOL, true, currentLine));
                    return;
                case "false":
                    tokens.Add(new Token(TokenType.BOOL, false, currentLine));
                    return;
                case "nova":
                    tokens.Add(new Token(TokenType.NOVA, iden, currentLine));
                    return;

                case "add":
                    tokens.Add(new Token(TokenType.ADD, iden, currentLine));
                    return;
                case "pop":
                    tokens.Add(new Token(TokenType.POP, iden, currentLine));
                    return;

                case "break":
                    tokens.Add(new Token(TokenType.BREAK, iden, currentLine));
                    return;
                case "return":
                    tokens.Add(new Token(TokenType.RETURN, iden, currentLine));
                    return;
            }
            tokens.Add(new Token(TokenType.IDENTIFIER, iden, currentLine));
        }

        char Peak()
        {
            if (currentIndex + 1 >= source.Length)
            {
                return '\0';
            }
            else
            {
                return source[currentIndex + 1];
            }
        }

        char Advance()
        {
            currentIndex++;
            if (currentIndex >= source.Length)
            {
                currentChar = '\0';
            }
            else
            {
                currentChar = source[currentIndex];
                if(currentChar == '\n')
                {
                    currentLine++;
                }
            }
            return currentChar;
        }
    }
}
