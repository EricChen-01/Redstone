using System.Diagnostics.Tracing;
using System.Reflection.Emit;
using RedstoneScript.AST;

namespace RedstoneScript.Interpreter;

public class RedstoneInterpreter
{
    public static RuntimeValue EvaluateProgram(ProgramNode programNode)
    {
        RuntimeValue lastRuntimeValue = new AirValue();

        foreach (INode node in programNode.Nodes)
        {
            lastRuntimeValue = Evaluate(node);
        }

        return lastRuntimeValue;
    }

    private static RuntimeValue Evaluate(INode node)
    {
        return node.Type switch
        {
            NodeType.Program => Evaluate<ProgramNode>(node, EvaluateProgram),
            NodeType.Identifier => throw new NotImplementedException(),
            NodeType.NumericLiteral => Evaluate(node, (NumericExpressionNode node) => new NumberValue(node.Value)),
            NodeType.BinaryExpression => Evaluate<BinaryExpressionNode>(node, EvaluateBinaryExpression),
            NodeType.NullLiteral => new AirValue(),
            _ => throw new InvalidOperationException($"Unexpected Node: {node.Type}. It could mean that it's not supported yet."),
        };
    }
 
    private static RuntimeValue EvaluateBinaryExpression(BinaryExpressionNode binaryExpressionNode)
    {
        var left = Evaluate(binaryExpressionNode.Left);
        var right = Evaluate(binaryExpressionNode.Right);

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
