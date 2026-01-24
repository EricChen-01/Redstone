namespace RedstoneScript.AST;

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