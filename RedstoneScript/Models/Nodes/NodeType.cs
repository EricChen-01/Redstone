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
    ContinueStatement,

    // Expressions
    NumericLiteral,
    NullLiteral,
    BooleanLiteral,
    StringLiteral,

    Identifier,

    AssignmentExpression,
    BinaryExpression,
    UnaryExpression,

    ObjectLiteral,
    Property,
    MemberAccessExpression,
    CallExpression,
}
