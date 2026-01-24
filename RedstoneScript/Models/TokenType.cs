namespace RedstoneScript.Lexer;

public enum TokenType
{
    EOF,               // End of file
    NewLine,           // New line indicator

    // Primatives
    Number,            // A number
    String,            // A string in the form "
    Identifier,        // The name of the variable / object 
    Null,               // Air
    True,               // On
    False,              // Off

    // Groupings + Operations
    Operator,          // Mathmatical Operator such as +, -, *, /
    Equals,            // =
    ParenthesisOpen,   // (
    ParenthesisClose,  // )
    Comma,             // ,
    Colon,             // :
    BraceOpen,         // {
    BraceClose,         // }
    BracketOpen,        // [
    BracketClose,       // ]
    Dot,                // . 

    // Program Keywords
    Variable,          // Item
    Constant,          // Bedrock
    While,             // Repeater
    If,                // Comparator
    Else,              // Otherwise
    Function,          // Workbench
    Return,            // Craft
    For,              // Hooper
}
