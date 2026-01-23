namespace RedstoneScript.NativeFunctions.Math;

using System;
using RedstoneScript.Interpreter;

public class MathFunctions
{
    public static void ImportMath(Scope scope)
    {
        scope.DefineVariable("abs", new NativeFunctionValue(MathFunctions.AbsoluteValue), true);
        scope.DefineVariable("square", new NativeFunctionValue(MathFunctions.Square), true);
        scope.DefineVariable("cube", new NativeFunctionValue(MathFunctions.Cube), true);
        scope.DefineVariable("max", new NativeFunctionValue(MathFunctions.Max), true);
        scope.DefineVariable("min", new NativeFunctionValue(MathFunctions.Min), true);
        scope.DefineVariable("negative", new NativeFunctionValue(MathFunctions.Negative), true);
    }
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

    public static RuntimeValue Square(List<RuntimeValue> arguments, Scope scope)
    {
        if (arguments.Count != 1)
        {
            throw new InvalidOperationException("Redstone Interpreter: square expects exactly 1 argument");
        }

        if (arguments[0] is not NumberValue number)
        {
            throw new InvalidOperationException("Redstone Interpreter: square expects a number");
        }

        return new NumberValue(number.Value * number.Value);
    }

    public static RuntimeValue Cube(List<RuntimeValue> arguments, Scope scope)
    {
        if (arguments.Count != 1)
        {
            throw new InvalidOperationException("Redstone Interpreter: cube expects exactly 1 argument");
        }

        if (arguments[0] is not NumberValue number)
        {
            throw new InvalidOperationException("Redstone Interpreter: cube expects a number");
        }

        return new NumberValue(number.Value * number.Value * number.Value);
    }

    public static RuntimeValue Max(List<RuntimeValue> arguments, Scope scope)
    {
        if (arguments.Count != 2)
        {
            throw new InvalidOperationException("Redstone Interpreter: max expects exactly 2 arguments");
        }

        if (arguments[0] is not NumberValue a || arguments[1] is not NumberValue b)
        {
            throw new InvalidOperationException("Redstone Interpreter: max expects numbers");
        }

        return new NumberValue(Math.Max(a.Value, b.Value));
    }

    public static RuntimeValue Min(List<RuntimeValue> arguments, Scope scope)
    {
        if (arguments.Count != 2)
        {
            throw new InvalidOperationException("Redstone Interpreter: min expects exactly 2 arguments");
        }

        if (arguments[0] is not NumberValue a || arguments[1] is not NumberValue b)
        {
            throw new InvalidOperationException("Redstone Interpreter: min expects numbers");
        }

        return new NumberValue(Math.Min(a.Value, b.Value));
    }

    public static RuntimeValue Negative(RuntimeValue number, Scope scope)
    {
        if (number is not NumberValue numberVal)
        {
            throw new InvalidOperationException("Redstone Interpreter: min expects numbers");
        }

        return new NumberValue(-numberVal.Value);
    }
}
