namespace RedstoneScript.AST;

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