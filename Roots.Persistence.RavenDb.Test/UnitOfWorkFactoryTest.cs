using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client;
using Roots.Persistence.RavenDb;
using Raven.Client.Document;
using Roots.Persistence.RavenDb.Test.Types;
using Raven.Client.Connection;
using SharpTestsEx;

namespace Roots.Identification.RavenDb.Test
{
    [TestClass]
    public class UnitOfWorkFactoryTest
    {
        [TestMethod]
        public void TestTypeLockRule()
        {
            var documentStore = new Mock<IDocumentStore>();
            var conventions = new Mock<DocumentConvention>();
            documentStore.Setup(x => x.Conventions).Returns(conventions.Object);            
            RavenDbUnitOfWorkFactory.SetupKeyTypeLockRule(documentStore.Object);            
            
            var prefixBase = conventions.Object.GenerateDocumentKey(string.Empty, null, new TestClassBase());
            var prefixDerived = conventions.Object.GenerateDocumentKey(string.Empty, null, new TestClassDerived());

            prefixDerived.Should().Be.EqualTo(prefixBase);
        }
    }
}
