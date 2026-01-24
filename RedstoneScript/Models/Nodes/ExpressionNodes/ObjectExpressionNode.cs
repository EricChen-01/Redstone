using System.Text;

namespace RedstoneScript.AST;

public class ObjectExpressionNode : ExpressionNode
{
    public List<PropertyExpressionNode> Properties { get; }
    

    public ObjectExpressionNode(List<PropertyExpressionNode> properties) : base(NodeType.ObjectLiteral)
    {
        Properties = properties;
    }

    public override string ToString(int indent)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{AstPrinter.Indent(indent)}ObjectLiteral");

        foreach (var prop in Properties)
        {
            sb.AppendLine(prop.ToString(indent + 1));
        }

        return sb.ToString().TrimEnd();
    }

}