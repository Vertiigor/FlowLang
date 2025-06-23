using Interpreter.Grammar;
using Interpreter.Grammar.Expressions;
using System.Text;

namespace Interpreter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                RunPromt();
                return;
            }

            if (args.Length > 1)
            {
                Console.WriteLine("Usage: flow {file.fl}");
                return;
            }

            // Ensure the first argument is a valid file path
            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }
            else
            {
                Run(filePath);
            }
        }

        private static void Run(string filePath)
        {
            // Load the file and execute the program
            byte[] bytes = File.ReadAllBytes(filePath);

            // Convert the byte array to a string using Encoding.UTF8.GetString
            string fileContent = Encoding.UTF8.GetString(bytes);

            Execute(fileContent);
        }

        private static void Execute(string content)
        {
            Scanner scanner = new Scanner(content);

            List<Token> tokens = scanner.ScanTokens();

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }

            Parser parser = new Parser(tokens);

            Expression expression = parser.Parse();

            Console.WriteLine("Parsed expression: " + AstPrinter.Print(expression));
            Console.WriteLine("Evaluating expression...");
            var result = Interpreter.Evaluate(expression);
            Console.WriteLine("Result: " + result);

            if (expression == null)
            {
                Console.WriteLine("Parsing failed.");
                return;
            }
        }

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                report(token.Line, " at end", message);
            }
            else
            {
                report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }

        private static void report(int line, string where, string message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);

            // hadError = true;
        }

        private static void RunPromt()
        {
            Console.WriteLine("Welcome to the Flow Interpreter!");
            Console.WriteLine("Type 'exit' to quit.");

            int lineNumber = 1;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{lineNumber} |> ");
                Console.ResetColor();

                string input = Console.ReadLine();

                lineNumber++;

                if (input?.ToLower() == "exit")
                {
                    return;
                }

                Execute(input);
            }
        }
    }
}
