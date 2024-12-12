using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.Standard
{
    class Num : FunctionNode
    {
        class NumValue : InnerFunctionNode
        {
            public NumValue() : base(new List<string>() { "value" }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                if (arguments[0] is StringClass str)
                {
                    bool isNumeric = int.TryParse(str.value, out int num);

                    if (isNumeric)
                    {
                        return new NumberClass(num);
                    }
                }

                return new NovaClass();
            }
        }
        public Num() : base("num", new List<InnerFunctionNode>())
        {
            base.actions.Add(new NumValue());
        }
    }
}
