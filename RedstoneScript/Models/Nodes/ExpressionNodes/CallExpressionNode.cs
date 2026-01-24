using System.Text;

namespace RedstoneScript.AST;

public class CallExpressionNode : ExpressionNode
{
    public ExpressionNode RightCallExpression { get; }

    public List<ExpressionNode> Arguments { get; }

    public CallExpressionNode(ExpressionNode rightCallExpression, List<ExpressionNode> arguments) : base(NodeType.CallExpression)
    {
        RightCallExpression = rightCallExpression;
        Arguments = arguments;
    }

    public override string ToString(int indent)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{AstPrinter.Indent(indent)}CallExpression");
        sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Callee:");
        sb.AppendLine($"{AstPrinter.Indent(indent + 2)}{RightCallExpression}");

        if (Arguments.Count > 0)
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Arguments:");
            foreach (var arg in Arguments)
            {
                sb.AppendLine($"{AstPrinter.Indent(indent + 2)}{arg}");
            }
        }

        return sb.ToString().TrimEnd();
    }


}