using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoAtomic.Extensions.Test.InheritanceDistanceTestTypes;
using SharpTestsEx;

namespace PhotoAtomic.Extensions.Test
{
    [TestClass]
    public class InheritanceDistanceTest
    {
        [TestMethod]
        public void OneLevelHierarchy_Expected_1()
        {
            typeof(ClassIntermediate).Specificity(typeof(ClassBase)).Should().Be(1);
        }

        [TestMethod]
        public void TwoLevelHierarchy_Expected_2()
        {
            typeof(ClassDerived).Specificity(typeof(ClassBase)).Should().Be(2);
        }

        [TestMethod]
        public void TwoLevelHierarchyTroughOneInterfaces_Expected_3()
        {
            typeof(ClassDerived).Specificity(typeof(IInterfaceBase)).Should().Be(3);
        }
        [TestMethod]
        public void OneLevelHierarchyTroughOneInterfaces_Expected_2()
        {
            typeof(ClassDerived).Specificity(typeof(IInterfaceIntermediate)).Should().Be(2);
        }
        [TestMethod]
        public void ZeroLevelHierarchyTroughOneInterfaces_Expected_1()
        {
            typeof(ClassDerived).Specificity(typeof(IInterfaceDerived)).Should().Be(1);
        }

        [TestMethod]
        public void ZeroLevelHierarchyTroughTwoInterfaces_Expected_2()
        {
            typeof(ClassDerived).Specificity(typeof(ICommon)).Should().Be(2);
        }

    }
}
