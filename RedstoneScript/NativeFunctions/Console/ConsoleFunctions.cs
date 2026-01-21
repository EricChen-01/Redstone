using RedstoneScript.Interpreter;

namespace RedstoneScript.NativeFunctions.Console;

using System;

public class ConsoleFunctions
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
