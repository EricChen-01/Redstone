using System.Text;

namespace RedstoneScript.AST;

public class BlockStatementNode : StatementNode
{
    public List<INode> Statements { get; }

    public BlockStatementNode(List<INode> statements)
        : base(NodeType.BlockStatement)
    {
        Statements = statements;
    }

    public override string ToString(int indent)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{AstPrinter.Indent(indent)}Block");

        foreach (var stmt in Statements)
        {
            sb.AppendLine(stmt.ToString(indent + 1));
        }

        return sb.ToString().TrimEnd();
    }
}

#endregion