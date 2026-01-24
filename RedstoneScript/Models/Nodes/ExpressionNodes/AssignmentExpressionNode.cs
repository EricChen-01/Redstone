namespace RedstoneScript.AST;

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