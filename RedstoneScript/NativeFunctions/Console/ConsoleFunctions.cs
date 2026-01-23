using RedstoneScript.Interpreter;

namespace RedstoneScript.NativeFunctions.Console;

using System;

public class ConsoleFunctions
{
    public static RuntimeValue Print(List<RuntimeValue> arguments, Scope scope)
    {
        foreach (var arg in arguments)
        {
            Console.Write(arg.ToString() + "");
        }
        Console.WriteLine();
        return new VoidValue();
    }
}
