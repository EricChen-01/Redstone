namespace RedstoneScript.Interpreter;

public class ReturnSignal
{
    public RuntimeValue Value { get; }

    public ReturnSignal(RuntimeValue value)
    {
        Value = value;
    }
}
