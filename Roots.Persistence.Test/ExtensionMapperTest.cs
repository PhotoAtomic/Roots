using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Roots.Persistence;
using Roots.Persistence.Test.Types;
using SharpTestsEx;

namespace Roots.Persistence.Test
{
    [TestClass]
    public class ExtensionMapperTest
    {
        [TestMethod]
        public void ExtendingAsyncMapper_Expected_ExtendedModuleInvoked()
        {
            var l = new List<int>(){1,2,3};
            var intQueryable = l.AsQueryable();

            TestExtensionForQueryableAsyncInt.CalledOn.Should().Be.Null();

            var result = intQueryable.SingleOrDefaultAsync();
            
            TestExtensionForQueryableAsyncInt.CalledOn.Should().Be.EqualTo(intQueryable);
        }

        [TestMethod]
        public void MultipleInterfaceOfExtension_Expected_CorrectOneInvoked()
        {
            var doubleList = new List<double>() { 1d, 2d, 3d };
            var doubleQueryable = doubleList.AsQueryable();

            var resultInt = doubleQueryable.SingleOrDefaultAsync();            

            TestExtensionForQueryableAsyncDouble.CalledOn.Should().Be.EqualTo(doubleQueryable);
            
        }

        [TestMethod]
        public void MultipleProviderOfExtension_Expected_CorrectOneInvoked()
        {
            var boolList = new List<bool>() { true, true, false };
            var boolQueryable = boolList.AsQueryable();

            var floatList = new List<float>() { 1f, 2f, 3f };
            var floatQueryable = floatList.AsQueryable();


            var resultBool = boolQueryable.SingleOrDefaultAsync();
            var resultFloat = floatQueryable.SingleOrDefaultAsync();

            TestExtensionForQueryableAsyncBool.CalledOn.Should().Be.EqualTo(boolQueryable);
            TestExtensionForQueryableAsyncFloat.CalledOn.Should().Be.EqualTo(floatQueryable);
        }
    }
}
