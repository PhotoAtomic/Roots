using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roots.Persistence;
using Roots.Persistence.RavenDb.Helper.Test;
using System.Threading;
using SharpTestsEx;

namespace Roots.Identification.RavenDb.Test
{
    [TestClass]
    public class RavenDbSecretTest
    {

        private IAsyncUnitOfWork uow;
        private User user;

        [TestInitialize]
        public void TestInitialize()
        {
            uow = new InMemoryRavenDbUnitOfWorkFactory().CreateAsyncNew(IsolationLevel.ReadItsOwnWrite);
            user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "UserName"
            };
            uow.RepositoryOf<User>().AddAsync(user).Wait();
            uow.CommitAsync().Wait();
        }

        [TestMethod]
        public void CreateSecret_Expected_SecretRetrieved()
        {
            var secretStore = new UserSecretStore(uow);

            var secretString = "secretString";
            var userName = user.UserName;

            var secret = new UserSecret
            {
                Secret = secretString,
                UserName =  userName,
            };

            secretStore.CreateAsync(secret, new CancellationToken()).Wait();

            uow.CommitAsync().Wait();

            var result = secretStore.FindAsync(userName, new CancellationToken()).Result;

            result.Secret.Should().Be(secretString);

        }
    }
}
