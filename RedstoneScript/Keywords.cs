namespace RedstoneScript.Lexer;

public static class Keywords
{
    private static Dictionary<string, TokenType> KeywordsDictionary = new()
    {
        {"chest", TokenType.Chest}, // variable
        {"comparator", TokenType.Comparator}, // if
        {"repeater", TokenType.Repeater}, // while
        {"hopper", TokenType.Hooper}, // for loop
        {"craft", TokenType.Craft}, // function
        {"air", TokenType.Air} // null
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
