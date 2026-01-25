namespace RedstoneScript.AST;

public class ReturnStatementNode : StatementNode
{
    public ExpressionNode? Value { get; }

    public ReturnStatementNode(ExpressionNode? value)
        : base(NodeType.ReturnStatement)
    {
        Value = value;
    }

    public override string ToString(int indent)
    {
        return Value == null
            ? $"{AstPrinter.Indent(indent)}Return"
            : $"${AstPrinter.Indent(indent)}Return {Value}";
    }
}