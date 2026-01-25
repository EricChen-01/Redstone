namespace RedstoneScript.AST;

public class BreakStatementNode : StatementNode
{
    public BreakStatementNode() : base(NodeType.BreakStatement)
    {
    }

    public override string ToString(int indent)
    {
        return "<break statement>";
    }
}
