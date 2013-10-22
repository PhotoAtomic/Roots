using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roots.Persistence.RavenDb.Helper.Test;
using Roots.Persistence.Cache;
using Roots.Persistence.RavenDb.Test.Types;
using System.Linq;
using SharpTestsEx;
using Roots.Persistence.RavenDb.Cache;

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

        [TestMethod]
        public void ApplyingCacheAnItemAddedFromStorage_Expected_ModificationApplied()
        {
            var factory = new InMemoryRavenDbUnitOfWorkFactory();
            var memCache = new MemoryCache(factory);
            var testName = "Test";
            var guid = Guid.NewGuid();
            var entity = new TestEntity { Id = guid, Name = testName };


            var foundFromCache = memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
            foundFromCache.Should().Be.Null();

            memCache.RepositoryOf<TestEntity>().Add(entity);
            memCache.Commit();

            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var found  = uow.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
                found.Name.Should().Be.EqualTo(testName);
                found.Id.Should().Be.EqualTo(guid);
            }            

        }


        [TestMethod]
        public void RemovingFromCacheAnItemInTheStorage_Expected_ItemRemoved()
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

            memCache.RepositoryOf<TestEntity>().RemoveById(guid);
            memCache.Commit();

            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var found = uow.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
                found.Should().Be.Null();                
            }    
        }


        [TestMethod]
        public void QueryingARemovedItem_Expected_ItemNotRetrieved()
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

            memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).Count().Should().Be(1);
            memCache.RepositoryOf<TestEntity>().RemoveById(guid);
            memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).Count().Should().Be(0);
            

            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var found = uow.RepositoryOf<TestEntity>().GetById(guid);
                found.Should().Not.Be.Null();
            }

            memCache.Commit();

            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var found = uow.RepositoryOf<TestEntity>().GetById(guid);
                found.Should().Be.Null();
            }
        }



        [TestMethod]
        public void RetrievingSingleItemItem_Expected_ItemNotRetrieved()
        {
            var factory = new InMemoryRavenDbUnitOfWorkFactory();

            var memCache = new MemoryCache(new RavenDbUnitOfWorkFactoryWrapperForMemoryCache(factory));
            var testName = "Test";


            
            var item = memCache.RepositoryOf<TestEntity>().Where(x => x.Name == testName).SingleOrDefault();
            item.Should().Be.Null();
            
        }


        [TestMethod]
        public void RetrievingByIdOfDifferentType_Expected_ItemRetrieved()
        {
            var factory = new InMemoryRavenDbUnitOfWorkFactory();

            var memCache = new MemoryCache(factory);
            var testName = "Test";
            var guid = Guid.NewGuid();
            var entity = new TestEntity { Id = guid, Name = testName };

            memCache.RepositoryOf<TestEntity>().Add(entity);
            var retrievedEntity = memCache.RepositoryOf<TestEntity>().GetById(guid.ToString());

            retrievedEntity.Should().Be.SameInstanceAs(entity);

        }

    }
}
