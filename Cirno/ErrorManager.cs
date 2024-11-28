using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno
{
    enum ErrorType
    {
        Temp,
        NoFoundVariable,
        SymbolNotRecognized,
        NumberCannotHaveMultiplePeriods,
        UnexpectedToken,
        UnfoundToken,
        UnexpectedArithmeticOperation,
        UnexpectedLogicalOperation,
    }
    enum ErrorSafety
    {
        Fatal,
        Warning,
    }
    class Error
    {
        public int line;
        public string message { get; private set; }
        public ErrorType type { get; private set; }
        public ErrorSafety safety { get; private set; }
        public Error(string message, int line, ErrorType type, ErrorSafety safety)
        {
            this.message = message;
            this.line = line;
            this.type = type;
            this.safety = safety;
        }
        public Error(string message, ErrorType type, ErrorSafety safety)
        {
            this.message = message;
            this.line = -1;
            this.type = type;
            this.safety = safety;
        }
        public void Print()
        {
            switch (safety)
            {
                case ErrorSafety.Fatal:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ErrorSafety.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.WriteLine($"\n[{(int)type}] {message}");
            if (line != -1)
            {
                Console.Write($"\tAt line {line}.");
            }
        }
    }
    static class ErrorManager
    {
        static List<Error> errors = new List<Error>();
        static public bool autocheck = false;
        public static void AddError(Error error)
        {
            errors.Add(error);
            if (autocheck)
            {
                if (ComputeErrors())
                {
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }
        }
        public static bool ComputeErrors()
        {
            ConsoleColor backgroundColor = Console.ForegroundColor;
            bool fatality = false;
            foreach(Error error in errors)
            {
                if(error.safety == ErrorSafety.Fatal)
                {
                    fatality = true;
                }
                error.Print();
            }
            Console.ForegroundColor = backgroundColor;

            return fatality;
        }
    }
}
