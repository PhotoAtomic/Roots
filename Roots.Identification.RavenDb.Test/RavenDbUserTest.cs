//using System;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Roots.Persistence.RavenDb.Helper.Test;
//using Microsoft.AspNet.Identity;
//using SharpTestsEx;
//using System.Threading;
//using Roots.Persistence;
//using System.Diagnostics;

//namespace Roots.Identification.RavenDb.Test
//{
//    [TestClass]
//    public class RavenDbUserTest
//    {

//        private IUnitOfWork uow;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            uow = new InMemoryRavenDbUnitOfWorkFactory().CreateNew(IsolationLevel.ReadItsOwnWrite);
//        }

//        [TestMethod]
//        public void CreateUser_Expected_Success()
//        {
            
//            var userStore = new UserStore(uow);

//            IUser user = new User();
//            user.Id = Guid.NewGuid().ToString();
//            user.UserName = "Name";

//            var result = userStore.CreateAsync(user, new CancellationToken());
//            uow.Commit();
//            result.Result.Success.Should().Be.True();
//        }

//        [TestMethod]
//        public void ReadACreatedUser_Expected_Retrieved()
//        {           

//            var userStore = new UserStore(uow);

//            User user = new User();
//            user.Id = Guid.NewGuid();
//            user.UserName = "Name";

//            var createTask = userStore.CreateAsync(user, new CancellationToken());
//            createTask.Wait();
//            createTask.Result.Success.Should().Be.True();
//            uow.Commit();
            

//            var findTask = userStore.FindByNameAsync(user.UserName, new CancellationToken());
//            findTask.Wait();
//            findTask.Result.Id.Should().Be.EqualTo(user.Id.ToString());
//        }

//        [TestMethod]
//        public void ReadACreatedUserEvenWithPlainDomain_Expected_RetrievedTheSameUserObject()
//        {            
            
//            var userStore = new UserStore(uow);

//            var user = new User();
//            var id = Guid.NewGuid();
//            user.Id = id;
//            user.UserName = "Name";

//            var createTask = userStore.CreateAsync(user, new CancellationToken());
//            createTask.Result.Success.Should().Be.True();

//            uow.Commit();         
            
//            var retrievedUser = uow.RepositoryOf<Domain.User>().GetById(id);
//            retrievedUser.Should().Not.Be.Null();
//            retrievedUser.UserName.Should().Be(user.UserName);

//        }
//    }
//}
