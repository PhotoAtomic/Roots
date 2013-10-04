using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.AspNet.Composition
{
    public class MefAdapter<T> where T : new()
    {
        private readonly T _typeToExport;

        public MefAdapter()
        {
            _typeToExport = new T();
        }

        [Export]
        public virtual T TypeToExport
        {
            get { return _typeToExport; }
        }
    }

}
