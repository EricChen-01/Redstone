namespace RedstoneScript.Interpreter;

public class NativeFunctions
{
    public static RuntimeValue Print(List<RuntimeValue> arguments, Scope scope)
    {
        Console.ForegroundColor = ConsoleColor.Green; 
        foreach (var arg in arguments)
        {
            Console.Write(arg.ToString() + " ");
        }
        Console.WriteLine();
        Console.ResetColor();
        return new NullValue();
    }
}
