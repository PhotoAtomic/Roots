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
    public class AsyncMemoryCacheTest
    {
        [TestMethod]
        public void RetrievingFromCacheAnAddedItem_Expected_ItemFound()
        {

            

            var factory = new InMemoryRavenDbUnitOfWorkFactoryForCache();
            var memCache = new AsyncMemoryCache(factory);
            var testName = "Test";

            var guid = Guid.NewGuid();
            var entity = new TestEntity{ Id = guid, Name = testName};

            memCache.RepositoryOf<TestEntity>().AddAsync(entity);

            var foundFromCache = memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
            
            foundFromCache.Should().Not.Be.Null();
            foundFromCache.Id.Should().Be.EqualTo(guid);
            
        }

        [TestMethod]
        public void RetrievingFromCacheAnItemAddedFromStorage_Expected_ItemFound()
        {
            var factory = new InMemoryRavenDbUnitOfWorkFactoryForCache();
            var memCache = new AsyncMemoryCache(factory);
            var testName = "Test";
            var guid = Guid.NewGuid();
            var entity = new TestEntity { Id = guid, Name = testName };

            using (var uow = factory.CreateAsyncNew())
            {
                uow.RepositoryOf<TestEntity>().AddAsync(entity).Wait();
                uow.CommitAsync().Wait();
            }

            var foundFromCache = memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
            foundFromCache.Should().Not.Be.Null();
            foundFromCache.Id.Should().Be.EqualTo(guid);
        }

        [TestMethod]
        public void ApplyingCacheAnItemAddedFromStorage_Expected_ModificationApplied()
        {
            var factory = new InMemoryRavenDbUnitOfWorkFactoryForCache();
            var memCache = new AsyncMemoryCache(factory);
            var testName = "Test";
            var guid = Guid.NewGuid();
            var entity = new TestEntity { Id = guid, Name = testName };


            var foundFromCache = memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
            foundFromCache.Should().Be.Null();

            memCache.RepositoryOf<TestEntity>().AddAsync(entity).Wait();
            memCache.CommitAsync().Wait();

            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var found = uow.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
                found.Name.Should().Be.EqualTo(testName);
                found.Id.Should().Be.EqualTo(guid);
            }

        }


        [TestMethod]
        public void RemovingFromCacheAnItemInTheStorage_Expected_ItemRemoved()
        {
            var factory = new InMemoryRavenDbUnitOfWorkFactoryForCache();

            var memCache = new AsyncMemoryCache(factory);
            var testName = "Test";
            var guid = Guid.NewGuid();
            var entity = new TestEntity { Id = guid, Name = testName };

            using (var uow = factory.CreateAsyncNew())
            {
                uow.RepositoryOf<TestEntity>().AddAsync(entity).Wait();
                uow.CommitAsync().Wait();
            }

            memCache.RepositoryOf<TestEntity>().RemoveByIdAsync(guid).Wait();
            memCache.CommitAsync().Wait();

            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var found = uow.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
                found.Should().Be.Null();
            }
        }


        [TestMethod]
        public void QueryingARemovedItem_Expected_ItemNotRetrieved()
        {
            var factory = new InMemoryRavenDbUnitOfWorkFactoryForCache();

            var memCache = new AsyncMemoryCache(factory);
            var testName = "Test";
            var guid = Guid.NewGuid();
            var entity = new TestEntity { Id = guid, Name = testName };

            using (var uow = factory.CreateAsyncNew())
            {
                uow.RepositoryOf<TestEntity>().AddAsync(entity).Wait();
                uow.CommitAsync().Wait();
            }

            memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).Count().Should().Be(1);
            memCache.RepositoryOf<TestEntity>().RemoveByIdAsync(guid).Wait();
            memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).Count().Should().Be(0);


            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var found = uow.RepositoryOf<TestEntity>().GetById(guid);
                found.Should().Not.Be.Null();
            }

            memCache.CommitAsync().Wait();

            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var found = uow.RepositoryOf<TestEntity>().GetById(guid);
                found.Should().Be.Null();
            }
        }
    }
}
