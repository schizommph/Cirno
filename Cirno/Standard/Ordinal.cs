using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.Standard
{
    class Ordinal : FunctionNode
    {
        class OrdinalValue : InnerFunctionNode
        {
            public OrdinalValue() : base(new List<string>() { "value" }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                if (arguments[0] is StringClass str)
                {
                    if(str.value.Length >= 1)
                    {
                        return new NumberClass(Convert.ToInt32(Convert.ToChar(str.value.Substring(0, 1))));
                    }
                }

                return new NovaClass();
            }
        }
        public Ordinal() : base("ordn", new List<InnerFunctionNode>())
        {
            base.actions.Add(new OrdinalValue());
        }
    }
}
