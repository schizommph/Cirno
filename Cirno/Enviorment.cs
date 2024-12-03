using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno
{
    internal class Enviorment
    {
        public static Dictionary<string, ObjectClass> global { get; private set; } = new Dictionary<string, ObjectClass>();

        public Enviorment parent { get; private set; }
        public Dictionary<string, ObjectClass> variables { get; private set; }
        public Enviorment(Enviorment parent)
        {
            this.variables = new Dictionary<string, ObjectClass>();
            this.parent = parent;
        }

        public bool ContainsVariable(string name)
        {
            if (variables.ContainsKey(name) || global.ContainsKey(name))
            {
                return true;
            }
            else if(parent == null)
            {
                return false;
            }
            else
            {
                return parent.ContainsVariable(name);
            }
        }
        public ObjectClass GetVariable(string name)
        {
            if (variables.ContainsKey(name))
            {
                return variables[name];
            }
            else if (global.ContainsKey(name))
            {
                return global[name];
            }
            else if (parent == null)
            {
                ErrorManager.AddError(new Error($"Variable {name} does not exist.", ErrorType.NoFoundVariable, ErrorSafety.Warning));
                return new NovaClass();
            }
            else
            {
                return parent.GetVariable(name);
            }
        }
        public ObjectClass SetVariable(string name, ObjectClass expr)
        {
            if (!ContainsVariable(name) || variables.ContainsKey(name))
            {
                variables[name] = expr;
                return expr;
            }
            else if (global.ContainsKey(name))
            {
                global[name] = expr;
                return expr;
            }
            else
            {
                return parent.SetVariable(name, expr);
            }
        }

        public static ObjectClass SetGlobalVariable(string name, ObjectClass expr)
        {
            global[name] = expr;
            return expr;
        }
    }
}
