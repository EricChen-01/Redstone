using RedstoneScript.AST;

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
            _ => throw new InvalidOperationException($"Unexpected Node: {node.Type}. It could mean that it's not supported yet."),
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
                    throw new DivideByZeroException();
                return new NumberValue(left.Value / right.Value);
            case OperatorType.MODULUS:
                return new NumberValue(left.Value % right.Value);
            default: 
                throw new InvalidOperationException($"Invalid Numeric Operation. {left.Value} {operatorSign} {right.Value}");
        }
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

        throw new InvalidOperationException(
            $"Unexpected node type. Expected {typeof(TNode).Name}, got {node.GetType().Name}"
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

        throw new InvalidOperationException(
            $"Unexpected node type. Expected {typeof(TNode).Name}, got {node.GetType().Name}"
        );
    }
}
