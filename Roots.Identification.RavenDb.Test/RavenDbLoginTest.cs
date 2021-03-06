﻿//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Roots.Persistence.RavenDb.Helper.Test;
//using SharpTestsEx;
//using Roots.Persistence;
//using Microsoft.AspNet.Identity;
//using System.Threading;
//using Roots.Persistence.RavenDb;
//using System.Linq;

//namespace Roots.Identification.RavenDb.Test
//{
//    [TestClass]
//    public class RavenDbLoginTest
//    {
//        private IUnitOfWork uow;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            uow = new InMemoryRavenDbUnitOfWorkFactory().CreateNew(IsolationLevel.ReadItsOwnWrite);                
//        }


//        [TestMethod]
//        public void CreateLoginForUser_ExpectedLoginCreated()
//        {
//            var loginStore = new UserLoginStore(uow);

//            string userId = "user";
//            string provider = "local";
//            string key = "key";

//            var login = loginStore.CreateNewInstance(userId,provider,key);
//            login.UserId.Should().Be(userId);
//            login.LoginProvider.Should().Be(provider);
//            login.ProviderKey.Should().Be(key);

//        }

//        [TestMethod]
//        public void AddLoginForUser_ExpectedLoginCreated()
//        {
//            var userStore = new UserStore(uow);
//            var loginStore = new UserLoginStore(uow);

//            var userId = Guid.NewGuid();
//            string userName = "name";
//            string provider = "local";
//            string key = "key";
            

//            IUser user = new User();
//            user.Id = userId.ToString();
//            user.UserName = userName;

//            var userCreationOperation = userStore.CreateAsync(user, new CancellationToken());
//            userCreationOperation.Result.Success.Should().Be.True();
//            uow.Commit();
//            var u = userStore.FindAsync(userId.ToString(),new CancellationToken());
//            u.Wait();
//            var a = u.Result;

//            var login = loginStore.CreateNewInstance(user.Id, provider, key);
//            var addOperation =loginStore.AddAsync(login, new CancellationToken());
//            addOperation.Result.Success.Should().Be.True();
//            uow.Commit();
           
//        }


//        [TestMethod]
//        public void RetrieveLoginsInfosForUser_ExpectedAllQueryReturns()
//        {
//            var userStore = new UserStore(uow);
//            var loginStore = new UserLoginStore(uow);

//            var userId = Guid.NewGuid();
//            string userName = "name";
//            string provider = "local";
//            string key = "key";


//            IUser user = new User();
//            user.Id = userId.ToString();
//            user.UserName = userName;

//            var userCreationOperation = userStore.CreateAsync(user, new CancellationToken());
//            userCreationOperation.Result.Success.Should().Be.True();
//            uow.Commit();
//            var u = userStore.FindAsync(userId.ToString(), new CancellationToken());
//            u.Wait();
//            var a = u.Result;

//            var login = loginStore.CreateNewInstance(user.Id, provider, key);
//            var addOperation = loginStore.AddAsync(login, new CancellationToken());
//            addOperation.Result.Success.Should().Be.True();
//            uow.Commit();

//            //retrival

//            var retrievedUser = loginStore.GetUserIdAsync(provider, key, new CancellationToken()).Result;
//            retrievedUser.Should().Be.EqualTo(userId.ToString());

//            var retrievedProviderKey = loginStore.GetProviderKeyAsync(userId.ToString(), provider, new CancellationToken()).Result;
//            retrievedProviderKey.Should().Be.EqualTo(key);

//            var retrievedLogins = loginStore.GetLoginsAsync(userId.ToString(), new CancellationToken()).Result;
//            retrievedLogins.Should().Have.Count.EqualTo(1);
//            retrievedLogins.ElementAt(0).LoginProvider.Should().Be(provider);
//            retrievedLogins.ElementAt(0).ProviderKey.Should().Be(key);
//            retrievedLogins.ElementAt(0).UserId.Should().Be(userId.ToString());
//        }


//        [TestMethod]
//        public void RemoveLoginsInfosForUser_ExpectedRemoved()
//        {
//            var userStore = new UserStore(uow);
//            var loginStore = new UserLoginStore(uow);

//            var userId = Guid.NewGuid();
//            string userName = "name";
//            string provider = "local";
//            string key = "key";


//            IUser user = new User();
//            user.Id = userId.ToString();
//            user.UserName = userName;

//            var userCreationOperation = userStore.CreateAsync(user, new CancellationToken());
//            userCreationOperation.Result.Success.Should().Be.True();
//            uow.Commit();
//            var u = userStore.FindAsync(userId.ToString(), new CancellationToken());
//            u.Wait();
//            var a = u.Result;

//            var login = loginStore.CreateNewInstance(user.Id, provider, key);
//            var addOperation = loginStore.AddAsync(login, new CancellationToken());
//            addOperation.Result.Success.Should().Be.True();
//            uow.Commit();

//            //remove

//            var removeOperation = loginStore.RemoveAsync(userId.ToString(), provider, key, new CancellationToken());
//            removeOperation.Wait();
//            removeOperation.Result.Success.Should().Be.True();
//            uow.Commit();

//            var logins = loginStore.GetLoginsAsync(userId.ToString(), new CancellationToken()).Result;
//            logins.Should().Have.Count.EqualTo(0);
//        }

//    }
//}
