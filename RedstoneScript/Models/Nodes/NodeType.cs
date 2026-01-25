namespace RedstoneScript.AST;

public enum NodeType
{
    // Root
    Program,

    // Statements
    VariableDeclaration,
    FunctionDeclaration,
    BlockStatement,
    ReturnStatement,
    IfStatement,
    WhileStatement,
    ForStatement,
    BreakStatement,

    // Expressions
    NumericLiteral,
    NullLiteral,
    BooleanLiteral,
    StringLiteral,

    Identifier,

    AssignmentExpression,
    BinaryExpression,

    ObjectLiteral,
    Property,
    MemberAccessExpression,
    CallExpression,
}
