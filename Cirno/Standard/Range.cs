using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.Standard
{
    class Range : FunctionNode
    {
        class RangeValue : InnerFunctionNode
        {
            public RangeValue() : base(new List<string>() { "value" }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                ListClass ret = new ListClass(new List<ObjectClass>());

                NumberClass i = new NumberClass(0);
                while (i.LessThan(arguments[0]).value)
                {
                    ret.AddToItems(new NumberClass(i.value));
                    i = (NumberClass)i.Add(new NumberClass(1));
                }

                return ret;
            }
        }
        public Range() : base("range", new List<InnerFunctionNode>())
        {
            base.actions.Add(new RangeValue());
        }
    }
}
