using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.Standard
{
    class Random : FunctionNode
    {
        class RandomEnd : InnerFunctionNode
        {
            System.Random random = new System.Random();
            public RandomEnd() : base(new List<string>() { }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                return new NumberClass((float)random.NextDouble());
            }
        }
        public Random() : base("random", new List<InnerFunctionNode>())
        {
            base.actions.Add(new RandomEnd());
        }
    }
}
