﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cirno
{
    class Parser
    {
        List<Token> tokens;
        Token currentToken;
        int currentIndex;
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.currentToken = null;
            this.currentIndex = -1;

            Advance();
        }
        public TreeNode ParseAST()
        {
            List<Node> ast = new List<Node>();
            while(currentToken != null)
            {
                ast.Add(Parse());
            }
            return new TreeNode(ast);
        }
        public Node Parse()
        {
            if (Match(TokenType.PRINT))
            {
                Advance();
                Node expr = Expr();
                return new PrintNode(expr);
            }
            else if (Match(TokenType.BREAK))
            {
                Advance();
                return new BreakNode();
            }
            else if (Match(TokenType.RETURN))
            {
                Advance();
                Node expr = Expr();
                return new ReturnNode(expr);
            }
            else if (Match(TokenType.WHILE))
            {
                Token doToken = currentToken;
                Advance();
                Node expr = Expr();

                List<Node> ast = new List<Node>();
                while (!Match(TokenType.END))
                {
                    if (currentToken == null || Peak() == null && !Match(TokenType.END))
                    {
                        ErrorManager.autocheck = true;
                        ErrorManager.AddError(new Error($"Cannot find end after do.", doToken.line, ErrorType.UnfoundToken, ErrorSafety.Fatal));
                        break;
                    }
                    ast.Add(Parse());
                }
                Consume(TokenType.END);
                return new WhileNode(expr, new TreeNode(ast));
            }
            else if (Match(TokenType.FN))
            {
                Advance();
                string name = (string)Consume(TokenType.IDENTIFIER).lexeme;
                List<string> parameters = new List<string>();
                Consume(TokenType.OPEN_PAREN);
                while(!Match(TokenType.CLOSED_PAREN))
                {
                    parameters.Add((string)Consume(TokenType.IDENTIFIER).lexeme);
                    if(!Match(TokenType.COMMA) && !Match(TokenType.CLOSED_PAREN))
                    {
                        ErrorManager.AddError(new Error($"Unexpected parameter when constructing function \"{name}\", with culprit being \"{currentToken.lexeme}\".", ErrorType.UnexpectedToken, ErrorSafety.Fatal));
                    }
                    else if(Match(TokenType.COMMA))
                    {
                        Consume(TokenType.COMMA);
                    }
                }
                Consume(TokenType.CLOSED_PAREN);
                List<Node> ast = new List<Node>();
                while (!Match(TokenType.END))
                {
                    ast.Add(Parse());
                }
                Consume(TokenType.END);
                return new FunctionNode(name, new InnerFunctionNode(parameters, new TreeNode(ast)));
            }
            else
            {
                return Expr();
            }
        }

        Node Identifier()
        {
            string name = (string)currentToken.lexeme;
            Node ret = new IdentifierNode(name);
            Advance();

            if (Match(TokenType.EQUALS))
            {
                Advance();

                ret = new SetVariableNode(name, Expr());
            }
            else if (Match(TokenType.OPEN_PAREN))
            {
                Consume(TokenType.OPEN_PAREN);

                List<Node> arguments = new List<Node>();
                while (!Match(TokenType.CLOSED_PAREN))
                {
                    arguments.Add(Expr());
                    if (!Match(TokenType.COMMA) && !Match(TokenType.CLOSED_PAREN))
                    {
                        ErrorManager.AddError(new Error($"Unexpected parameter when constructing call \"{name}\", with the culprit being \"{currentToken.lexeme}\".", ErrorType.UnexpectedToken, ErrorSafety.Fatal));
                    }
                    else if (Match(TokenType.COMMA))
                    {
                        Consume(TokenType.COMMA);
                    }
                }
                Consume(TokenType.CLOSED_PAREN);

                ret = new CallNode(((IdentifierNode)ret).name, arguments);
            }

            return ret;
        }

        Node Factor()
        {
            Node ret = null;
            if (Match(TokenType.NUMBER))
            {
                ret = new NumberNode((float)currentToken.lexeme);
                Advance();
            }
            else if (Match(TokenType.STRING))
            {
                ret = new StringNode((string)currentToken.lexeme);
                Advance();
            }
            else if (Match(TokenType.IDENTIFIER))
            {
                ret = Identifier();
            }
            else if (Match(TokenType.BOOL))
            {
                ret = new BoolNode((bool)currentToken.lexeme);
                Advance();
            }
            else if (Match(TokenType.NOVA))
            {
                ret = new NovaNode();
                Advance();
            }
            else if (Match(TokenType.OPEN_PAREN))
            {
                Advance();
                ret = Expr();
                Consume(TokenType.CLOSED_PAREN);
            }
            else if (Match(TokenType.DO))
            {
                Token doToken = currentToken;
                Advance();
                List<Node> ast = new List<Node>();
                while (!Match(TokenType.END))
                {
                    if (currentToken == null || Peak() == null && !Match(TokenType.END))
                    {
                        ErrorManager.autocheck = true;
                        ErrorManager.AddError(new Error($"Cannot find end after do.", doToken.line, ErrorType.UnfoundToken, ErrorSafety.Fatal));
                        break;
                    }
                    ast.Add(Parse());
                }
                Consume(TokenType.END);
                ret = new TreeNode(ast);
            }
            else if (Match(TokenType.IF))
            {
                Dictionary<Node, TreeNode> ifActions = new Dictionary<Node, TreeNode>();
                TreeNode elseAction = null;

                while(true)
                {
                    if(Match(TokenType.IF, TokenType.ELIF))
                    {
                        Advance();
                        Node expr = Expr();
                        List<Node> ast = new List<Node>();
                        while(!Match(TokenType.ELIF, TokenType.ELSE, TokenType.END))
                        {
                            ast.Add(Parse());
                        }
                        ifActions.Add(expr, new TreeNode(ast));
                    }
                    else if(Match(TokenType.ELSE))
                    {
                        Advance();
                        List<Node> ast = new List<Node>();
                        while (!Match(TokenType.END))
                        {
                            ast.Add(Parse());
                        }
                        elseAction = new TreeNode(ast);
                    }
                    else if (Match(TokenType.END))
                    {
                        Advance();
                        break;
                    }
                }
                ret = new IfNode(ifActions, elseAction);
            }
            else
            {
                if(currentToken != null)
                {
                    ErrorManager.AddError(new Error($"Unexpected token \"{currentToken.type}\".", currentToken.line, ErrorType.UnexpectedToken, ErrorSafety.Fatal));
                }
                Advance();
            }
            return ret;
        }
        Node Term()
        {
            Node left = Factor();
            while (Match(TokenType.STAR, TokenType.SLASH, TokenType.MOD))
            {
                Token op = currentToken;
                Advance();
                Node right = Factor();
                left = new BinaryOperatorNode(left, right, op);
            }
            return left;
        }
        Node Expression()
        {
            Node left = Term();
            while(Match(TokenType.PLUS, TokenType.MINUS))
            {
                Token op = currentToken;
                Advance();
                Node right = Term();
                left = new BinaryOperatorNode(left, right, op);
            }
            return left;
        }
        Node EqualityExpression()
        {
            Node left = Expression();
            while (Match(TokenType.EQUALS_EQUALS, TokenType.BANG_EQUALS, TokenType.LESS_THAN, TokenType.GREATER_THAN, TokenType.LESS_EQUALS, TokenType.GREATER_EQUALS))
            {
                Token op = currentToken;
                Advance();
                Node right = Expression();
                left = new BinaryOperatorNode(left, right, op);
            }
            return left;
        }
        Node LogicalExpression()
        {
            Node left = EqualityExpression();
            while (Match(TokenType.AND, TokenType.OR))
            {
                Token op = currentToken;
                Advance();
                Node right = EqualityExpression();
                left = new BinaryOperatorNode(left, right, op);
            }
            return left;
        }

        Node Expr()
        {
            return LogicalExpression();
        }

        bool Match(params TokenType[] tokens)
        {
            if(currentToken == null) return false;

            foreach(TokenType token in tokens)
            {
                if(currentToken.type == token)
                {
                    return true;
                }
            }
            return false;
        }

        Token Advance()
        {
            currentIndex++;
            if (currentIndex >= tokens.Count)
            {
                currentToken = null;
            }
            else
            {
                currentToken = tokens[currentIndex];
            }
            return currentToken;
        }
        Token Peak()
        {
            if (currentIndex + 1 >= tokens.Count)
            {
                return null;
            }
            else
            {
                return tokens[currentIndex + 1];
            }
        }
        Token Consume(TokenType type)
        {
            if(currentToken.type != type)
            {
                ErrorManager.AddError(new Error($"Expected \"{type}\", instead got {currentToken.type}", currentToken.line, ErrorType.UnexpectedToken, ErrorSafety.Fatal));
                return null;
            }
            else
            {
                Token ret = currentToken;
                Advance();
                return ret;
            }
        }
    }
}
