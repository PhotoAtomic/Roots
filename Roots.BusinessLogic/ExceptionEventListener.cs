using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic
{
    public abstract class ExceptionEventListener
        <T> : IExceptionEventListener where T : Exception
    {

        public void OnException(Exception exception)
        {
            ExceptionOccurred((T)exception);
        }

        protected abstract void ExceptionOccurred(T t);
    }
}
