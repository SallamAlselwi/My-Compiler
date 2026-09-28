using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCompiler.Common;
using MyCompiler.Syntax_Layer.AST;

namespace MyCompiler.Syntax_Layer
{
    public class MySyntax
    {
        readonly List<Token> tokens;
        readonly SymbolTable symbolTable;
        readonly ErrorHandler errorHandler;

        private int current = 0;
        public MySyntax(List<Token> tokens, SymbolTable symbolTable, ErrorHandler errorHandler)
        {
            this.tokens = tokens;
            this.symbolTable = symbolTable;
            this.errorHandler = errorHandler;
        }
        public MyProgramNode Parse()
        {
            MyProgramNode program = new MyProgramNode();

            while (!IsAtEnd())
            {
                program.Statements.Add(ParseStatement());
            }

            return program;
        }
        private Token Peek()
        {
            return tokens[current];
        }
        private Token Previous()
        {
            return tokens[current - 1];
        }
        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }
        private Token Advance()
        {
            if (!IsAtEnd())
                current++;

            return Previous();
        }
        private bool Match(TokenType type)
        {
            if (Peek().Type == type)
            {
                Advance();
                return true;
            }

            return false;
        }
        private Token Consume(TokenType expectedType)
        {
            if (Peek().Type == expectedType)
            {
                return Advance();
            }

            errorHandler.Add($"Expected {expectedType} but found {Peek().Type}", Peek().Line, Peek().Column);

            return Peek();
        }
        private MyStatement ParseStatement()
        {
            switch (Peek().Type)
            {
                case TokenType.INT:
                    return ParseVariableDeclaration();

                case TokenType.IF:
                    return ParseIf();

                case TokenType.WHILE:
                    return ParseWhile();

                case TokenType.DO:
                    return ParseDoWhile();

                //case TokenType.FOR:
                //    return ParseFor();

                case TokenType.LBRACE:
                    return ParseBlock();

                case TokenType.IDENTIFIER:

                    if (PeekNext().Type == TokenType.ASSIGN)
                    {
                        return ParseAssignment();
                    }

                    return ParseExpressionStatement();

                default:

                    errorHandler.Add($"Unexpected token {Peek().Type}", Peek().Line, Peek().Column);

                    Advance();

                    return new MyBlockStatement();
            }
        }
        private MyBlockStatement ParseBlock()
        {
            Consume(TokenType.LBRACE);

            MyBlockStatement block = new MyBlockStatement();

            while (!IsAtEnd() && Peek().Type != TokenType.RBRACE)
            {
                block.Statements.Add(ParseStatement());
            }

            Consume(TokenType.RBRACE);

            return block;
        }
        private MyVariableDeclaration ParseVariableDeclaration()
        {
            Consume(TokenType.INT);

            Token name = ConsumeIdentifier();

            MyExpression? initializer = null;

            if (Match(TokenType.ASSIGN))
            {
                initializer = ParseExpression();
            }

            Consume(TokenType.SEMICOLON);

            if (!symbolTable.Add(new Symbol(name.Lexeme, TokenType.INT)))
            {
                errorHandler.Add($"Variable '{name.Lexeme}' is already declared.", name.Line, name.Column);
            }

            return new MyVariableDeclaration(TokenType.INT, name.Lexeme, initializer);
        }
        private Token ConsumeIdentifier()
        {
            return Consume(TokenType.IDENTIFIER);
        }
        private MyIfStatement ParseIf()
        {
            Consume(TokenType.IF);

            Consume(TokenType.LPAREN);

            MyExpression condition = ParseExpression();

            Consume(TokenType.RPAREN);

            MyStatement thenBranch = ParseStatement();

            MyStatement? elseBranch = null;

            if (Match(TokenType.ELSE))
            {
                elseBranch = ParseStatement();
            }

            return new MyIfStatement(TokenType.IF, condition, thenBranch, elseBranch);
        }
        private MyWhileStatement ParseWhile()
        {
            Consume(TokenType.WHILE);

            Consume(TokenType.LPAREN);

            MyExpression condition = ParseExpression();

            Consume(TokenType.RPAREN);

            MyStatement body = ParseStatement();

            return new MyWhileStatement(TokenType.WHILE, condition, body);
        }
        private MyDoWhileStatement ParseDoWhile()
        {
            Consume(TokenType.DO);

            MyStatement body = ParseStatement();

            Consume(TokenType.WHILE);

            Consume(TokenType.LPAREN);

            MyExpression condition = ParseExpression();

            Consume(TokenType.RPAREN);

            Consume(TokenType.SEMICOLON);

            return new MyDoWhileStatement(TokenType.DO, body, TokenType.WHILE, condition);
        }
        private void ParseFor()
        {
            Consume(TokenType.FOR);

            Consume(TokenType.LPAREN);

            MyExpression condition = ParseExpression();

            Consume(TokenType.SEMICOLON);

            ParseExpression();

            Consume(TokenType.SEMICOLON);

            ParseExpression();

            Consume(TokenType.RPAREN);

            ParseStatement();
        }
        private MyExpressionStatement ParseExpressionStatement()
        {
            MyExpression expression = ParseExpression();

            Consume(TokenType.SEMICOLON);

            return new MyExpressionStatement(expression);
        }

