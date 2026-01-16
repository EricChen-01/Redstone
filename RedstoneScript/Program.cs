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

    try
    {
        var tokens = RedstoneTokenizer.Tokenize(input);
        
        var parser = new RedstoneParser(tokens);
        var ast = parser.ParseRoot();

        Console.WriteLine("\nAST:");
        Console.WriteLine(ast.ToString());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Console.WriteLine();
}