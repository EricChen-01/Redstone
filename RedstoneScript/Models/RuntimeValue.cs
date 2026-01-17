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

public class AirValue : RuntimeValue
{
    public string value = "air";
    public AirValue() : base(RuntimeValueType.Air)
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