namespace RedstoneScript.AST;

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