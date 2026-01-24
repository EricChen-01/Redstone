using System.Text;

namespace RedstoneScript.AST;

/// <summary>
/// Basic Node
/// </summary>
public interface INode
{
    public NodeType Type {get; }
    string ToString(int indent);
}

/// <summary>
/// A representation of an Expression. Which produces a value.
/// </summary>
public abstract class ExpressionNode : INode
{
    public NodeType Type { get; }

    protected ExpressionNode(NodeType type)
    {
        Type = type;
    }

    public override string ToString()
        => ToString(0);

    public abstract string ToString(int indent);
}

/// <summary>
/// A representation of a Statement. Which executes behavior
/// </summary>
public abstract class StatementNode : INode
{
    public NodeType Type { get; }

    protected StatementNode(NodeType type)
    {
        Type = type;
    }

    public override string ToString()
        => ToString(0);

    public abstract string ToString(int indent);
}

/// <summary>
/// The root node that will contain all the other nodes. This is the entrypoint and the Program.
/// </summary>
public class ProgramNode : INode
{
    public NodeType Type { get; } = NodeType.Program;

    public List<INode> Nodes { get; }

    public ProgramNode(List<INode> nodes)
    {
        Nodes = nodes;
    }

    public override string ToString()
    {
        return ToString(0);
    }

    public string ToString(int indent)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{AstPrinter.Indent(indent)}Program");

        foreach (var node in Nodes)
        {
            sb.AppendLine(node.ToString(indent + 1));
        }

        return sb.ToString().TrimEnd();
    }

}

#region Helpers
public static class AstPrinter
{
    public static string Indent(int level)
        => new string(' ', level * 2);
}

#endregion