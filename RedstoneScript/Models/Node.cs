using System.Reflection.Metadata;
using RedstoneScript.Lexer;

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

#region Statements
public class VariableDelarationNode : StatementNode
{
    public string Identifier { get; }

    public bool IsConstant { get; }

    public ExpressionNode? Value { get; }

    public VariableDelarationNode(string identifier, ExpressionNode? value, bool isConstant = false) : base(NodeType.VariableDeclaration)
    {
        Identifier = identifier;
        IsConstant = isConstant;
        Value = value;
    }
}

#endregion

#region Expressions
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
    public string Value { get; }= "air"; // This is representing null.
    
    public NullExpressionNode() : base(NodeType.NullLiteral)
    {}

    public override string ToString()
    {
        return $"NullLiteral";
    }
}

public class BooleanExpressionNode : ExpressionNode
{   
    public bool Value { get; }
    public BooleanExpressionNode(bool value) : base(NodeType.BooleanLiteral)
    {
        Value = value;
    }

    public override string ToString()
    {
        return "Boolean";
    }
}

public class AssignmentExpressionNode : ExpressionNode
{
    public ExpressionNode LeftExpression { get; } // an expression because it will help support things like complexObject.foo = 3 + 3
    
    public ExpressionNode RightExpression { get; }

    public AssignmentExpressionNode(ExpressionNode left, ExpressionNode right) : base(NodeType.AssignmentExpression)
    {
        LeftExpression = left;
        RightExpression = right;
    }

    public override string ToString()
    {
        return $"AssignmentExpression: {LeftExpression} = {RightExpression}";
    }

}

public class ObjectExpressionNode : ExpressionNode
{
    public List<PropertyExpressionNode> Properties { get; }
    

    public ObjectExpressionNode(List<PropertyExpressionNode> properties) : base(NodeType.ObjectLiteral)
    {
        Properties = properties;
    }

    public override string ToString()
    {
        return $"Object";
    }
}

public class PropertyExpressionNode : ExpressionNode
{
    public string Name { get; }

    public ExpressionNode? Value { get; }

    public PropertyExpressionNode(string name, ExpressionNode? value = null) : base(NodeType.Property)
    {
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return $"Property: {Name}: {Value}";
    }

}

#endregion