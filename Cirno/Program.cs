using Cirno.Standard;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Cirno
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectClass ret = null;
            if (args.Length == 1)
            {
                ret = RunFile(args[0]);
            }
            else
            {
                ret = RunFile("main.crn");
            }
            if (ret != null)
            {
                Console.Write(ret.Out());
            }
            Console.ReadKey();
        }
        public static ObjectClass RunFile(string name)
        {
            string source = File.ReadAllText("main.crn");

            Lexer lexer = new Lexer(source);
            List<Token> tokens = lexer.Lex();

            if (ErrorManager.ComputeErrors())
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

            Enviorment.global.Add(
                "print", new FunctionClass(new Print())
            );
            Enviorment.global.Add(
                "input", new FunctionClass(new Input())
            );
            Enviorment.global.Add(
                "range", new FunctionClass(new Standard.Range())
            );

            // Stopwatch watch = Stopwatch.StartNew();
            ObjectClass ret = null;
            try
            {
                ret = interpreter.Visit(ast, enviorment);
            }
            catch (BreakException)
            {
                ErrorManager.AddError(new Error($"Break had been used outside of a loop.", ErrorType.UnexpectedException, ErrorSafety.Fatal));
            }
            catch (ReturnException)
            {
                ErrorManager.AddError(new Error($"Return had been used in an innappropriate matter.", ErrorType.UnexpectedException, ErrorSafety.Fatal));
            }
            return ret;
        }
    }
}