using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic
{
    public interface IGettableQueryable<T> : IQueryable<T>
    {
        T GetById(object id);
    }
}
