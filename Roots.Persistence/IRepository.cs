using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Persistence
{
    public interface IRepository<T> : IQueryable<T>
    {
        T GetById(ValueType id);

        T GetById(string id);

        void Add(T item);

        void Remove(T item);     
    }
}
