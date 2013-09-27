using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roots.Persistence.RavenDb.Test.Types;
using System.Reflection;
using SharpTestsEx;

namespace Roots.Persistence.RavenDb.Test
{
    [TestClass]
    public class RepositoryBaseTest
    {

        public class Accessor<T> : RavenDbRespositoryBase<T>
        {
            public Accessor(Func<Type,PropertyInfo> generator):base(generator)
            {

            }

            public new TOut InvokeByTargetType<TOut>(object id, Func<string, TOut> methodWithStringId, Func<ValueType, TOut> methodWithValueTypeId)
            {
                return base.InvokeByTargetType<TOut>(id, methodWithStringId, methodWithValueTypeId);
            }

            protected override Raven.Client.Linq.IRavenQueryable<T> GetRavenQueryable()
            {
                throw new NotImplementedException();
            }
        }


        [TestMethod]
        public void PassingStringToEntityWithStringId_Expected_StringMethodInvoke()
        {
            var baseRepository = new Accessor<EntityWithStringId>(IdGenerator);

            string id = "test";            

            baseRepository.InvokeByTargetType(
                id, 
                x =>
                {
                    x.Should().Be(id);
                    return 0;
                }, 
                x => 
                {
                    Assert.Fail();
                    return 0;
                });
        }


        [TestMethod]
        public void PassingGuidToEntityWithStringId_Expected_StringMethodInvoke()
        {
            var baseRepository = new Accessor<EntityWithStringId>(IdGenerator);

            var id = Guid.NewGuid();

            baseRepository.InvokeByTargetType(
                id,
                x =>
                {
                    x.Should().Be.EqualTo(id.ToString());
                    return 0;
                },
                x =>
                {
                    Assert.Fail();
                    return 0;
                });
        }

        [TestMethod]
        public void PassingGuidToEntityWithGuidId_Expected_ValueTypeMethodInvoke()
        {
            var baseRepository = new Accessor<EntityWithGuidId>(IdGenerator);

            var id = Guid.NewGuid();

            baseRepository.InvokeByTargetType(
                id,
                x =>
                {
                    Assert.Fail();                    
                    return 0;
                },
                x =>
                {
                    Guid.Equals(x, id).Should().Be.True();
                    return 0;
                });
        }

        [TestMethod]
        public void PassingStringToEntityWithGuidId_Expected_ValueTypeMethodInvoke()
        {
            var baseRepository = new Accessor<EntityWithGuidId>(IdGenerator);

            var id = Guid.NewGuid();

            baseRepository.InvokeByTargetType(
                id.ToString(),
                x =>
                {
                    Assert.Fail();
                    return 0;
                },
                x =>
                {
                    Guid.Equals(x, id).Should().Be.True();
                    return 0;
                });
        }


        private PropertyInfo IdGenerator(Type t)
        {
            return t.GetProperty("Id");
        }
    }
}
