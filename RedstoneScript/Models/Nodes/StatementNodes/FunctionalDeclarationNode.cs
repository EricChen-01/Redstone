using System.Text;

namespace RedstoneScript.AST;

public class FunctionDelarationNode : StatementNode
{
    public string Name { get; }

    public List<string> Parameters { get; }

    public BlockStatementNode Body { get; }

    public FunctionDelarationNode(string name, List<string> parameters, BlockStatementNode body) : base(NodeType.FunctionDeclaration)
    {
        Name = name;
        Parameters = parameters;
        Body = body;
    }

    public override string ToString(int indent)
    {
        var sb = new StringBuilder();

        // Function declaration header
        sb.AppendLine($"{AstPrinter.Indent(indent)}FunctionDeclaration {Name}");

        // Parameters
        sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Parameters:");
        if (Parameters != null && Parameters.Count > 0)
        {
            foreach (var param in Parameters)
            {
                sb.AppendLine($"{AstPrinter.Indent(indent + 2)}{param}");
            }
        }
        else
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 2)}(none)");
        }

        // Body
        sb.AppendLine($"{AstPrinter.Indent(indent + 1)}Body:");
        if (Body != null)
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 2)}(function body)");
        }
        else
        {
            sb.AppendLine($"{AstPrinter.Indent(indent + 2)}(empty)");
        }

        return sb.ToString().TrimEnd();
    }

}

