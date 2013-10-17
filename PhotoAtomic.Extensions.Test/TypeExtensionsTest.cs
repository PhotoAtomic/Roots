using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTestsEx;

namespace PhotoAtomic.Extensions.Test
{
    [TestClass]
    public class TypeExtensionsTest
    {
        [TestMethod]
        public void IsEnumerable()
        {
            typeof(IList<int>).IsEnumerable().Should().Be.True();
        }

    }
}
