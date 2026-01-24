namespace RedstoneScript.AST;

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