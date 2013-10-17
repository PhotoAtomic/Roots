using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoAtomic.Extensions.Test.InheritanceDistanceTestTypes;
using SharpTestsEx;
using System.Collections.Generic;

namespace PhotoAtomic.Extensions.Test
{
    [TestClass]
    public class InheritanceDistanceTest
    {
        [TestMethod]
        public void OneLevelHierarchy_Expected_1()
        {
            typeof(ClassIntermediate).SpecificityDistance(typeof(ClassBase)).Should().Be(1);
        }

        [TestMethod]
        public void TwoLevelHierarchy_Expected_2()
        {
            typeof(ClassDerived).SpecificityDistance(typeof(ClassBase)).Should().Be(2);
        }

        [TestMethod]
        public void TwoLevelHierarchyTroughOneInterfaces_Expected_3()
        {
            typeof(ClassDerived).SpecificityDistance(typeof(IInterfaceBase)).Should().Be(3);
        }
        [TestMethod]
        public void OneLevelHierarchyTroughOneInterfaces_Expected_2()
        {
            typeof(ClassDerived).SpecificityDistance(typeof(IInterfaceIntermediate)).Should().Be(2);
        }
        [TestMethod]
        public void ZeroLevelHierarchyTroughOneInterfaces_Expected_1()
        {
            typeof(ClassDerived).SpecificityDistance(typeof(IInterfaceDerived)).Should().Be(1);
        }

        [TestMethod]
        public void ZeroLevelHierarchyTroughTwoInterfaces_Expected_2()
        {
            typeof(ClassDerived).SpecificityDistance(typeof(ICommon)).Should().Be(2);
        }

        [TestMethod]
        public void GenericBaseType()
        {
            typeof(GenericClass<int>).SpecificityDistance(typeof(GenericClass<>)).Should().Be(1);
        }

        [TestMethod]
        public void GenericBaseType2()
        {
            typeof(GenericClass<int>).SpecificityDistance(typeof(GenericClass<int>)).Should().Be(0);
        }

        [TestMethod]
        public void GenericBaseType3()
        {
            typeof(GenericClass<IEnumerable<int>>).SpecificityDistance(typeof(GenericClass<IEnumerable<int>>)).Should().Be(0);
        }

        [TestMethod]
        public void GenericBaseType4()
        {
            typeof(GenericClass<IEnumerable<int>>).SpecificityDistance(typeof(GenericClass<>)).Should().Be(1);
        }

        

    }
}
