using System.Text.RegularExpressions;

namespace RedstoneScript.Lexer;

public partial class RedstoneTokenizer
{
    /// <summary>
    /// Tokenizes the input inot a list of Tokens.
    /// </summary>
    public static List<Token> Tokenize(string sourceCode)
    {
        var tokens = new List<Token>();
        var allLines = sourceCode.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        
        foreach(string line in allLines)
        {
            TokenizeLine(line, tokens);   
            tokens.Add(new Token("\\n", TokenType.NewLine));
        }
        
        // end of file token
        tokens.Add(new Token(string.Empty, TokenType.EOF));

        return tokens;
    }

    private static void TokenizeLine(string line, List<Token> tokens)
    {
        // we want to split by all " " because our language will be space sensitive for now
        var all = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        foreach(string input in all)
        {
            if (input == "(")
            {
                tokens.Add(new Token(input, TokenType.ParenthesisOpen));
                continue;
            }
            else if (input == ")")
            {
                tokens.Add(new Token(input, TokenType.ParenthesisClose));
                continue;
            }

            else if (input == "[")
            {
                tokens.Add(new Token(input, TokenType.BracketOpen));
                continue;
            }
            else if (input == "]")
            {
                tokens.Add(new Token(input, TokenType.BracketClose));
                continue;
            }

            else if (input == "{")
            {
                tokens.Add(new Token(input, TokenType.BraceOpen));
                continue;
            }
            else if (input == "}")
            {
                tokens.Add(new Token(input, TokenType.BraceClose));
                continue;
            }

            else if (input == ".")
            {
                tokens.Add(new Token(".", TokenType.Dot));
                continue;
            }

            else if (input == ":")
            {
                tokens.Add(new Token(input, TokenType.Colon));
                continue;
            }

            else if (input == ",")
            {
                tokens.Add(new Token(input, TokenType.Comma));
                continue;
            }

            // math operators
            else if (input is "+" or "-" or "*" or "/" or "%")
            {
                tokens.Add(new Token(input, TokenType.Operator));
                continue;
            }

            else if (input == "=")
            {
                tokens.Add(new Token(input, TokenType.Equals));
                continue;
            }

            // keywords like item, repeater, etc
            else if (Keywords.TryGetKeyword(input, out var keywordType))
            {
                tokens.Add(new Token(input, keywordType));
                continue;
            }

            // number
            else if (double.TryParse(input, out _))
            {
                tokens.Add(new Token(input, TokenType.Number));
                continue;
            }

            // identifier
            else if (IsIdentifer(input))
            {
                tokens.Add(new Token(input, TokenType.Identifier));
            }
            
            // error handling
            else
            {
                Console.WriteLine("Unrecognized input in source: {0}", input);
                throw new InvalidOperationException("Redstone Token Parser: Invalid Syntax");
            }
        }   

    }

    private static bool IsIdentifer(string input)
    {
        // identifier (letters + numbers, must include at least one letter)
        return isIdentiferRegex().IsMatch(input);
    }

    [GeneratedRegex(@"^[A-Za-z0-9]*[A-Za-z][A-Za-z0-9]*$")]
    private static partial Regex isIdentiferRegex();
}
