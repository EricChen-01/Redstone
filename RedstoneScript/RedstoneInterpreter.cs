using RedstoneScript.AST;
using RedstoneScript.Interpreter.Signals;

namespace RedstoneScript.Interpreter;

public class RedstoneInterpreter
{
    private Stack<NodeType> loopStack = new Stack<NodeType>();
    public RedstoneInterpreter()
    {}

    public RuntimeValue EvaluateProgram(ProgramNode programNode, Scope scope)
    {
        RuntimeValue lastRuntimeValue = new NullValue();

        foreach (INode node in programNode.Nodes)
        {
            lastRuntimeValue = Evaluate(node, scope);
        }

        return lastRuntimeValue;
    }

    public bool ValidateProgram(ProgramNode programNode)
    {
        for (int i = 0; i < programNode.Nodes.Count; i++)
        {
            var node = programNode.Nodes[i];

            switch (node)
            {
                case VariableDelarationNode:
                case FunctionDelarationNode:
                case CallExpressionNode:
                case IfStatementNode:
                case WhileSatementNode:
                case AssignmentExpressionNode:
                    break;
                case ExpressionNode:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Redstone Validation error: standalone expression not allowed at top level (statement {i + 1})");
                    Console.ResetColor();
                    return false;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Validation error: unknown node type at statement {i + 1}");
                    Console.ResetColor();
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Evaluates the current Node given the current scope.
    /// </summary>
    private RuntimeValue Evaluate(INode node, Scope scope)
    {
        return node.Type switch
        {
            NodeType.Program => Evaluate<ProgramNode>(node, scope, EvaluateProgram),
            NodeType.Identifier => Evaluate<IdentifierExpressionNode>(node, scope, EvaluateIdentifierExpression),
            NodeType.NumericLiteral => Evaluate(node, (NumericExpressionNode node) => new NumberValue(node.Value)),
            NodeType.StringLiteral => Evaluate(node, (StringExpressionNode node) => new StringValue(node.Value)),
            NodeType.BinaryExpression => Evaluate<BinaryExpressionNode>(node, scope, EvaluateBinaryExpression),
            NodeType.NullLiteral => new NullValue(),
            NodeType.BooleanLiteral => Evaluate(node, (BooleanExpressionNode node) => new BooleanValue(node.Value)),
            NodeType.VariableDeclaration => Evaluate<VariableDelarationNode>(node, scope, EvaluateVariableDeclarationStatement),
            NodeType.AssignmentExpression => Evaluate<AssignmentExpressionNode>(node, scope, EvaluateAssignmentExpression),
            NodeType.ObjectLiteral => Evaluate<ObjectExpressionNode>(node, scope, EvaluateObjectExpression),
            NodeType.CallExpression => Evaluate<CallExpressionNode>(node, scope, EvaluateCallExpression),
            NodeType.MemberAccessExpression => Evaluate<MemberAccessExpression>(node, scope, EvaluateMemberAccessExpression),
            NodeType.FunctionDeclaration => Evaluate<FunctionDelarationNode>(node, scope, EvaluateFunctionDeclarationStatement),
            NodeType.IfStatement => Evaluate<IfStatementNode>(node, scope, EvaluateIfStatement),
            NodeType.WhileStatement => Evaluate<WhileSatementNode>(node, scope, EvaluateWhileStatement),
            NodeType.BreakStatement => loopStack.Any(type => type == NodeType.WhileStatement || type == NodeType.ForStatement) ? throw new BreakSignal() : throw new InvalidOperationException($"Redstone Interpreter: 'cut' used outside of a loop"),
            _ => throw new InvalidOperationException($"Redstone Interpreter: Unexpected Node during execution stage: {node.Type}. It could mean that it's not supported yet.\n{node}"),
        };
    }
 
    private RuntimeValue EvaluateIdentifierExpression(IdentifierExpressionNode identifierExpressionNode, Scope scope)
    {
        return scope.ResolveVariable(identifierExpressionNode.Name);
    }

    private RuntimeValue EvaluateBinaryExpression(BinaryExpressionNode binaryExpressionNode, Scope scope)
    {
        var left = Evaluate(binaryExpressionNode.Left, scope);
        var right = Evaluate(binaryExpressionNode.Right, scope);

        if (left is NumberValue l && right is NumberValue r && "+-*/%".Contains(binaryExpressionNode.Operator))
        {
            return EvaluateNumericExpression(l, r, binaryExpressionNode.Operator);
        }

        switch (binaryExpressionNode.Operator)
        {
            // Comparison
            case "==":
                return EvaluateEqualComparisonExpression(left, right);
            case "!=":
                return EvaluateNotEqualComparisonExpression(left, right);
            case "<":
                return EvaluateLessThanComparisonExpression(left, right);
            case "<=":
                return EvaluateLessThanEqualComparisonExpression(left, right);
            case ">":
                return EvaluateGreaterThanComparisonExpression(left,right);
            case ">=":
                return EvaluateGreaterThanEqualComparisonExpression(left, right);
        }

        throw new NotSupportedException(
            $"Operator '{binaryExpressionNode.Operator}' not supported for types " +
            $"{left.GetType().Name} and {right.GetType().Name}"
        );
    }

    private BooleanValue EvaluateEqualComparisonExpression(RuntimeValue left, RuntimeValue right)
    {
        if (left is NumberValue lNum && right is NumberValue rNum)
        {
            return new BooleanValue(lNum.Value == rNum.Value);   
        }
        if (left is BooleanValue lBool && right is BooleanValue rBool)
        {
            return new BooleanValue(lBool.Value == rBool.Value);   
        }
        if (left is StringValue lStr && right is StringValue rStr)
        {
            return new BooleanValue(lStr.Value == rStr.Value);   
        }
        
        return new BooleanValue(false);
    }

    private BooleanValue EvaluateNotEqualComparisonExpression(RuntimeValue left, RuntimeValue right)
    {
        if (left is NumberValue lNum && right is NumberValue rNum)
        {
            return new BooleanValue(lNum.Value != rNum.Value);   
        }
        if (left is BooleanValue lBool && right is BooleanValue rBool)
        {
            return new BooleanValue(lBool.Value != rBool.Value);   
        }
        if (left is StringValue lStr && right is StringValue rStr)
        {
            return new BooleanValue(lStr.Value != rStr.Value);   
        }
        
        return new BooleanValue(true);
    }

    private BooleanValue EvaluateLessThanComparisonExpression(RuntimeValue left, RuntimeValue right)
    {
        if (left is NumberValue l && right is NumberValue r)
            return new BooleanValue(l.Value < r.Value);
        throw new InvalidOperationException($"Redstone Interpreter: Cannot compare {left.Type} < {right.Type}");
    }

    private BooleanValue EvaluateLessThanEqualComparisonExpression(RuntimeValue left, RuntimeValue right)
    {
        if (left is NumberValue l && right is NumberValue r)
            return new BooleanValue(l.Value <= r.Value);
        throw new InvalidOperationException($"Redstone Interpreter: Cannot compare {left.Type} <= {right.Type}");
    }

    private BooleanValue EvaluateGreaterThanComparisonExpression(RuntimeValue left, RuntimeValue right)
    {
        if (left is NumberValue l && right is NumberValue r)
            return new BooleanValue(l.Value > r.Value);
        throw new InvalidOperationException($"Redstone Interpreter: Cannot compare {left.Type} > {right.Type}");
    }

    private BooleanValue EvaluateGreaterThanEqualComparisonExpression(RuntimeValue left, RuntimeValue right)
    {
        if (left is NumberValue l && right is NumberValue r)
            return new BooleanValue(l.Value >= r.Value);
        throw new InvalidOperationException($"Redstone Interpreter: Cannot compare {left.Type} >= {right.Type}");
    }

    private NumberValue EvaluateNumericExpression(NumberValue left, NumberValue right, string operatorSign)
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

    private RuntimeValue EvaluateVariableDeclarationStatement(VariableDelarationNode variableDelarationNode, Scope scope)
    {
        var name = variableDelarationNode.Identifier;
        var isConstant = variableDelarationNode.IsConstant;

        var finalValue = variableDelarationNode.Value != null ? Evaluate(variableDelarationNode.Value, scope) : new NullValue();

        return scope.DefineVariable(name, finalValue, isConstant);
    }

    private RuntimeValue EvaluateAssignmentExpression(AssignmentExpressionNode assignmentExpressionNode, Scope scope)
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

    private RuntimeValue EvaluateObjectExpression(ObjectExpressionNode objectExpressionNode, Scope scope)
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

    private RuntimeValue EvaluateCallExpression(CallExpressionNode callExpressionNode, Scope scope)
    {
        List<RuntimeValue> arguments = callExpressionNode.Arguments
            .Select(arg => Evaluate(arg, scope))
            .ToList();
        
        var functionValue = Evaluate(callExpressionNode.RightCallExpression, scope);

        if (functionValue.Type != RuntimeValueType.NativeFunction && functionValue.Type != RuntimeValueType.Function)
        {
            throw new InvalidOperationException($"Redstone Interpreter: {functionValue} is not a function.");
        }

        if (functionValue is NativeFunctionValue nativeFunction)
        {
            return nativeFunction.FunctionCall(arguments, scope);
        }
        else if (functionValue is FunctionValue function)
        {
            var functionScope = new Scope(function.DeclarationScope);
            var parameters = function.Parameters;
            if (parameters.Count > arguments.Count)
            {
                var missing = parameters.Skip(arguments.Count).ToList();
                throw new InvalidOperationException($"Redstone Interpreter: Required arguments not passed. Missing {string.Join(",", missing)}");
            }
            for(int i = 0 ; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                functionScope.DefineVariable(parameter, arguments[i], false);
            }
            
            return EvaluateBlockStatement(function.Body, functionScope);
        }

        throw new InvalidOperationException($"Redstone Interpreter: Could not determine function to run. got: {functionValue.Type}");
    }

    private RuntimeValue EvaluateMemberAccessExpression(MemberAccessExpression memberAccessExpression, Scope scope)
    {
        var objectValue = Evaluate(memberAccessExpression.Object, scope); // evaluate the memberAccessExpression first.
        if (objectValue is not ObjectValue evaluatedObject)
        {
            throw new InvalidOperationException($"Redstone Interpreter: Cannot access property of non-object. Got: {objectValue}");
        }

        // property must be an identifier
        if (memberAccessExpression.Property is not IdentifierExpressionNode identifierExpressionNode)
        {
            throw new InvalidOperationException($"Redstone Interpreter: Member access property must be an identifier");
        }

        // look up the property in the object
        if (!evaluatedObject.Properties.TryGetValue(identifierExpressionNode.Name, out var value))
        {
            throw new InvalidOperationException($"Redstone Interpreter: Property '{identifierExpressionNode.Name}' does not exist on object");
        }
            
        return value;
    }

    private RuntimeValue EvaluateFunctionDeclarationStatement(FunctionDelarationNode functionDelarationNode, Scope scope)
    {
        var name = functionDelarationNode.Name;
        var parameters = functionDelarationNode.Parameters;
        var body = functionDelarationNode.Body.Statements;

        var newFunction = new FunctionValue(name, parameters, body, scope);
        return scope.DefineVariable(name, newFunction, true);
    }

    private RuntimeValue EvaluateBlockStatement(List<INode> statements, Scope scope)
    {
        foreach (var statement in statements)
        {
            Evaluate(statement, scope);
        }
        return new VoidValue();
    }

    private RuntimeValue EvaluateIfStatement(IfStatementNode node, Scope scope)
    {
        var conditionValue = Evaluate(node.Condition, scope);

        if (conditionValue is BooleanValue booleanValue)
        {
            if (booleanValue.Value)
            {
                var ifStatementBody = node.Body.Statements;
                var ifSatementScope = new Scope(scope);
                EvaluateBlockStatement(ifStatementBody, ifSatementScope);   
            } else
            {   
                if (node.Else != null)
                {
                    var elseIfStatementBody = node.Else;
                    var elseIfSatementScope = new Scope(scope);
                    if(elseIfStatementBody is IfStatementNode ifStatementNode)
                    {
                        EvaluateIfStatement(ifStatementNode, elseIfSatementScope);   
                    }else if (elseIfStatementBody is BlockStatementNode elseStatementNode)
                    {
                        EvaluateBlockStatement(elseStatementNode.Statements, elseIfSatementScope);   
                    }
                } 
            }
        }
        else
        {
            throw new InvalidOperationException($"Redstone Interpreter: If statement expected a boolean value, but got {conditionValue.Type}.\nNode: {node}");
        }

        return new VoidValue();
    }

    private RuntimeValue EvaluateWhileStatement(WhileSatementNode node, Scope scope)
    {
        loopStack.Push(NodeType.WhileStatement);
        var IsTruthy = () => {
            var value = Evaluate(node.Condition, scope);
            if (value is not BooleanValue b)
            {
                throw new InvalidOperationException($"Redstone Interpreter: While statement expected a boolean value, but got {value.Type}.\nNode: {node}");   
            }
            return b.Value;
        };

        try
        {
            while (IsTruthy())
            {
                try
                {
                    var whileStatementBody = node.Body.Statements;
                    var whileSatementScope = new Scope(scope);
                    EvaluateBlockStatement(whileStatementBody, whileSatementScope);
                }
                catch (BreakSignal)
                {
                    break;
                }
                catch (ContinueSignal)
                {
                    continue;
                }
            }
        }
        finally
        {
            loopStack.Pop();   
        }
        return new VoidValue();
    }

#region Helpers
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

    /// <summary>
    /// Evaluate with scope and return nothing
    /// </summary>
    private static void Evaluate<TNode>(
        INode node,
        Scope scope,
        Action<TNode, Scope> evaluator
    )
        where TNode : class, INode
    {
        if (node is TNode typedNode)
        {
            evaluator(typedNode, scope);
            return;
        }

        throw new InvalidOperationException(
            $"Redstone Interpreter: Unexpected node type. Expected {typeof(TNode).Name}, got {node.GetType().Name}"
        );
    }
#endregion
}
