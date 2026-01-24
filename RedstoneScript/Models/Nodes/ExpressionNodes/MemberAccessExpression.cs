namespace RedstoneScript.AST;

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