        private MyAssignment ParseAssignment()
        {
            Token name = ConsumeIdentifier();

            Consume(TokenType.ASSIGN);

            MyExpression value = ParseExpression();

            Consume(TokenType.SEMICOLON);

            if (!symbolTable.Exists(name.Lexeme))
            {
                errorHandler.Add($"Variable '{name.Lexeme}' is not declared.", name.Line, name.Column);
            }

            return new MyAssignment(name.Lexeme, value);
        }
        private Token PeekNext()
        {
            if (current + 1 >= tokens.Count)
                return tokens[current];

            return tokens[current + 1];
        }

        /*private MyExpression ParseExpression()
        {
            MyExpression left;

            if (Peek().Type == TokenType.IDENTIFIER)
            {
                Token identifier = Advance();

                left = new MyIdentifierExpression(identifier.Lexeme);
            }
            else if (Peek().Type == TokenType.INTEGER)
            {
                Token number = Advance();

                left = new MyLiteralExpression(number.Lexeme);
            }
            else
            {
                errorHandler.Add($"Unexpected token {Peek().Type} in expression",Peek().Line,Peek().Column);

                return new MyLiteralExpression("0");
            }

            if (Match(TokenType.PLUS))
            {
                MyExpression right = ParseExpression();

                return new MyBinaryExpression(left,TokenType.PLUS,right);
            }

            if (Match(TokenType.MINUS))
            {
                MyExpression right = ParseExpression();

                return new MyBinaryExpression(left,TokenType.MINUS,right);
            }

            if (Match(TokenType.MULTIPLY))
            {
                MyExpression right = ParseExpression();

                return new MyBinaryExpression(left,TokenType.MULTIPLY,right);
            }

            if (Match(TokenType.DIVIDE))
            {
                MyExpression right = ParseExpression();

                return new MyBinaryExpression(left,TokenType.DIVIDE,right);
            }

            if (Match(TokenType.LESS))
            {
                MyExpression right = ParseExpression();

                return new MyBinaryExpression(left,TokenType.LESS,right);
            }

            if (Match(TokenType.GREATER))
            {
                MyExpression right = ParseExpression();

                return new MyBinaryExpression(left,TokenType.GREATER,right);
            }

            if (Match(TokenType.EQUAL))
            {
                MyExpression right = ParseExpression();

                return new MyBinaryExpression(left,TokenType.EQUAL,right);
            }

            if (Match(TokenType.INCREMENT))
            {
                return new MyUnaryExpression(TokenType.INCREMENT,left);
            }

            if (Match(TokenType.DECREMENT))
            {
                return new MyUnaryExpression(TokenType.DECREMENT,left);
            }

            return left;
        }*/

        private MyExpression ParseExpression()
        {
            return ParseComparison();
        }

        private MyExpression ParseComparison()
        {
            MyExpression left = ParseAddition();

            while (Match(TokenType.LESS) || Match(TokenType.GREATER) || Match(TokenType.EQUAL))
            {
                TokenType op = Previous().Type;

                MyExpression right = ParseAddition();

                left = new MyBinaryExpression(left, op, right);
            }

            return left;
        }

        private MyExpression ParseAddition()
        {
            MyExpression left = ParseMultiplication();

            while (Match(TokenType.PLUS) || Match(TokenType.MINUS))
            {
                TokenType op = Previous().Type;

                MyExpression right = ParseMultiplication();

                left = new MyBinaryExpression(left, op, right);
            }

            return left;
        }

        private MyExpression ParseMultiplication()
        {
            MyExpression left = ParsePrimary();

            while (Match(TokenType.MULTIPLY) || Match(TokenType.DIVIDE))
            {
                TokenType op = Previous().Type;

                MyExpression right = ParsePrimary();

                left = new MyBinaryExpression(left, op, right);
            }

            return left;
        }

        private MyExpression ParsePrimary()
        {
            if (Match(TokenType.IDENTIFIER))
            {
                return new MyIdentifierExpression(Previous().Lexeme);
            }

            if (Match(TokenType.INTEGER))
            {
                return new MyLiteralExpression(Previous().Lexeme);
            }

            errorHandler.Add($"Unexpected token {Peek().Type} in expression", Peek().Line, Peek().Column);

            return new MyLiteralExpression(string.Empty);
            //return new MyLiteralExpression("0");
        }







    }
}
