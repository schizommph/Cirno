using System.Runtime.InteropServices;

namespace Cirno
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = File.ReadAllText("main.crn");

            Lexer lexer = new Lexer(source);
            List<Token> tokens = lexer.Lex();

            if(ErrorManager.ComputeErrors())
            {
                Console.ReadKey();
                Environment.Exit(1);
            }

            Parser parser = new Parser(tokens);
            TreeNode ast = parser.ParseAST();

            if (ErrorManager.ComputeErrors())
            {
                Console.ReadKey();
                Environment.Exit(1);
            }

            ErrorManager.autocheck = true;

            Interpreter interpreter = new Interpreter();
            Enviorment enviorment = new Enviorment(null);

            Console.Write(interpreter.Visit(ast, enviorment));

            Console.ReadKey();
        }
    }
}