using RedstoneScript.AST;
using RedstoneScript.Interpreter;
using RedstoneScript.Lexer;
using RedstoneScript.AST.Parser;

Console.WriteLine("Welcome to RedstoneScript CLI v0.0.1!");
Console.WriteLine("Type 'exit' to quit.\n");

while (true)
{
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

        var result = RedstoneInterpreter.EvaluateProgram(ast);

        Console.WriteLine(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Console.WriteLine();
}

