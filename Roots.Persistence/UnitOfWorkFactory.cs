using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public static class UnitOfWorkFactory
    {
        private static object syncLock = new object();
        public static IUnitOfWorkFactory Current
        {
            get;
            private set;
        }
        public static void Configure(IUnitOfWorkFactory factory)
        {
            lock (syncLock)
            {
                if (UnitOfWorkFactory.Current != null) throw new Exception("Already initialized");
                UnitOfWorkFactory.Current = factory;
            }
        }

        public static IUnitOfWork CreateNew()
        {
            if (Current == null) return null;
            return Current.CreateNew();
        }

        
    }
}
