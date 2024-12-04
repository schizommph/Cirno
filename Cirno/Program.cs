using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Cirno
{
    class Program
    {
        static void Main(string[] args)
        {
            // Stopwatch watch = Stopwatch.StartNew();

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

            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                Console.Write(interpreter.Visit(ast, enviorment));
            }
            catch (BreakException)
            {
                ErrorManager.AddError(new Error($"Break had been used outside of a loop.", ErrorType.UnexpectedException, ErrorSafety.Fatal));
            }
            catch (ReturnException)
            {
                ErrorManager.AddError(new Error($"Return had been used in an innappropriate matter.", ErrorType.UnexpectedException, ErrorSafety.Fatal));
            }

            watch.Stop();
            Console.WriteLine($"Elapsed: {watch.ElapsedMilliseconds / 1000f}");
            Console.ReadKey();
        }
    }
}