namespace RedstoneScript.Interpreter.Signals;

public class ReturnSignal : Exception
{   
    public RuntimeValue Value { get; }
    public ReturnSignal(RuntimeValue value)
    {
        Value = value;
    }
}
