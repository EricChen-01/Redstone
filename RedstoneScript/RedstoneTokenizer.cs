using System.Text;
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
        int currentCharacterIndex = 0;

        // parse until end of source code
        while(currentCharacterIndex != sourceCode.Length)
        {
            char character = sourceCode[currentCharacterIndex];

            // handling cases
            if (IsSkippableCharacter(character))
            {
                currentCharacterIndex++;
                continue;
            }

            if (character == '\n')
            {
                tokens.Add(new Token("\\n", TokenType.NewLine));
                currentCharacterIndex++;
                continue;
            }

            if (character == '/' && currentCharacterIndex + 1 < sourceCode.Length && sourceCode[currentCharacterIndex + 1] == '/')
            {
                // Skip until end of line or end of file
                while (currentCharacterIndex < sourceCode.Length && sourceCode[currentCharacterIndex] != '\n')
                {
                    currentCharacterIndex++;
                }
                continue;
            }

            // handles boolean operators like ==, !=, <, <=, >, >=, and !
            if ("=!<>".Contains(character))
            {
                char? next = (currentCharacterIndex + 1 < sourceCode.Length) ? sourceCode[currentCharacterIndex + 1] : null;
                string? combined = next != null ? $"{character}{next}" : null;

                if (combined == "==" || combined == "!=" || combined == "<=" || combined == ">=")
                {
                    tokens.Add(new Token(combined, TokenType.Operator));
                    currentCharacterIndex += 2;
                    continue;
                }

                if (character == '<' || character == '>' || character == '!')
                {
                    tokens.Add(new Token(character.ToString(), TokenType.Operator));
                    currentCharacterIndex += 1;
                    continue;
                }
            }

            switch (character)
            {
                case '(':
                    tokens.Add(new Token("(", TokenType.ParenthesisOpen)); currentCharacterIndex++; continue;
                case ')':
                    tokens.Add(new Token(")", TokenType.ParenthesisClose)); currentCharacterIndex++; continue;
                case '{':
                    tokens.Add(new Token("{", TokenType.BraceOpen)); currentCharacterIndex++; continue;
                case '}':
                    tokens.Add(new Token("}", TokenType.BraceClose)); currentCharacterIndex++; continue;
                case '[':
                    tokens.Add(new Token("[", TokenType.BracketOpen)); currentCharacterIndex++;; continue;
                case ']':
                    tokens.Add(new Token("]", TokenType.BracketClose)); currentCharacterIndex++; continue;
                case '.':
                    tokens.Add(new Token(".", TokenType.Dot)); currentCharacterIndex++; continue;
                case ':':
                    tokens.Add(new Token(":", TokenType.Colon)); currentCharacterIndex++; continue;
                case ',':
                    tokens.Add(new Token(",", TokenType.Comma)); currentCharacterIndex++; continue;
                case '=':
                    tokens.Add(new Token("=", TokenType.Equals)); currentCharacterIndex++; continue;
            }

            // handles math operators
            if ("+-*/%".Contains(character))
            {
                tokens.Add(new Token(character.ToString(), TokenType.Operator));
                currentCharacterIndex++;
                continue;
            }

            // handles numbers
            if (char.IsDigit(character))
            {
                int start = currentCharacterIndex;

                while (
                    (currentCharacterIndex < sourceCode.Length) && 
                    (char.IsDigit(sourceCode[currentCharacterIndex]) ||
                    sourceCode[currentCharacterIndex] == '.'))
                {
                    currentCharacterIndex++;
                }

                var number = sourceCode[start..currentCharacterIndex];
                if (!IsValidNumber(number))
                {
                    throw new InvalidOperationException("Redstone Token Parser: Invalid Syntax");
                }
                tokens.Add(new Token(number, TokenType.Number));
                continue;
            }

            // handles identifiers and keywords
            if (char.IsLetter(character) || character == '_')
            {
                int start = currentCharacterIndex;

                while (currentCharacterIndex < sourceCode.Length && 
                    (char.IsLetterOrDigit(sourceCode[currentCharacterIndex]) || sourceCode[currentCharacterIndex] == '_'))
                {
                    currentCharacterIndex++;
                }
                var text = sourceCode.Substring(start, currentCharacterIndex - start);

                if (IsIdentifer(text) && Keywords.TryGetKeyword(text, out var keywordType))
                {
                    tokens.Add(new Token(text, keywordType));   
                }
                else if (IsIdentifer(text))
                {
                    tokens.Add(new Token(text, TokenType.Identifier));                    
                }
                else
                {
                    throw new InvalidOperationException($"Redstone Token Parser: Invalid Syntax. {text} cannot be an identifier.");
                }
                continue;
            }

            // handle strings
            var isValidString = false;
            if (character == '"')
            {
                var sb = new StringBuilder();
                var start = currentCharacterIndex;
                currentCharacterIndex++; // move past the "

                while (currentCharacterIndex < sourceCode.Length)
                {
                    char c = sourceCode[currentCharacterIndex];
                    
                    // handle closing quote
                    if (c == '"')
                    {
                        currentCharacterIndex++; // consume closing "
                        tokens.Add(new Token(sb.ToString(), TokenType.String));
                        isValidString = true;
                        break;
                    }

                    // handles literal back slash '\'
                    if (c == '\\')
                    {
                        currentCharacterIndex++;
                        if (currentCharacterIndex >= sourceCode.Length)
                        {
                            throw new InvalidOperationException($"Redstone Token Parser: Unterminated string at end of file.");   
                        }

                        char escape = sourceCode[currentCharacterIndex];
                        sb.Append(escape switch
                        {
                            'n' => '\n',
                            't' => '\t',
                            '"' => '"',
                            '\\' => '\\',
                            _ => throw new Exception($"Invalid escape \\{escape}")
                        });
                    }
                    else
                    {
                        sb.Append(c);
                    }

                    currentCharacterIndex++;
                }
                
                if (!isValidString)
                {
                    throw new Exception($"Redstone Token Parser: Unterminated string literal detected at {start}.");
                }

                continue;
            }

            // error handling
            throw new Exception($"Redstone Token Parser: Unexpected character '{character}' at position {currentCharacterIndex}.");
        }
        
        // end of file token
        tokens.Add(new Token(string.Empty, TokenType.EOF));

        return tokens;
    }
    
    private static bool IsSkippableCharacter(char c)
    {
        if (c == ' ' || c == '\t' || c == '\r')
        {
            return true;
        }

        return false;
    }

    private static bool IsIdentifer(string input)
    {
        // identifier (letters + numbers, must include at least one letter)
        return isIdentiferRegex().IsMatch(input);
    }

    [GeneratedRegex(@"^[A-Za-z0-9]*[A-Za-z][A-Za-z0-9]*$")]
    private static partial Regex isIdentiferRegex();

    private static bool IsValidNumber(string input)
    {
        return isValidNumberRegex().IsMatch(input);
    }

    [GeneratedRegex(@"^(0|[1-9][0-9]*)(\.[0-9]+)?$")]
    private static partial Regex isValidNumberRegex();

}
