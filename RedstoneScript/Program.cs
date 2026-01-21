using RedstoneScript.AST;
using RedstoneScript.Interpreter;
using RedstoneScript.Lexer;
using RedstoneScript.AST.Parser;


// Bugs:
// No negative numbers
// No Comments

static void ShowSplash()
{
    Console.ForegroundColor = ConsoleColor.Red;

    Console.WriteLine(@"
██████╗ ███████╗██████╗ ███████╗████████╗ ██████╗ ███╗   ██╗███████╗
██╔══██╗██╔════╝██╔══██╗██╔════╝╚══██╔══╝██╔═══██╗████╗  ██║██╔════╝
██████╔╝█████╗  ██║  ██║███████╗   ██║   ██║   ██║██╔██╗ ██║█████╗  
██╔══██╗██╔══╝  ██║  ██║╚════██║   ██║   ██║   ██║██║╚██╗██║██╔══╝  
██║  ██║███████╗██████╔╝███████║   ██║   ╚██████╔╝██║ ╚████║███████╗
╚═╝  ╚═╝╚══════╝╚═════╝ ╚══════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═══╝╚══════╝
");

    Console.ResetColor();

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("Welcome to Redstone CLI v0.0.1!");
    Console.WriteLine("Type 'exit' to quit.\n");
    Console.ResetColor();
    Console.WriteLine();
}

static string? GetFile(string input)
{
    // Extract the file path after "run"
    var parts = input.Trim().Split(' ', 2); // split into ["run", "filepath"]
    if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Usage: run <file_path>");
        Console.ResetColor();
        return null;
    }

    var filePath = parts[1].Trim();
    string? source = null;
    if (File.Exists(filePath))
    {
        try
        {
            source = File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
            Console.ResetColor();
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"File not found: {filePath}");
        Console.ResetColor();
    }

    return source;
}

ShowSplash();

Scope globalScope = new Scope();
bool showAst = false;

while (true)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("[");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write("Redstone");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("] ");
    Console.ResetColor();

    Console.Write(">>> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Trim().ToLower() == "exit")
        break;

    if (input.Trim() == "clear" || input.Trim() == "cls")
    {
        Console.Clear();
        continue;
    }

    if (input.Trim() == "debug")
    {
        showAst = true;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Debug on.");
        Console.ResetColor();
        continue;
    }
    
    try
    {
        if (input.TrimStart().StartsWith("run"))
        {
            input = GetFile(input);
            if (input == null)
            {
                continue;
            }
        }

        var tokens = RedstoneTokenizer.Tokenize(input);
        var parser = new RedstoneParser(tokens);
        var ast = parser.ParseRoot();

        if (showAst)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(ast);
            Console.ResetColor();
        }

        var result = RedstoneInterpreter.EvaluateProgram(ast, globalScope);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine(result);   
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ResetColor();
    }

    Console.WriteLine();
}

