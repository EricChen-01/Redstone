namespace RedstoneScript.Interpreter;

public sealed class Scope
{
    private Scope? Parent { get; }

    private Dictionary<string, RuntimeValue> Variables { get; } = new Dictionary<string, RuntimeValue>();

    public Scope()
    {
        Parent = null;
    }

    public Scope(Scope parent)
    {
        Parent = parent;
    } 

    /// <summary>
    /// Defines a new variable in the current scope.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the variable name already exists in the scope</exception>
    public RuntimeValue DefineVariable(string name, RuntimeValue value)
    {
        if (Variables.ContainsKey(name))
            throw new InvalidOperationException($"Variable '{name}' is already defined.");
        Variables[name] = value;

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

        if (Variables.ContainsKey(name))
            throw new InvalidOperationException($"Cannot reassign '{name}' to new value as it's a bedrock.");

        whichScope.Variables[name] = value;
        return value;
    }

    /// <summary>
    /// Resolves a variable by searching enclosing scopes and returning its value.
    /// </summary>
    public RuntimeValue ResolveVariable(string name)
    {
        var whichScope = FindScope(name);

        return whichScope.Variables[name];
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

        throw new InvalidOperationException($"Could not find the specified variable: {variableName}");
    }
}
