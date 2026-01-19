namespace RedstoneScript.Interpreter;

public sealed class VariableEntry
{
    public RuntimeValue Value { get; set; }
    public bool IsConstant { get; }

    public VariableEntry(RuntimeValue runtimeValue, bool isConstant)
    {
        Value = runtimeValue;
        IsConstant = isConstant;
    }
}
