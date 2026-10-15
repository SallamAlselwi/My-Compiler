using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Lexical_Layer

{
    public class MyLexical
    {
        string source;

        StreamReader streamReader;

        private readonly ErrorHandler errorHandler = new ErrorHandler();

        private List<Token> tokens = new List<Token>();

        private int current;

        int line = 0, column = 0;

        public MyLexical(string filepath, ErrorHandler errorHandler)
        {
            streamReader = new StreamReader(filepath);

            source = "";

            this.errorHandler = errorHandler;

            current = 0;
        }

        public List<Token> Analyze()
        {
            while (!streamReader.EndOfStream)
            {
                source = "" + streamReader.ReadLine();

                line++;

                current = 0;

                while (!IsAtEnd())
                {
                    ScanToken();
                }

            }
            tokens.Add(new Token(TokenType.EOF, "", line, current - 1));

            streamReader.Close();
            return tokens;
        }

        private bool IsAtEnd()
        {
            return current >= source.Length;
        }

        private char PeekNext()
        {
            if (IsAtEnd())
                return '\0';

            return source[current+1];
        }
        private char Peek()
        {
            if (IsAtEnd())
                return '\0';

            return source[current];
        }

        private char Advance()
        {
            return source[current++];
        }

        private void ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                case '(':
                    tokens.Add(new Token(TokenType.LPAREN, "(", line, current));
                    break;
                case ')':
                    tokens.Add(new Token(TokenType.RPAREN, ")", line, current));
                    break;
                case '{':
                    tokens.Add(new Token(TokenType.LBRACE, "{", line, current));
                    break;
                case '}':
                    tokens.Add(new Token(TokenType.RBRACE, "}", line, current));
                    break;
                case ';':
                    tokens.Add(new Token(TokenType.SEMICOLON, ";", line, current));
                    break;
                case ',':
                    tokens.Add(new Token(TokenType.COMMA, ",", line, current));
                    break;
                case '=':
                    if(Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.EQUAL, "==", line, current));
                        Advance();
                    }
                    else
                        tokens.Add(new Token(TokenType.ASSIGN, "=", line, current));
                    break;
                case '<':
                    tokens.Add(new Token(TokenType.LESS, ">", line, current));
                    break;
                case '>':
                    tokens.Add(new Token(TokenType.GREATER, "<", line, current));
                    break;
                case '/':
                    if (Peek() == '/')
                    {
                        current = source.Length;
                    }
                    else
                        tokens.Add(new Token(TokenType.DIVIDE, "/", line, current));
                    break;
                case '*':
                    tokens.Add(new Token(TokenType.MULTIPLY, "*", line, current));
                    break;
                case '-':
                    tokens.Add(new Token(TokenType.MINUS, "-", line, current));
                    break;
                case '+':
                    tokens.Add(new Token(TokenType.PLUS, "+", line, current));
                    break;
                case '\n':
                case ' ':
                case '\r':
                case '\t':
                    break;

                default:
                    if (IsLetter(c) || c == '_')
                    {
                        ReadIdentifier();
                    }
                    else if (IsDigit(c))
                    {
                        ReadNumber();
                    }
                    else
                    {
                        errorHandler.Add($"Unknown character '{c}'", line, current);
                    }

                    break;

            }
        }
        private void ReadIdentifier()
        {
            int start = current - 1;

            while (IsLetterOrDigit(Peek()) || Peek() == '_')
            {
                Advance();
            }

            string text = source.Substring(start, current - start);

            switch (text)
            {
                case "ifs":
                    tokens.Add(new Token(TokenType.IF, text, line, current));
                    break;

                case "else":
                    tokens.Add(new Token(TokenType.ELSE, text, line, current));
                    break;

                case "whiles":
                    tokens.Add(new Token(TokenType.WHILE, text, line, current));
                    break;

                case "dos":
                    tokens.Add(new Token(TokenType.DO, text, line, current));
                    break;

                case "fors":
                    tokens.Add(new Token(TokenType.FOR, text, line, current));
                    break;

                case "ints":
                    tokens.Add(new Token(TokenType.INT, text, line, current));
                    break;

                case "floats":
                    tokens.Add(new Token(TokenType.FLOAT, text, line, current));
                    break;

                default:
                    tokens.Add(new Token(TokenType.IDENTIFIER, text, line, current));
                    break;
            }
        }
        private void ReadNumber()
        {
            int start = current - 1;

            while (IsDigit(Peek()))
            {
                Advance();
            }

            string number = source.Substring(start, current - start);

            tokens.Add(new Token(TokenType.INTEGER, number, line, current));
        }

        private bool IsLetter(char letter)
        {
            if (letter >= 65 && letter <= 90 || letter >= 97 && letter <= 122)
                return true;
            else
                return false;
        }

        private bool IsDigit(char letter)
        {
            if (letter >= 48 && letter <= 57)
                return true;
            else
                return false;
        }
        private bool IsLetterOrDigit(char letter)
        {
            if (IsLetter(letter) || IsDigit(letter))
                return true;
            else
                return false;
        }
    }
}
