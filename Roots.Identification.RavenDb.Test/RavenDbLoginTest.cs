using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roots.Persistence.RavenDb.Helper.Test;
using SharpTestsEx;
using Roots.Persistence;
using Microsoft.AspNet.Identity;
using System.Threading;
using Roots.Persistence.RavenDb;

namespace Roots.Identification.RavenDb.Test
{
    [TestClass]
    public class RavenDbLoginTest
    {
        private IAsyncUnitOfWork uow;

        [TestInitialize]
        public void TestInitialize()
        {
            uow = new InMemoryRavenDbUnitOfWorkFactory().CreateAsyncNew(IsolationLevel.ReadItsOwnWrite);
                //new RavenDbUnitOfWorkFactory("http://localhost:8080").CreateAsyncNew();
        }


        [TestMethod]
        public void CreateLoginForUser_ExpectedLoginCreated()
        {
            var loginStore = new UserLoginStore(uow);

            string userId = "user";
            string provider = "local";
            string key = "key";

            var login = loginStore.CreateNewInstance(userId,provider,key);
            login.UserId.Should().Be(userId);
            login.LoginProvider.Should().Be(provider);
            login.ProviderKey.Should().Be(key);

        }

        [TestMethod]
        public void AddLoginForUser_ExpectedLoginCreated()
        {
            var userStore = new UserStore(uow);
            var loginStore = new UserLoginStore(uow);

            var userId = Guid.NewGuid();
            string userName = "name";
            string provider = "local";
            string key = "key";
            

            IUser user = new User();
            user.Id = userId.ToString();
            user.UserName = userName;

            var userCreationOperation = userStore.CreateAsync(user, new CancellationToken());
            userCreationOperation.Result.Success.Should().Be.True();
            uow.CommitAsync().Wait();
            var u = userStore.FindAsync(userId.ToString(),new CancellationToken());
            u.Wait();
            var a = u.Result;

            var login = loginStore.CreateNewInstance(user.Id, provider, key);
            var addOperation =loginStore.AddAsync(login, new CancellationToken());
            addOperation.Result.Success.Should().Be.True();
            uow.CommitAsync().Wait();
           

        }       
    }
}
