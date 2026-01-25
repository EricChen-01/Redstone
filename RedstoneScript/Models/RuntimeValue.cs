using RedstoneScript.AST;

namespace RedstoneScript.Interpreter;

public interface IRuntimeValue
{
    RuntimeValueType Type { get; }
}

public abstract class RuntimeValue : IRuntimeValue
{
    public RuntimeValueType Type { get; }

    public RuntimeValue(RuntimeValueType type)
    {
        Type = type;
    }

    public abstract override string ToString();
}

public class NullValue : RuntimeValue
{
    public string value = "air";
    public NullValue() : base(RuntimeValueType.Null)
    {
    }

    public override string ToString()
    {
        return $"{value}";
    }
}

public class NumberValue : RuntimeValue
{
    public double Value { get; }
    public NumberValue(double value) : base(RuntimeValueType.Number)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"{Value}";
    }
}

public class StringValue : RuntimeValue
{
    public string Value { get; }
    public StringValue(string value) : base(RuntimeValueType.String)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}

public class BooleanValue : RuntimeValue
{
    public bool Value { get; }
    public BooleanValue(bool value) : base(RuntimeValueType.Boolean)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value ? "on" : "off";
    }
}

public class ObjectValue : RuntimeValue
{
    public Dictionary<string, RuntimeValue> Properties { get; }
    public ObjectValue(Dictionary<string, RuntimeValue> properties) : base(RuntimeValueType.Object)
    {
        Properties = properties;
    }

    public override string ToString()
    {
        if (Properties.Count == 0)
            return "{}";

        var parts = Properties.Select(kv =>
        {
            var key = kv.Key;
            var value = kv.Value?.ToString() ?? "null"; // handle null values
            return $"{key}: {value}";
        });

        return "{ " + string.Join(", ", parts) + " }";
    }
}

public class NativeFunctionValue : RuntimeValue
{
    public Func<List<RuntimeValue>, Scope, RuntimeValue> FunctionCall { get; }

    public NativeFunctionValue(Func<List<RuntimeValue>, Scope, RuntimeValue> function) 
        : base(RuntimeValueType.NativeFunction)
    {
        FunctionCall = function;
    }

    public NativeFunctionValue(Func<RuntimeValue, Scope, RuntimeValue> function) 
        : base(RuntimeValueType.NativeFunction)
    {
        // Wrap the single argument function into the list-based signature
        FunctionCall = (args, scope) =>
        {
            if (args.Count != 1)
                throw new Exception("This function expects exactly 1 argument");
            return function(args[0], scope);
        };
    }

    public override string ToString()
    {
        return "<native function>";
    }
}

public class FunctionValue : RuntimeValue
{
    public string Name { get; }
    public List<string> Parameters { get; }
    public List<INode> Body { get; }
    public Scope DeclarationScope { get; }

    public FunctionValue(string name, List<string> parameters, List<INode> body, Scope scope) 
        : base(RuntimeValueType.Function)
    {
        Name = name;
        Parameters = parameters;
        Body = body;
        DeclarationScope = scope;
    }

    public override string ToString()
    {
        return "<function declaration>";
    }
}

public class VoidValue : RuntimeValue
{
    public VoidValue() : base(RuntimeValueType.Void)
    {
    }

    public override string ToString()
    {
        return string.Empty;
    }
}
