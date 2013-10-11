using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateNew(IsolationLevel isolationLevel = IsolationLevel.None);

        IAsyncUnitOfWork CreateAsyncNew(IsolationLevel isolationLevel = IsolationLevel.None);
        
    }
}
