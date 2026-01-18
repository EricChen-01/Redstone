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

