namespace RedstoneScript.Lexer;

public enum TokenType
{
    EOF,               // End of file

    // Primatives
    Number,            // A number
    String,            // A string in the form "
    Identifier,        // The name of the variable / object 
    Air,               // The null literal
     

    // Groupings + Operations
    Operator,          // Mathmatical Operator such as +, -, *, /
    Equals,            // =
    ParenthesisOpen,   // (
    ParenthesisClose,  // )

    // Program Keywords
    Item,             // The keyword to assign variables
    Repeater,          // while loop
    Comparator,        // if statement 
    Craft,             // function
    Hooper,            // for loop
}
