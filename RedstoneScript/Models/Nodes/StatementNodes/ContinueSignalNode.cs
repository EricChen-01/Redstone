using RedstoneScript.AST;

namespace RedstoneScript.AST;

public class ContinueSignalNode : StatementNode
{
    public ContinueSignalNode() : base(NodeType.ContinueStatement)
    {
    }

    public override string ToString(int indent)
    {
        return "<continue statement>";
    }
}
