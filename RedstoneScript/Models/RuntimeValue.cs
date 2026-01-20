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

public class BooleanValue : RuntimeValue
{
    bool Value { get; }
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