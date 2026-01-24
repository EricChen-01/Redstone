namespace RedstoneScript.AST;

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