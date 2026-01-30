using System;

namespace RedstoneScript.AST;

public class ForStatementNode : StatementNode
{
    public StatementNode Initializer { get; }

    public ExpressionNode Condition { get; }

    public ExpressionNode Increment { get; }

    public BlockStatementNode Body { get; }

    public ForStatementNode(StatementNode initializer, ExpressionNode condition, ExpressionNode increment, BlockStatementNode body) : base(NodeType.ForStatement)
    {
        Initializer = initializer;
        Condition = condition;
        Increment = increment;
        Body = body;
    }

public override string ToString(int indent)
    {
        return $@"{AstPrinter.Indent(indent)}ForStatement
        {AstPrinter.Indent(indent + 1)}Condition:
        {AstPrinter.Indent(indent + 2)}{Condition}
        {AstPrinter.Indent(indent + 1)}Increment:
        {AstPrinter.Indent(indent + 2)}{Increment}
        {AstPrinter.Indent(indent + 1)}Body:
        {AstPrinter.Indent(indent + 2)}{Body}";
    }
}
