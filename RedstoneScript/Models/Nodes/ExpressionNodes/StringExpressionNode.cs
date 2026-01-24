namespace RedstoneScript.AST;

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