namespace RedstoneScript.AST;

public class UnaryExpressionNode : ExpressionNode
{
    public string Operator { get; }
    public ExpressionNode Right { get; }
    public UnaryExpressionNode(string @operator, ExpressionNode right) : base(NodeType.UnaryExpression)
    {
        Operator = @operator;
        Right = right;
    }

    public override string ToString(int indent)
    {
        return $@"{AstPrinter.Indent(indent)}BinaryExpression ({Operator})
        {AstPrinter.Indent(indent + 1)}Right:
        {AstPrinter.Indent(indent + 2)}{Right}";
    }
}
