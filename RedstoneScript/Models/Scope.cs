using RedstoneScript.NativeFunctions.Console;
using RedstoneScript.NativeFunctions.Math;

namespace RedstoneScript.Interpreter;

public sealed class Scope
{
    private Scope? Parent { get; }

    private Dictionary<string, VariableEntry> Variables { get; } = new Dictionary<string, VariableEntry>();

    public Scope()
    {
        Parent = null;
        AddNativeFunctions();
    }

    public Scope(Scope parent)
    {
        Parent = parent;
    } 

    private void AddNativeFunctions()
    {
        Variables.Add("chat", new VariableEntry(new NativeFunctionValue(ConsoleFunctions.Print), true));
        MathFunctions.ImportMath(this);
    }

#region helpers
    /// <summary>
    /// Defines a new variable in the current scope.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the variable name already exists in the scope</exception>
    public RuntimeValue DefineVariable(string name, RuntimeValue value, bool isConstant)
    {
        if (Variables.ContainsKey(name))
            throw new InvalidOperationException($"Redstone Interpreter: Variable '{name}' is already defined.");
        Variables[name] = new VariableEntry(value, isConstant);

        return value;
    }

    /// <summary>
    /// Assigns an variable to a new value.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public RuntimeValue AssignVariable(string name, RuntimeValue value)
    {
        var whichScope = FindScope(name);
        var variables = whichScope.Variables;
        
        // determine if it's a constant. if so throw error.
        if (variables[name].IsConstant)
            throw new InvalidOperationException($"Redstone Interpreter: Cannot reassign '{name}' to new value as it's a bedrock.");

        whichScope.Variables[name].Value = value;
        return value;
    }

    /// <summary>
    /// Resolves a variable by searching enclosing scopes and returning its value.
    /// </summary>
    public RuntimeValue ResolveVariable(string name)
    {
        var whichScope = FindScope(name);

        return whichScope.Variables[name].Value;
    }

    /// <summary>
    /// Finds the scope a variable belongs to.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the variable does no belong to any scope.</exception>
    public Scope FindScope(string variableName)
    {
        if (Variables.ContainsKey(variableName))
            return this;

        if (Parent != null)
        {
            return Parent.FindScope(variableName);
        }

        throw new InvalidOperationException($"Redstone Interpreter: Could not find the specified variable: {variableName}");
    }
#endregion
}
