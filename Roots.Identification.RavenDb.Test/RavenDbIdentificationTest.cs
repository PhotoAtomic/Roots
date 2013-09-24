using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roots.Persistence.RavenDb.Helper.Test;
using Microsoft.AspNet.Identity;
using SharpTestsEx;
using System.Threading;

namespace Roots.Identification.RavenDb.Test
{
    [TestClass]
    public class RavenDbIdentificationTest
    {
        [TestMethod]
        public void CreateUser_Expected_Success()
        {
            var userStore = new UserStore(new InMemoryRavenDbUnitOfWorkFactory());

            IUser user = new User();
            user.Id = Guid.NewGuid().ToString();
            user.UserName = "Name";

            var result = userStore.CreateAsync(user, new System.Threading.CancellationToken());
            result.Result.Success.Should().Be.True();
        }

        [TestMethod]
        public void ReadACreatedUser_Expected_Retrieved()
        {
            var userStore = new UserStore(new InMemoryRavenDbUnitOfWorkFactory());

            IUser user = new User();
            user.Id = Guid.NewGuid().ToString();
            user.UserName = "Name";

            var createTask = userStore.CreateAsync(user, new CancellationToken());
            createTask.Result.Success.Should().Be.True();

            var findTask = userStore.FindByNameAsync(user.UserName, new CancellationToken());
            findTask.Result.Id.Should().Be.EqualTo(user.Id);
        }
    }
}
