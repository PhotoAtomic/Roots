//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Roots.Persistence;
//using Roots.Persistence.RavenDb.Helper.Test;
//using System.Threading;
//using SharpTestsEx;

//namespace Roots.Identification.RavenDb.Test
//{
//    [TestClass]
//    public class RavenDbSecretTest
//    {

//        private IUnitOfWork uow;
//        private User user;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            uow = new InMemoryRavenDbUnitOfWorkFactory().CreateNew(IsolationLevel.ReadItsOwnWrite);
//            user = new User
//            {
//                Id = Guid.NewGuid(),
//                UserName = "UserName"
//            };
//            uow.RepositoryOf<User>().Add(user);
//            uow.Commit();
//        }

//        [TestMethod]
//        public void CreateSecret_Expected_SecretRetrieved()
//        {
//            var secretStore = new UserSecretStore(uow);

//            var secretString = "secretString";
//            var userName = user.UserName;

//            var secret = new UserSecret
//            {
//                Secret = secretString,
//                UserName =  userName,
//            };

//            secretStore.CreateAsync(secret, new CancellationToken()).Wait();

//            uow.Commit();

//            var result = secretStore.FindAsync(userName, new CancellationToken()).Result;

//            result.Secret.Should().Be(secretString);

//        }
//    }
//}
