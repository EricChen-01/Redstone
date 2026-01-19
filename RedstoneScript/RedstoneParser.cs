using RedstoneScript.Lexer;

namespace RedstoneScript.AST.Parser;

public class RedstoneParser
{
    private List<Token> tokens = new();
    private int currentTokenIndex = 0;


    public RedstoneParser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    // Entry point
    public ProgramNode ParseRoot()
    {
        var statements = new List<INode>();

        while (!IsAtEnd())
        {            
            statements.Add(ParseStatement());

            IgnoreNewLine();
            // if (IsAtEnd()) break; // stop at EOF
        }

        return new ProgramNode(statements);
    }

    /// <summary>
    /// Parses a statement node category
    /// </summary>
    private INode ParseStatement()
    {
        // fallback.
        switch (Current().Type)
        {
            case TokenType.Variable:
            case TokenType.Constant:
                return ParseVariableDeclaration();
            default:
                return ParseExpression();
        }
    }

    private StatementNode ParseVariableDeclaration()
    {
        var isConstant = Advance().Type == TokenType.Constant;
        var identifierName = Expect(TokenType.Identifier, "Expected a variable name.").Value;
        
        // check if it's any of the keywords that are restricted
        if (Keywords.TryGetKeyword(identifierName, out TokenType matched))
        {
            throw new InvalidOperationException($"Redstone Node Parser: Cannot use reserved keyword: {matched}");
        }
        
        // handle constant
        // pattern below must happen.
        // const variableName = ...
        // pattern below cannot happen
        // const varName
        if (isConstant)
        {
            Expect(TokenType.Equals, "Expected an equal sign to the right of the variable.");
            var expression = ParseExpression();
            Expect(TokenType.NewLine, "Expected a new line character.");
            return new VariableDelarationNode(identifierName, expression, true);
        }

        // handle regular var declarations
        // var varName = ...
        // OR 
        // var varName
        if (Current().Type == TokenType.Equals)
        {
            Expect(TokenType.Equals);
            var expression = ParseExpression();
            Expect(TokenType.NewLine);   
            return new VariableDelarationNode(identifierName, expression, isConstant); 
        }else if(Current().Type == TokenType.NewLine)
        {
            Expect(TokenType.NewLine);   
            return new VariableDelarationNode(identifierName, null, isConstant); 
        }

        // throw error
        throw new NotImplementedException("Redstone Node Parser: Parsing variable declaration is not supported yet.");
    }

    /// <summary>
    /// Parses an expression node category
    /// </summary>
    /// <remarks></remarks>
    private ExpressionNode ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private ExpressionNode ParseMultiplicitiveExpression()
    {
        var left = ParsePrimaryExpression();

        while(
            IsToken(TokenType.Operator, OperatorType.MULTIPLICATION) ||
            IsToken(TokenType.Operator, OperatorType.DIVISION) || 
            IsToken(TokenType.Operator, OperatorType.MODULUS))
        {
            var operation = Advance().Value;
            var right = ParsePrimaryExpression();
            left = new BinaryExpressionNode(left, right, operation);
        }


        return left;
    }

    private ExpressionNode ParseAdditiveExpression()
    {
        var left = ParseMultiplicitiveExpression();

        while (
            IsToken(TokenType.Operator, OperatorType.ADDITION) || 
            IsToken(TokenType.Operator, OperatorType.SUBTRACTION))
        {
            var operation = Advance().Value;
            var right = ParseMultiplicitiveExpression();
            left = new BinaryExpressionNode(left, right, operation);
        }

        return left;
    }

    private ExpressionNode ParseAssignmentExpression()
    {
        var left = ParseAdditiveExpression(); // in future we'll do objects parsing for this

        if (Match(TokenType.Equals)) // if it's an equal tokentype. then we proceed and advance.
        {
            var right = ParseAssignmentExpression();
            return new AssignmentExpressionNode(left, right);
        }

        return left;
    }

    private ExpressionNode ParsePrimaryExpression()
    {
        var token = Current();
        
        switch (token.Type)
        {
            case TokenType.Number:
                if (double.TryParse(Advance().Value, out double parsedDouble))
                    return new NumericExpressionNode(parsedDouble);
                throw new InvalidOperationException("Redstone Node Parser: The parser tried to parse a TokenType.Number, but failed.");
            case TokenType.Identifier:
                return new IdentifierExpressionNode(Advance().Value);
            case TokenType.ParenthesisOpen:
                Advance();
                var expression = ParseExpression();
                Expect(TokenType.ParenthesisClose, "Unexpected token found. Expected closing parenthesis.");
                return expression;
            case TokenType.Null:
                Advance();
                return new NullExpressionNode();
            case TokenType.True:
                Advance();
                return new BooleanExpressionNode(true);
            case TokenType.False:
                Advance();
                return new BooleanExpressionNode(false);
            default:
                throw new Exception($"Redstone Node Parser: Unhandled parsing error: {token.Type} was not handled. Could it be that it's not supported yet?");
        }
    }

#region helpers
    private bool IsAtEnd() => Current().Type == TokenType.EOF;

    private Token Previous(){
        if (currentTokenIndex == 0)
        {
            throw new IndexOutOfRangeException("Redstone Node Parser: No previous Token.");
        } 
        return tokens[currentTokenIndex - 1];
    }

    private Token Advance()
    {
        if (!IsAtEnd())
        {
            currentTokenIndex++;
        } 

        return Previous();
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd())
            return false;

        return Current().Type == type;
    }

    private bool Match(params TokenType[] types)
    {
        foreach (var type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }
        return false;
    }

    private Token Current() => tokens[currentTokenIndex];

    private bool IsToken(TokenType tokenType, string value) => (Current().Type == tokenType) && (Current().Value == value); 

    private void IgnoreNewLine()
    {
        while(Current().Type == TokenType.NewLine)
        {
            Advance();
        }
    }

    private Token Expect(TokenType type, string? errorMessage = null)
    {
        if (Check(type))
            return Advance();

        throw new Exception(
            errorMessage ?? $"Redstone Node Parser: Expected {type}, got {Current().Type}"
        );
    }
#endregion
}
