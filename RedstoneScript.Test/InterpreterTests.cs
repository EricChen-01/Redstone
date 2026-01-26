using RedstoneScript.AST.Parser;
using RedstoneScript.Interpreter;
using RedstoneScript.Interpreter.Signals;
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
        public void FunctionCall_WorkCorrectly_10()
        {
            var source = Load("10.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("3", output); // first argument
            Assert.Contains("4", output); // second argument
        }

        [Fact]
        public void IfStatement_WorkCorrectly_11()
        {
            var source = Load("11.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("a is on", output);
        }

        [Fact]
        public void IfStatement_NumericComparison_WorkCorrectly_12()
        {
            var source = Load("12.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("x is less than y", output);
            Assert.Contains("x is 5", output);
        }

        [Fact]
        public void IfStatement_NestedIfStatements_WorkCorrectly_13()
        {
            var source = Load("13.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("x < y", output);
            Assert.Contains("y is 7", output);
        }

        [Fact]
        public void IfStatement_ExpressionsWithConditions_WorkCorrectly_14()
        {
            var source = Load("14.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("a minus b is greater than 2", output);
            Assert.Contains("a plus b equals 7", output);
        }

        [Fact]
        public void WhileStatement_WorkCorrectly_15()
        {
            var source = Load("15.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("3", output);
        }

        [Fact]
        public void WhileStatement_NestedIfStatement_WorkCorrectly_16()
        {
            var source = Load("16.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("2", output);
        }

        [Fact]
        public void WhileStatement_Cut_ExitsRepeater_17()
        {
            var source = Load("17.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("3", output);
            Assert.Contains("done", output);

            Assert.DoesNotContain("4", output);
        }

        [Fact]
        public void WhileStatement_CutInsideNestedIf_WorksCorrectly_18()
        {
            var source = Load("18.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("5", output);
            Assert.Contains("4", output);
            Assert.Contains("3", output);

            Assert.DoesNotContain("2", output);
            Assert.Contains("signal stopped", output);
        }

        [Fact]
        public void Cut_OutsideRepeater_ThrowsError_19()
        {
            var source = Load("19.rsd");

            var ex = Assert.Throws<InvalidOperationException>(() => CaptureConsoleOutput(source));

            Assert.Contains("'cut' used outside of a loop", ex.Message);
        }

        [Fact]
        public void WhileStatement_PulseInsideNestedIf_WorksCorrectly_20()
        {
            var source = Load("20.rsd");

            var output = CaptureConsoleOutput(source);

            Assert.Contains("12", output);
        }

        [Fact]
        public void PulseStatement_OutsideRepeater_ThrowsError_21()
        {
            var source = Load("21.rsd");

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                CaptureConsoleOutput(source);
            });

            Assert.Contains("'pulse' used outside of a loop", exception.Message);
        }

        [Fact]
        public void PulseStatement_InsideNestedComparator_WorksCorrectly_22()
        {
            var source = Load("22.rsd");

            var output = CaptureConsoleOutput(source);

            // Nested comparator message
            Assert.Contains("nested comparator: 1", output);

            // Loop end messages
            Assert.Contains("loop end: 1", output);
            Assert.Contains("loop end: 3", output);
            Assert.Contains("loop end: 4", output);
            Assert.Contains("loop end: 5", output);

            // Make sure skipped iteration is not present
            Assert.DoesNotContain("loop end: 2", output);
        }

        [Fact]
        public void ReturnStatement_SimpleFunction_WorksCorrectly_23()
        {
            // Load the Redstone program
            var source = Load("23.rsd");

            // Capture the console output (Chat calls)
            var output = CaptureConsoleOutput(source);

            // Check that the output contains the returned value
            Assert.Contains("42", output);
        }

        [Fact]
        public void ReturnStatement_FunctionWithoutReturn_ReturnsVoid_24()
        {
            // Load the Redstone program
            var source = Load("24.rsd");

            // Capture console output (Chat calls)
            var output = CaptureConsoleOutput(source);

            // Since no return, expect void output
            // Depending on your interpreter, void might print empty string or "void"
            Assert.True(string.IsNullOrWhiteSpace(output) || output.Contains("void"));
        }

        [Fact]
        public void ReturnStatement_InsideLoop_WorksCorrectly_25()
        {
            // Load the Redstone program
            var source = Load("25.rsd");

            // Capture the console output (Chat calls)
            var output = CaptureConsoleOutput(source);

            // Check that the output contains the returned value
            Assert.Contains("3", output);
        }   

        [Fact]
        public void ReturnStatement_NestedFunctionCalls_WorksCorrectly_26()
        {
            // Load the Redstone program
            var source = Load("26.rsd");

            // Capture the console output (Chat calls)
            var output = CaptureConsoleOutput(source);

            // Check that the output contains the returned value from nested functions
            Assert.Contains("99", output);
        }

        [Fact]
        public void ReturnStatement__WorksCorrectly_27()
        {
            // Load the Redstone program
            var source = Load("27.rsd");

            // Capture the console output (Chat calls)
            var output = CaptureConsoleOutput(source);

            // Check that the output contains the correct returned value
            Assert.Contains("4", output);
        }

        [Fact]
        public void ReturnStatement_OutOfFunction_ThrowsError_28()
        {
            var source = Load("28.rsd");

            var ex = Assert.Throws<InvalidOperationException>(() => CaptureConsoleOutput(source));
            Assert.Contains($"craft' used outside of a function", ex.Message);
        }

        [Fact]
        public void ReturnStatement_InObject_ThrowsError_29()
        {
            // Load the Redstone program
            var source = Load("29.rsd");

            // Capture the console output (Chat calls)
            var ex = Assert.Throws<Exception>(() => CaptureConsoleOutput(source));
        }

        [Fact]
        public void UnaryOperation_UnaryMinus_WorksCorrectly_30()
        {
            // Load the Redstone program
            var source = Load("30.rsd");

            // Capture the console output (Chat calls)
            var output = CaptureConsoleOutput(source);
            
            // should be 10
            Assert.Contains("10", output);
        }

        [Fact]
        public void UnaryOperation_UnaryMinus_WorksCorrectly_31()
        {
            // Load the Redstone program
            var source = Load("31.rsd");

            // Capture the console output (Chat calls)
            var output = CaptureConsoleOutput(source);
            
            // should be on
            Assert.Contains("on", output);
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
                var interpreter = new RedstoneInterpreter();
                
                interpreter.ValidateProgram(program);
                interpreter.EvaluateProgram(program, scope);
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
