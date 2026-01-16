namespace RedstoneScript.AST;

/// <summary>
/// Basic Node
/// </summary>
public interface INode
{
    public NodeType Type {get; }
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

    public abstract override string ToString();
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
        var childStrings = string.Join(", ", Nodes.Select(n => n.ToString()));
        return $"RootNode: [{childStrings}]";
    }

}

public class BinaryExpressionNode : ExpressionNode
{
    public ExpressionNode Left { get; init; }

    public ExpressionNode Right { get; init; }

    public string Operator { get; init; }

    public BinaryExpressionNode(ExpressionNode left, ExpressionNode right, string expressionOperator) : base(NodeType.BinaryExpression)
    {
        Left = left;
        Right = right;
        Operator = expressionOperator;
    }

    public override string ToString()
    {
        return $"BinaryExpression ({Operator}): [Left: {Left}, Right: {Right}]";
    }
}

public class NumericExpressionNode : ExpressionNode
{
    public double Value { get; }

    public NumericExpressionNode(double value) : base(NodeType.NumericLiteral)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"NumericLiteral {Value}";
    }
}

public class IdentifierExpressionNode : ExpressionNode
{
    public string Name { get; } 

    public IdentifierExpressionNode(string name) : base(NodeType.Identifier)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"Identifier {Name}";
    }
}

public class NullExpressionNode : ExpressionNode
{
    string Value { get; }= "Air"; // This is representing null.
    
    public NullExpressionNode() : base(NodeType.NullLiteral)
    {}

    public override string ToString()
    {
        return $"NullLiteral";
    }
}