namespace Interpreter
{
    internal class Scanner
    {
        public string Source { get; private set; }

        public Scanner(string source)
        {
            Source = source;
        }

        public List<Token> ScanTokens()
        {
            List<Token> tokens = new List<Token>();

            // Implement the scanning logic here
            // . . . 
            // This is a placeholder implementation

            return tokens;
        }
    }
}
