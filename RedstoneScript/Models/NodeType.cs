namespace RedstoneScript.AST;

public enum NodeType
{
    // Statements
    Program,
    VariableDeclaration,


    // Expressions
    NumericLiteral,
    Identifier,
    BinaryExpression,
    NullLiteral,
    BooleanLiteral,
    AssignmentExpression,
    
}
