using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.Standard
{
    class ReadKey : FunctionNode
    {
        class ReadKeyEnd : InnerFunctionNode
        {
            public ReadKeyEnd() : base(new List<string>() { }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                string key = Console.ReadKey().KeyChar.ToString();
                if (key == null) key = "";
                
                return new StringClass(key);
            }
        }
        public ReadKey() : base("readkey", new List<InnerFunctionNode>())
        {
            base.actions.Add(new ReadKeyEnd());
        }
    }
}
