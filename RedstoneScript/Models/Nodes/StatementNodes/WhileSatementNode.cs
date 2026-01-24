namespace RedstoneScript.AST;

public class WhileSatementNode: StatementNode
{
    public ExpressionNode Condition { get; }
    public BlockStatementNode Body { get; }


    public WhileSatementNode(ExpressionNode condition, BlockStatementNode body)
        : base(NodeType.WhileStatement)
    {
        Condition = condition;
        Body = body;
    }

    public override string ToString(int indent)
    {
        return $@"{AstPrinter.Indent(indent)}WhileStatement
        {AstPrinter.Indent(indent + 1)}Condition:
        {AstPrinter.Indent(indent + 2)}{Condition}
        {AstPrinter.Indent(indent + 1)}Body:
        {AstPrinter.Indent(indent + 2)}{Body}";
    }
}

#endregion