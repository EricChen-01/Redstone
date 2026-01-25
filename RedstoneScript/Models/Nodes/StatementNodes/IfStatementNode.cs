namespace RedstoneScript.AST;

public class IfStatementNode : StatementNode
{
    public ExpressionNode Condition { get; }
    public BlockStatementNode Body { get; }
    public StatementNode? Else { get; }

    public IfStatementNode(ExpressionNode condition, BlockStatementNode body, StatementNode? @else)
        : base(NodeType.IfStatement)
    {
        Condition = condition;
        Body = body;
        Else = @else;
    }

    public override string ToString(int indent)
    {
        return $@"{AstPrinter.Indent(indent)}IfStatement
        {AstPrinter.Indent(indent + 1)}Condition:
        {AstPrinter.Indent(indent + 2)}{Condition}
        {AstPrinter.Indent(indent + 1)}Body:
        {AstPrinter.Indent(indent + 2)}{Body}
        {AstPrinter.Indent(indent + 1)}Else:
        {AstPrinter.Indent(indent + 2)}{Else}";

    }
}