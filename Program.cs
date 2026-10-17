namespace CoolInterpreter
{
    internal class Program
    {
        static bool hadError = false;
        public static void error(int line, string message) {
            report(line, "", message);
        }
        public static void report(int line, String where, String message) {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;    
        }
        private static void runFile(String path) {
            string content = File.ReadAllText(path);
            run(content);
        }
        private static void runPrompt() {
            while (true) {
                Console.Write("> ");
                String? line = Console.ReadLine();
                if (line == null) break;
                run(line);
                hadError = false;
            }
        }
        private static void run(String source) {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();

            foreach(Token token in tokens) {
                Console.WriteLine(token);
                
            }
        }
        static int Main(string[] args)
        {
            if (args.Length > 1) {
                Console.WriteLine("Only one <script> argument is allowed!");
                hadError = true;
            } else if (args.Length == 1) {
                runFile(args[0]);
            } else {
                runPrompt();
            }
            return hadError ? 1 : 0;
        }
    }
}
