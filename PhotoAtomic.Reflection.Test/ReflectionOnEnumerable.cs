using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoAtomic.Reflection;
using System.Collections.Generic;
using SharpTestsEx;

namespace PhotoAtomic.Reflection.Test
{
    [TestClass]
    public class ReflectionOnEnumerable
    {
        [TestMethod]
        public void ReflectionOnEnumerable_Returned_Type()
        {
            Reflect.GetSequenceType(typeof(IEnumerable<int>))
                .Should().Be.EqualTo(typeof(int));
        }
    }
}
