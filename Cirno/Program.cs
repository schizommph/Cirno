using Cirno.Standard;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Cirno
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunFile("main.crn");
            }
        }
        public static void RunFile(string name)
        {
            if(!name.EndsWith(".crn"))
            {
                ErrorManager.autocheck = true;
                ErrorManager.AddError(new Error("Cirno files can only end in \".crn\".", ErrorType.IncorrectFileExtension, ErrorSafety.Fatal));
            }

            if(!File.Exists(name))
            {
                ErrorManager.autocheck = true;
                ErrorManager.AddError(new Error($"File \"{name}\" does not exist.", ErrorType.FileNotFound, ErrorSafety.Fatal));
            }

            string source = File.ReadAllText(name);

            Lexer lexer = new Lexer(source);
            List<Token> tokens = lexer.Lex();

            if (ErrorManager.ComputeErrors())
            {
                Environment.Exit(1);
            }

            Parser parser = new Parser(tokens);
            TreeNode ast = parser.ParseAST();

            if (ErrorManager.ComputeErrors())
            {
                Environment.Exit(1);
            }

            ErrorManager.autocheck = true;

            Interpreter interpreter = new Interpreter();
            Enviorment enviorment = new Enviorment(null);

            if(!Enviorment.global.ContainsKey("print"))
            {
                Enviorment.global.Add(
                    "print", new FunctionClass(new Print())
                );
                Enviorment.global.Add(
                    "input", new FunctionClass(new Input())
                );
                Enviorment.global.Add(
                    "range", new FunctionClass(new Standard.Range())
                );
                Enviorment.global.Add(
                    "num", new FunctionClass(new Standard.Num())
                );
                Enviorment.global.Add(
                    "char", new FunctionClass(new Standard.Char())
                );
                Enviorment.global.Add(
                    "ordn", new FunctionClass(new Standard.Ordinal())
                );
                Enviorment.global.Add(
                    "readkey", new FunctionClass(new Standard.ReadKey())
                );
                Enviorment.global.Add(
                    "random", new FunctionClass(new Standard.Random())
                );
                Enviorment.global.Add(
                    "sys", new FunctionClass(new Standard.Sys())
                );
            }

            // Stopwatch watch = Stopwatch.StartNew();
            try
            {
                interpreter.Visit(ast, enviorment);
            }
            catch (BreakException)
            {
                ErrorManager.AddError(new Error($"Break had been used outside of a loop.", ErrorType.UnexpectedException, ErrorSafety.Fatal));
            }
            catch (ReturnException)
            {
                ErrorManager.AddError(new Error($"Return had been used in an innappropriate matter.", ErrorType.UnexpectedException, ErrorSafety.Fatal));
            }
        }
    }
}