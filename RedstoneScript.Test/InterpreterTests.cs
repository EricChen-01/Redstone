using RedstoneScript.AST.Parser;
using RedstoneScript.Interpreter;
using RedstoneScript.Lexer;

namespace RedstoneScript.Test
{
    public class InterpreterTests
    {
        [Fact]
        public void BooleanComparisonOperators_WorkCorrectly_01()
        {
            var source = Load("01.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("a is: 5", output);
            Assert.Contains("b is: 10", output);
            Assert.Contains("c is: 5", output);

            Assert.Contains("a == c: on", output);
            Assert.Contains("a != b: on", output);
            Assert.Contains("a < b: on", output);
            Assert.Contains("b > a: on", output);
            Assert.Contains("a <= c: on", output);
            Assert.Contains("b >= a: on", output);
        }

        [Fact]
        public void LeadingNewlines_ExecutesCorrectly_02()
        {
            var source = Load("02.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("x is: 3", output);
            Assert.Contains("y is: 3", output);
            Assert.Contains("x == y: on", output);
        }

        [Fact]
        public void Variables_WorkCorrectly_03()
        {
            var source = Load("03.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("5", output);
        }

        [Fact]
        public void AssignmentExpressions_WorkCorrectly_04()
        {
            var source = Load("04.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("10", output); // a
            Assert.Contains("10", output); // b
        }

        [Fact]
        public void ArithmeticPrecedence_WorkCorrectly_05()
        {
            var source = Load("05.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("14", output); // x = 2 + 3 * 4
            Assert.Contains("20", output); // y = (2 + 3) * 4
        }

        [Fact]
        public void BooleanValues_WorkCorrectly_06()
        {
            var source = Load("06.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("on", output);  // a = on
            Assert.Contains("off", output); // b = off
        }

        [Fact]
        public void ComparisonExpressions_WorkCorrectly_07()
        {
            var source = Load("07.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("5", output);       // a
            Assert.Contains("10", output);      // b

            Assert.Contains("on", output);      // a < b
            Assert.Contains("off", output);     // a > b
            Assert.Contains("off", output);     // a == b
            Assert.Contains("on", output);      // a != b
            Assert.Contains("on", output);      // a <= b
            Assert.Contains("on", output);      // b >= a
        }

        [Fact]
        public void ObjectExpressions_WorkCorrectly_08()
        {
            var source = Load("08.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("10", output); // obj.x
            Assert.Contains("20", output); // obj.y
        }

        [Fact]
        public void NestedObjectMemberAccess_WorkCorrectly_09()
        {
            var source = Load("09.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("42", output); // obj.inner.value
        }

        [Fact]
        public void WorkbenchFunctionCall_WorkCorrectly_10()
        {
            var source = Load("10.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("3", output); // first argument
            Assert.Contains("4", output); // second argument
        }
    
        private static string Load(string fileName)
        {
            var baseDir = AppContext.BaseDirectory;
            var samplesDir = Path.Combine(baseDir, "SamplePrograms");
            var fullPath = Path.Combine(samplesDir, fileName);

            if (!File.Exists(fullPath))
            {
                throw new InvalidOperationException($"Redstone Interpreter: Sample file not found: {fileName}");   
            }

            return File.ReadAllText(fullPath);
        }

        private static string CaptureConsoleOutput(string source)
        {
            var output = CaptureConsole(() =>
            {
                var scope = new Scope();
                var tokens = RedstoneTokenizer.Tokenize(source);
                var program = new RedstoneParser(tokens).ParseRoot();
                var interpreter = RedstoneInterpreter.EvaluateProgram(program, scope);
            });

            return output;
        }

        private static string CaptureConsole(Action action)
        {
            var originalOut = Console.Out;
            using var writer = new StringWriter();
            Console.SetOut(writer);

            try
            {
                action();
                return writer.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }
}
