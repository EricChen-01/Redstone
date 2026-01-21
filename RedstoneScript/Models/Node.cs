using System.Reflection.Metadata;
using System.Text;
using RedstoneScript.Lexer;

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

    public override string ToString(int indent)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{AstPrinter.Indent(indent)}VarDeclaration {Identifier}");

        if (Value != null)
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Value:");
            sb.AppendLine(Value.ToString(indent + 2));
        }

        return sb.ToString().TrimEnd();
    }

}

public class FunctionDelarationNode : StatementNode
{
    public string Name { get; }

    public List<string> Parameters { get; }

    public BlockStatementNode Body { get; }

    public FunctionDelarationNode(string name, List<string> parameters, BlockStatementNode body) : base(NodeType.FunctionDeclaration)
    {
        Name = name;
        Parameters = parameters;
        Body = body;
    }

    public override string ToString(int indent)
    {
        var sb = new StringBuilder();

        // Function declaration header
        sb.AppendLine($"{AstPrinter.Indent(indent)}FunctionDeclaration {Name}");

        // Parameters
        sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Parameters:");
        if (Parameters != null && Parameters.Count > 0)
        {
            foreach (var param in Parameters)
            {
                sb.AppendLine($"{AstPrinter.Indent(indent + 2)}{param}");
            }
        }
        else
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 2)}(none)");
        }

        // Body
        sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Body:");
        if (Body != null)
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 2)}(function body)");
        }
        else
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 2)}(empty)");
        }

        return sb.ToString().TrimEnd();
    }

}

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

public class ReturnStatementNode : StatementNode
{
    public ExpressionNode? Value { get; }

    public ReturnStatementNode(ExpressionNode? value)
        : base(NodeType.ReturnStatement)
    {
        Value = value;
    }


    public override string ToString(int indent)
    {
        return Value == null
            ? $"{AstPrinter.Indent(indent)}Return"
            : $"${AstPrinter.Indent(indent)}Return {Value}";
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

    public override string ToString(int indent)
    {
        return
    $@"{AstPrinter.Indent(indent)}BinaryExpression ({Operator})
        {AstPrinter.Indent(indent + 1)}Left:
        {AstPrinter.Indent(indent + 2)}{Left}
        {AstPrinter.Indent(indent + 1)}Right:
        {AstPrinter.Indent(indent + 2)}{Right}";
    }

}

public class NumericExpressionNode : ExpressionNode
{
    public double Value { get; }

    public NumericExpressionNode(double value) : base(NodeType.NumericLiteral)
    {
        Value = value;
    }

    public override string ToString(int indent)
    {
        return $"{AstPrinter.Indent(indent)}Number({Value})";
    }

}

public class StringExpressionNode : ExpressionNode
{
    public string Value { get; }

    public StringExpressionNode(string value) : base(NodeType.StringLiteral)
    {
        Value = value;
    }

    public override string ToString(int indent)
    {
        return $"{AstPrinter.Indent(indent)}String({Value})";
    }

}

public class IdentifierExpressionNode : ExpressionNode
{
    public string Name { get; } 

    public IdentifierExpressionNode(string name) : base(NodeType.Identifier)
    {
        Name = name;
    }

    public override string ToString(int indent)
    {
        return $"{AstPrinter.Indent(indent)}Identifier({Name})";
    }
}

public class NullExpressionNode : ExpressionNode
{
    public string Value { get; }= "air"; // This is representing null.
    
    public NullExpressionNode() : base(NodeType.NullLiteral)
    {}

    public override string ToString(int indent)
    {
        return $"{AstPrinter.Indent(indent)}Null";
    }
}

public class BooleanExpressionNode : ExpressionNode
{   
    public bool Value { get; }
    public BooleanExpressionNode(bool value) : base(NodeType.BooleanLiteral)
    {
        Value = value;
    }

    public override string ToString(int indent)
    {
        return $"{AstPrinter.Indent(indent)}Boolean({Value})";
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

    public override string ToString(int indent)
    {
        return
    $@"{AstPrinter.Indent(indent)}Assignment
{AstPrinter.Indent(indent + 1)}Target:
{AstPrinter.Indent(indent + 2)}{LeftExpression}
{AstPrinter.Indent(indent + 1)}Value:
{AstPrinter.Indent(indent + 2)}{RightExpression}";
    }


}

public class ObjectExpressionNode : ExpressionNode
{
    public List<PropertyExpressionNode> Properties { get; }
    

    public ObjectExpressionNode(List<PropertyExpressionNode> properties) : base(NodeType.ObjectLiteral)
    {
        Properties = properties;
    }

    public override string ToString(int indent)
    {
    var sb = new StringBuilder();
    sb.AppendLine($"{AstPrinter.Indent(indent)}ObjectLiteral");

    foreach (var prop in Properties)
    {
        sb.AppendLine(prop.ToString(indent + 1));
    }

    return sb.ToString().TrimEnd();
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

    public override string ToString(int indent)
    {
        if (Value == null)
            return $"{AstPrinter.Indent(indent)}Property({Name})";

        return
    $@"{AstPrinter.Indent(indent)}Property({Name})
{Value.ToString(indent + 1)}";
    }


}

public class MemberAccessExpression : ExpressionNode
{
    public ExpressionNode Object { get; } // we are using Expression because it could be getObject().x, where the object first needs to be evaluated from an expression.

    public ExpressionNode Property { get; }

    public MemberAccessExpression(ExpressionNode objectNode, ExpressionNode property) : base(NodeType.MemberAccessExpression)
    {
        Object = objectNode;
        Property = property;
    }

    public override string ToString(int indent)
    {
        return
    $@"{AstPrinter.Indent(indent)}MemberAccess
        {AstPrinter.Indent(indent + 1)}Object:
        {AstPrinter.Indent(indent + 2)}{Object}
        {AstPrinter.Indent(indent + 1)}Property:
        {AstPrinter.Indent(indent + 2)}{Property}";
    }


}

public class CallExpressionNode : ExpressionNode
{
    public ExpressionNode RightCallExpression { get; }

    public List<ExpressionNode> Arguments { get; }

    public CallExpressionNode(ExpressionNode rightCallExpression, List<ExpressionNode> arguments) : base(NodeType.CallExpression)
    {
        RightCallExpression = rightCallExpression;
        Arguments = arguments;
    }

    public override string ToString(int indent)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{AstPrinter.Indent(indent)}CallExpression");
        sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Callee:");
        sb.AppendLine($"{AstPrinter.Indent(indent + 2)}{RightCallExpression}");

        if (Arguments.Count > 0)
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Arguments:");
            foreach (var arg in Arguments)
            {
                sb.AppendLine($"{AstPrinter.Indent(indent + 2)}{arg}");
            }
        }

        return sb.ToString().TrimEnd();
    }


}

#endregion

#region Helpers
public static class AstPrinter
{
    public static string Indent(int level)
        => new string(' ', level * 2);
}

#endregion