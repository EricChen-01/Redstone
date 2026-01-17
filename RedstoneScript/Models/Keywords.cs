namespace RedstoneScript.Lexer;

public static class Keywords
{
    /// <summary>
    /// Maps all the Redstone Language Keywords to the Token equivalent.
    /// </summary>
    private static Dictionary<string, TokenType> KeywordsDictionary = new()
    {
        {"item", TokenType.Variable},
        {"comparator", TokenType.If},
        {"repeater", TokenType.While},
        {"hopper", TokenType.For},
        {"craft", TokenType.Function},
        {"air", TokenType.Null}
    };

    /// <summary>
    /// Tries and gets the Keyword.
    /// </summary>
    /// <param name="key">the keyword to get</param>
    /// <param name="tokenType">the output token type</param>
    /// <returns>true if the key exists as a keyword. False otherwise.</returns>
    public static bool TryGetKeyword(string key, out TokenType tokenType)
    {
        return KeywordsDictionary.TryGetValue(key, out tokenType);
    }


}
