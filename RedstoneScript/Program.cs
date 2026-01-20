using RedstoneScript.AST;
using RedstoneScript.Interpreter;
using RedstoneScript.Lexer;
using RedstoneScript.AST.Parser;

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

static void RunFile(string input, Scope scope)
{
    // Extract the file path after "run"
    var parts = input.Trim().Split(' ', 2); // split into ["run", "filepath"]
    if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Usage: run <file_path>");
        Console.ResetColor();
        return;
    }

    var filePath = parts[1].Trim();

    if (File.Exists(filePath))
    {
        try
        {
            var source = File.ReadAllText(filePath);
            var tokens = RedstoneTokenizer.Tokenize(source);
            var parser = new RedstoneParser(tokens);
            var ast = parser.ParseRoot();

            var result = RedstoneInterpreter.EvaluateProgram(ast, scope);

            if (result != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result);
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error executing file {filePath}: {ex.Message}");
            Console.ResetColor();
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"File not found: {filePath}");
        Console.ResetColor();
    }
}

ShowSplash();

Scope globalScope = new Scope();

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
    
    try
    {
        if (input.TrimStart().StartsWith("run"))
        {
            RunFile(input, globalScope);
            continue;
        }

        var tokens = RedstoneTokenizer.Tokenize(input);
        var parser = new RedstoneParser(tokens);
        var ast = parser.ParseRoot();
        
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

