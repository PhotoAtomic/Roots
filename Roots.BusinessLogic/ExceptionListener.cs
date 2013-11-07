using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic
{
    public class ExceptionListener<TException>:ExceptionEventListener<TException> where TException : Exception
    {

        private Action<TException> handler;

        public ExceptionListener(Action<TException> handler)
        {
            this.handler = handler;
        }

        protected override void ExceptionOccurred(TException t)
        {
            if (handler == null) return;
            handler(t);
        }
    }
}
