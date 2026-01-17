namespace RedstoneScript.Lexer;

public enum TokenType
{
    EOF,               // End of file

    // Primatives
    Number,            // A number
    String,            // A string in the form "
    Identifier,        // The name of the variable / object 
    Null,               // Air
     

    // Groupings + Operations
    Operator,          // Mathmatical Operator such as +, -, *, /
    Equals,            // =
    ParenthesisOpen,   // (
    ParenthesisClose,  // )

    // Program Keywords
    Variable,          // Item
    While,             // Repeater
    If,                // Comparator
    Function,          // Craft
    For,              // Hooper
}
