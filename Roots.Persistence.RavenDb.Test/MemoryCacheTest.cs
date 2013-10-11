using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roots.Persistence.RavenDb.Helper.Test;
using Roots.Persistence.Cache;
using Roots.Persistence.RavenDb.Test.Types;
using System.Linq;
using SharpTestsEx;

namespace Roots.Persistence.RavenDb.Test
{
    [TestClass]
    public class MemoryCacheTest
    {
        [TestMethod]
        public void RetrievingFromCacheAnAddedItem_Expected_ItemFound()
        {
            var factory = new InMemoryRavenDbUnitOfWorkFactory();
            var memCache = new MemoryCache(factory);
            var testName = "Test";

            var guid = Guid.NewGuid();
            var entity = new TestEntity{ Id = guid, Name = testName};

            memCache.RepositoryOf<TestEntity>().Add(entity);

            var foundFromCache = memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
            foundFromCache.Should().Not.Be.Null();
            foundFromCache.Id.Should().Be.EqualTo(guid);
        }

        [TestMethod]
        public void RetrievingFromCacheAnItemAddedFromStorage_Expected_ItemFound()
        {
            var factory = new InMemoryRavenDbUnitOfWorkFactory();
            var memCache = new MemoryCache(factory);
            var testName = "Test";
            var guid = Guid.NewGuid();
            var entity = new TestEntity { Id = guid, Name = testName };

            using (var uow = factory.CreateNew())
            {
                uow.RepositoryOf<TestEntity>().Add(entity);
                uow.Commit();
            }            

            var foundFromCache = memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
            foundFromCache.Should().Not.Be.Null();
            foundFromCache.Id.Should().Be.EqualTo(guid);
        }
    }
}
