namespace RedstoneScript.AST;

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
