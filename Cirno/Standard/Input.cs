using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.Standard
{
    class Input : FunctionNode
    {
        class InputValue : InnerFunctionNode
        {
            public InputValue() : base(new List<string>() { "value" }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                Console.Write(arguments[0].Out());
                string ret = Console.ReadLine();
                if (ret == null) ret = "";
                
                return new StringClass(ret);
            }
        }
        public Input() : base("input", new List<InnerFunctionNode>())
        {
            base.actions.Add(new InputValue());
        }
    }
}
