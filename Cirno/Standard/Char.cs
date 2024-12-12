using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.Standard
{
    class Char : FunctionNode
    {
        class CharValue : InnerFunctionNode
        {
            public CharValue() : base(new List<string>() { "value" }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                if (arguments[0] is NumberClass num)
                {
                    return new StringClass(Convert.ToString((char)(int)num.value));
                }

                return new NovaClass();
            }
        }
        public Char() : base("char", new List<InnerFunctionNode>())
        {
            base.actions.Add(new CharValue());
        }
    }
}
