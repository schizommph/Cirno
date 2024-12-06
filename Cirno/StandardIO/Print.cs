using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.StandardIO
{
    class Print : FunctionNode
    {
        class PrintNewLine : InnerFunctionNode
        {
            public PrintNewLine() : base(new List<string>(), null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                Console.WriteLine();
                return new NovaClass();
            }
        }
        class PrintValue : InnerFunctionNode
        {
            public PrintValue() : base(new List<string>() { "value" }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                Console.WriteLine(arguments[0].Out());
                return new NovaClass();
            }
        }
        class PrintValueEnd : InnerFunctionNode
        {
            public PrintValueEnd() : base(new List<string>() { "value", "end" }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                Console.Write(arguments[0].Out() + arguments[1].Out());
                return new NovaClass();
            }
        }

        public Print() : base("print", new List<InnerFunctionNode>())
        {
            base.actions.Add(new PrintNewLine());
            base.actions.Add(new PrintValue());
            base.actions.Add(new PrintValueEnd());
        }
    }
}
