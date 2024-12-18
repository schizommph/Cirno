using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno
{
    class BreakException : Exception
    {

    }
    class ContinueException : Exception
    {

    }
    class ReturnException : Exception
    {
        public ObjectClass value { get; private set; }
        public ReturnException(ObjectClass value)
        {
            this.value = value;
        }
    }
}
