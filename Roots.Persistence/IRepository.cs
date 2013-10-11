using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Roots.Persistence
{
    public interface IRepository<T> : IQueryable<T>
    {
        T GetById(object id);

        void RemoveById(object id);

        void Add(T item);

        void Remove(T item);

        PropertyInfo GetIdProperty();
    }
}
