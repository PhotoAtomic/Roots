//using Microsoft.AspNet.Identity;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Roots.Persistence;
//using Roots.Persistence.RavenDb.Helper.Test;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using SharpTestsEx;

//namespace Roots.Identification.RavenDb.Test
//{
//    [TestClass]
//    public class RavenDbRoleTest
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
//        public void AddingAndRetrievingRoleToUser_Expected_RoleRetrieved()
//        {
//            var roleId = "role1";

//            var roleStore = new RoleStore(uow);

//            roleStore.IsUserInRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Result.Should().Be.False();

//            roleStore.AddUserToRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Wait();
//            uow.Commit();

//            roleStore.IsUserInRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Result.Should().Be.True();
//        }


//        [TestMethod]
//        public void RemovingRoleFromUser_Expected_RoleRetrieved()
//        {
//            var roleId = "role1";

//            var roleStore = new RoleStore(uow);
            

//            roleStore.AddUserToRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Wait();
//            uow.Commit();

//            roleStore.IsUserInRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Result.Should().Be.True();

//            roleStore.RemoveUserFromRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Wait();
//            uow.Commit();

//            roleStore.IsUserInRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Result.Should().Be.False();


//        }


//        [TestMethod]
//        public void DeletingRoleUsedByUserWithCheck_Expected_RemovalFailed()
//        {
//            var roleId = "role1";

//            var roleStore = new RoleStore(uow);

//            roleStore.AddUserToRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Wait();
//            uow.Commit();

//            roleStore.DeleteRoleAsync(roleId, true, new CancellationToken()).Result.Success.Should().Be.False();
//            roleStore.IsUserInRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Result.Should().Be.True();
           
//        }

//        [TestMethod]
//        public void DeletingRoleUsedByUserWithoutCheck_Expected_RemovalFailed()
//        {
//            var roleId = "role1";

//            var roleStore = new RoleStore(uow);

//            roleStore.AddUserToRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Wait();
//            uow.Commit();            

//            roleStore.DeleteRoleAsync(roleId, false, new CancellationToken()).Result.Success.Should().Be.True();
//            roleStore.IsUserInRoleAsync(((IUser)user).Id, roleId, new CancellationToken()).Result.Should().Be.False();
//        }
//    }
//}
