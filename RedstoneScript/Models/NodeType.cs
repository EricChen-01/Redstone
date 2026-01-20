namespace RedstoneScript.AST;

public enum NodeType
{
    // Root
    Program,
    Keyword,

    // Statements
    VariableDeclaration,


    // Expressions
    NumericLiteral,
    NullLiteral,
    BooleanLiteral,

    Identifier,

    AssignmentExpression,
    BinaryExpression,

    ObjectLiteral,
    Property,
}
