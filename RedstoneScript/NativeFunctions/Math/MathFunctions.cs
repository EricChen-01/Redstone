namespace RedstoneScript.NativeFunctions.Math;

using System;
using RedstoneScript.Interpreter;

public class MathFunctions
{
    public static RuntimeValue AbsoluteValue(RuntimeValue argument, Scope scope)
    {

        if (argument is not NumberValue number)
        {
            throw new Exception("abs expects a number");   
        }

        var result = number.Value < 0
            ? -number.Value
            : number.Value;

        return new NumberValue(result);
    }
}
