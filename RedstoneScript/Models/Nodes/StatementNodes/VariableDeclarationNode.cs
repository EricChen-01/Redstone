using System.Text;

namespace RedstoneScript.AST;

public class VariableDelarationNode : StatementNode
{
    public string Identifier { get; }

    public bool IsConstant { get; }

    public ExpressionNode? Value { get; }

    public VariableDelarationNode(string identifier, ExpressionNode? value, bool isConstant = false) : base(NodeType.VariableDeclaration)
    {
        Identifier = identifier;
        IsConstant = isConstant;
        Value = value;
    }

    public override string ToString(int indent)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{AstPrinter.Indent(indent)}VarDeclaration {Identifier}");

        if (Value != null)
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Value:");
            sb.AppendLine(Value.ToString(indent + 2));
        }

        return sb.ToString().TrimEnd();
    }

}

