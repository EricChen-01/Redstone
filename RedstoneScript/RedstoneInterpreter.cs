using RedstoneScript.AST;
using RedstoneScript.Lexer;

namespace RedstoneScript.Interpreter;

public class RedstoneInterpreter
{

    public static RuntimeValue EvaluateProgram(ProgramNode programNode, Scope scope)
    {
        RuntimeValue lastRuntimeValue = new NullValue();

        foreach (INode node in programNode.Nodes)
        {
            lastRuntimeValue = Evaluate(node, scope);
        }

        return lastRuntimeValue;
    }

    /// <summary>
    /// Evaluates the current Node given the current scope.
    /// </summary>
    private static RuntimeValue Evaluate(INode node, Scope scope)
    {
        return node.Type switch
        {
            NodeType.Program => Evaluate<ProgramNode>(node, scope, EvaluateProgram),
            NodeType.Identifier => Evaluate<IdentifierExpressionNode>(node, scope, EvaluateIdentifierExpression),
            NodeType.NumericLiteral => Evaluate(node, (NumericExpressionNode node) => new NumberValue(node.Value)),
            NodeType.BinaryExpression => Evaluate<BinaryExpressionNode>(node, scope, EvaluateBinaryExpression),
            NodeType.NullLiteral => new NullValue(),
            NodeType.BooleanLiteral => Evaluate(node, (BooleanExpressionNode node) => new BooleanValue(node.Value)),
            NodeType.VariableDeclaration => Evaluate<VariableDelarationNode>(node, scope, EvaluateVariableDeclarationStatement),
            NodeType.AssignmentExpression => Evaluate<AssignmentExpressionNode>(node, scope, EvaluateAssignmentExpression),
            NodeType.ObjectLiteral => Evaluate<ObjectExpressionNode>(node, scope, EvaluateObjectExpression),
            _ => throw new InvalidOperationException($"RedStone Interpreter: Unexpected Node during execution stage: {node.Type}. It could mean that it's not supported yet.\n\t{node}"),
        };
    }
 
    private static RuntimeValue EvaluateIdentifierExpression(IdentifierExpressionNode identifierExpressionNode, Scope scope)
    {
        return scope.ResolveVariable(identifierExpressionNode.Name);
    }

    private static RuntimeValue EvaluateBinaryExpression(BinaryExpressionNode binaryExpressionNode, Scope scope)
    {
        var left = Evaluate(binaryExpressionNode.Left, scope);
        var right = Evaluate(binaryExpressionNode.Right, scope);

        if (left is NumberValue l && right is NumberValue r)
        {
            return EvaluateNumericExpression(l, r, binaryExpressionNode.Operator);
        }

        throw new NotSupportedException(
            $"Operator '{binaryExpressionNode.Operator}' not supported for types " +
            $"{left.GetType().Name} and {right.GetType().Name}"
        );
    }

    private static NumberValue EvaluateNumericExpression(NumberValue left, NumberValue right, string operatorSign)
    {
        switch (operatorSign)
        {
            case OperatorType.ADDITION:
                return new NumberValue(left.Value + right.Value);
            case OperatorType.SUBTRACTION:
                return new NumberValue(left.Value - right.Value);
            case OperatorType.MULTIPLICATION:
                return new NumberValue(left.Value * right.Value);
            case OperatorType.DIVISION:
                if (right.Value == 0)
                    throw new DivideByZeroException("Redstone Interpreter: Cannot divide by zero.");
                return new NumberValue(left.Value / right.Value);
            case OperatorType.MODULUS:
                return new NumberValue(left.Value % right.Value);
            default: 
                throw new InvalidOperationException($"Redstone Interpreter: Invalid Numeric Operation. {left.Value} {operatorSign} {right.Value}");
        }
    }   

    private static RuntimeValue EvaluateVariableDeclarationStatement(VariableDelarationNode variableDelarationNode, Scope scope)
    {
        var name = variableDelarationNode.Identifier;
        var isConstant = variableDelarationNode.IsConstant;

        var finalValue = variableDelarationNode.Value != null ? Evaluate(variableDelarationNode.Value, scope) : new NullValue();

        return scope.DefineVariable(name, finalValue, isConstant);
    }

    private static RuntimeValue EvaluateAssignmentExpression(AssignmentExpressionNode assignmentExpressionNode, Scope scope)
    {
        switch (assignmentExpressionNode.LeftExpression)
        {
            case IdentifierExpressionNode identifier:
                {
                    var value = Evaluate(assignmentExpressionNode.RightExpression, scope);
                    return scope.AssignVariable(identifier.Name, value);
                }
            default:
                throw new NotSupportedException("Redstone Interpreter: Unexpected assignmentExpressionNode. Currently only supporting Identifiers.");
        }
    }

    private static RuntimeValue EvaluateObjectExpression(ObjectExpressionNode objectExpressionNode, Scope scope)
    {
        var newObject = new ObjectValue(new Dictionary<string, RuntimeValue>());

        foreach (PropertyExpressionNode property in objectExpressionNode.Properties)
        {
            var name = property.Name;
            var value = property.Value;
            var evaluatedValue = value != null ? Evaluate(value, scope) : scope.ResolveVariable(name); // handles { x } => { x : x } by searching x in the scope.
            newObject.Properties[name] = evaluatedValue;
        }

        return newObject;
    }

    /// <summary>
    /// Evaluate with a scope
    /// </summary>
    private static RuntimeValue Evaluate<TNode>(
        INode node,
        Scope scope,
        Func<TNode, Scope, RuntimeValue> evaluator
    )
        where TNode : class, INode
    {
        if (node is TNode typedNode)
            return evaluator(typedNode, scope);

        throw new InvalidOperationException($"Redstone Interpreter: Unexpected node type. Expected {typeof(TNode).Name}, got {node.GetType().Name}"
        );
    }

    /// <summary>
    /// Evaluate without a scope
    /// </summary>
    private static RuntimeValue Evaluate<TNode>(
        INode node,
        Func<TNode, RuntimeValue> evaluator
    )
        where TNode : class, INode
    {
        if (node is TNode typedNode)
            return evaluator(typedNode);

        throw new InvalidOperationException($"Redstone Interpreter: Unexpected node type. Expected {typeof(TNode).Name}, got {node.GetType().Name}"
        );
    }
}
