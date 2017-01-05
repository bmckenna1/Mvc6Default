using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mvc6Default.Security.Interfaces;
using Mvc6Default.Security.Models;
using Mvc6Default.Security.Services;
using Mvc6Default.Security.test.Mocks;

namespace Mvc6Default.Security.test.Services
{
    [TestClass]
    public class UserServiceTests
    {
        #region set up
        readonly string _userId = "123455";
        readonly string _userName = "The User Name";
        readonly string _email = "a@b.com";
        readonly string _phoneNumber = "303-555-5555";
        readonly bool _phoneNumberConfirmed = true;
        readonly string _passwordHash = "zdhvlghjhHBFV354Ckjbc";
        readonly bool _twoFactorEnabled = true;

        private ApplicationUser GetUserObj(bool includeId)
        {
            return new ApplicationUser
            {
                UserId = includeId ? _userId : null,
                UserName = _userName,
                Email = _email,
                PhoneNumber = _phoneNumber,
                PhoneNumberConfirmed = _phoneNumberConfirmed,
                PasswordHash = _passwordHash,
                TwoFactorEnabled = _twoFactorEnabled
            };
        }
        private bool AreEqual(ApplicationUser user1, ApplicationUser user2)
        {
            return user1.UserId == user2.UserId
                   && user1.UserName == user2.UserName
                   && user1.Email == user2.Email
                   && user1.PhoneNumber == user2.PhoneNumber
                   && user1.PhoneNumberConfirmed == user2.PhoneNumberConfirmed
                   && user1.PasswordHash == user2.PasswordHash
                   && user1.TwoFactorEnabled == user2.TwoFactorEnabled;
        }
        #endregion

        #region CreateAsync

        [TestMethod]
        public async Task CreateAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            ((SecurityBusinessMock)business).UsersToReturn.Add(GetUserObj(true));
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.CreateAsync(GetUserObj(false), tokenSource.Token);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("CreateUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task CreateAsyncTest_NoData()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.CreateAsync(new ApplicationUser(), tokenSource.Token);
            Assert.AreEqual("CreateUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual("CreateUserFailed", result.Errors.ToList()[0].Code);
            Assert.AreEqual("There was a problem saving the new user.", result.Errors.ToList()[0].Description);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task CreateAsyncTest_ThrowException()
        {
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            ((SecurityBusinessMock)business).UsersToReturn.Add(GetUserObj(true));
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.CreateAsync(GetUserObj(false), tokenSource.Token);
            Assert.Fail("result is " + result.Succeeded);
        }
        #endregion

        #region UpdateAsync

        [TestMethod]
        public async Task UpdateAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            ((SecurityBusinessMock)business).UsersToReturn.Add(GetUserObj(true));
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.UpdateAsync(GetUserObj(true), tokenSource.Token);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("UpdateUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task UpdateAsyncTest_NoData()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.UpdateAsync(new ApplicationUser(), tokenSource.Token);
            Assert.AreEqual("UpdateUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual("UpdateUserFailed", result.Errors.ToList()[0].Code);
            Assert.AreEqual("There was a problem saving the user.", result.Errors.ToList()[0].Description);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task UpdateAsyncTest_ThrowException()
        {
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            ((SecurityBusinessMock)business).UsersToReturn.Add(GetUserObj(true));
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.UpdateAsync(GetUserObj(true), tokenSource.Token);
            Assert.Fail("result is " + result.Succeeded);
        }
        #endregion

        #region DeleteAsync

        [TestMethod]
        public async Task DeleteAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock { DeleteUserReturnValue = true };
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.DeleteAsync(GetUserObj(true), tokenSource.Token);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("DeleteUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task DeleteAsyncTest_DeleteFailsWithoutException()
        {
            ISecurityBusiness business = new SecurityBusinessMock { DeleteUserReturnValue = false };
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.DeleteAsync(GetUserObj(true), tokenSource.Token);
            Assert.AreEqual("DeleteUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual("DeleteUserFailed", result.Errors.ToList()[0].Code);
            Assert.AreEqual("There was a problem deleting the user.", result.Errors.ToList()[0].Description);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task DeleteAsyncTest_NullUser()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.DeleteAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result.Succeeded);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task DeleteAsyncTest_ThrowException()
        {
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            ((SecurityBusinessMock)business).UsersToReturn.Add(GetUserObj(true));
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.DeleteAsync(GetUserObj(false), tokenSource.Token);
            Assert.Fail("result is " + result.Succeeded);
        }
        #endregion

        #region GetUserIdAsync

        [TestMethod]
        public async Task GetUserIdAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetUserIdAsync(GetUserObj(true), tokenSource.Token);
            Assert.AreEqual(_userId, result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task GetUserIdAsyncTest_NullUser()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetUserIdAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result);
        }
        #endregion

        #region GetUserNameAsync

        [TestMethod]
        public async Task GetUserNameAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetUserNameAsync(GetUserObj(true), tokenSource.Token);
            Assert.AreEqual(_userName, result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task GetUserNameAsyncTest_NullUser()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetUserNameAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result);
        }
        #endregion

        #region SetUserNameAsync

        [TestMethod]
        public async Task SetUserNameAsyncTest_HappyPath()
        {
            string newUserName = "The New User Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser user = GetUserObj(true);
            await service.SetUserNameAsync(user, newUserName, tokenSource.Token);
            Assert.AreEqual(newUserName, user.UserName);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task SetUserNameAsyncTest_NullUser()
        {
            string newUserName = "The New User Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            await service.SetUserNameAsync(null, newUserName, tokenSource.Token);
            Assert.Fail();
        }
        #endregion

        #region GetNormalizedUserNameAsync

        [TestMethod]
        public async Task GetNormalizedUserNameAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetNormalizedUserNameAsync(GetUserObj(true), tokenSource.Token);
            Assert.AreEqual(_userName, result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task GetNormalizedUserNameAsyncTest_NullUser()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetNormalizedUserNameAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result);
        }
        #endregion

        #region SetNormalizedUserNameAsync

        [TestMethod]
        public async Task SetNormalizedUserNameAsyncTest_HappyPath()
        {
            string normalizedUserName = "The New User Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser user = GetUserObj(true);
            await service.SetNormalizedUserNameAsync(user, normalizedUserName, tokenSource.Token);
            Assert.AreEqual(normalizedUserName, user.UserName);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task SetNormalizedUserNameAsyncTest_NullUser()
        {
            string normalizedUserName = "The New User Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            await service.SetNormalizedUserNameAsync(null, normalizedUserName, tokenSource.Token);
            Assert.Fail();
        }
        #endregion

        #region FindByIdAsync

        [TestMethod]
        public async Task FindByIdAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            ((SecurityBusinessMock)business).UsersToReturn.Add(GetUserObj(true));
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser result = await service.FindByIdAsync(_userId, tokenSource.Token);
            Assert.IsTrue(AreEqual(GetUserObj(true), result));
            Assert.AreEqual("GetUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task FindByIdAsyncTest_NoData()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser result = await service.FindByIdAsync(_userId, tokenSource.Token);
            Assert.IsNull(result);
            Assert.AreEqual("GetUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task FindByIdAsyncTest_ThrowException()
        {
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser result = await service.FindByIdAsync(_userId, tokenSource.Token);
            Assert.Fail("result is " + result.UserId + " | " + result.UserName);
        }
        #endregion

        #region FindByNameAsync

        [TestMethod]
        public async Task FindByNameAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            ((SecurityBusinessMock)business).UsersToReturn.Add(GetUserObj(true));
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser result = await service.FindByNameAsync(_userName, tokenSource.Token);
            Assert.IsTrue(AreEqual(GetUserObj(true), result));
            Assert.AreEqual("GetUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task FindByNameAsyncTest_NoData()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser result = await service.FindByNameAsync(_userName, tokenSource.Token);
            Assert.IsNull(result);
            Assert.AreEqual("GetUser", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task FindByNameAsyncTest_ThrowException()
        {
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser result = await service.FindByNameAsync(_userName, tokenSource.Token);
            Assert.Fail("result is " + result.UserId + " | " + result.UserName);
        }
        #endregion
        
        #region GetPasswordHashAsync

        [TestMethod]
        public async Task GetPasswordHashAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetPasswordHashAsync(GetUserObj(true), tokenSource.Token);
            Assert.AreEqual(_passwordHash, result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task GetPasswordHashAsyncTest_NullUser()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetPasswordHashAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result);
        }
        #endregion

        #region SetPasswordHashAsync

        [TestMethod]
        public async Task SetPasswordHashAsyncTest_HappyPath()
        {
            string passwordHash = "The New User Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser user = GetUserObj(true);
            await service.SetPasswordHashAsync(user, passwordHash, tokenSource.Token);
            Assert.AreEqual(passwordHash, user.PasswordHash);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task SetPasswordHashAsyncTest_NullUser()
        {
            string passwordHash = "The New User Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            await service.SetPasswordHashAsync(null, passwordHash, tokenSource.Token);
            Assert.Fail();
        }
        #endregion

        #region GetUserNameAsync

        [TestMethod]
        public async Task HasPasswordAsyncTest_HappyPathTrue()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            bool result = await service.HasPasswordAsync(GetUserObj(true), tokenSource.Token);
            Assert.AreEqual(true, result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        public async Task HasPasswordAsyncTest_HappyPathFalse()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser user = GetUserObj(true);
            user.PasswordHash = string.Empty;
            bool result = await service.HasPasswordAsync(user, tokenSource.Token);
            Assert.IsFalse(result);
            user.PasswordHash = " ";
            result = await service.HasPasswordAsync(user, tokenSource.Token);
            Assert.IsFalse(result);
            user.PasswordHash = null;
            result = await service.HasPasswordAsync(user, tokenSource.Token);
            Assert.IsFalse(result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task HasPasswordAsyncTest_NullUser()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            bool result = await service.HasPasswordAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result);
        }
        #endregion

        #region Not implemented

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), AllowDerivedTypes = false)]
        public async Task AddLoginAsyncTest_NotImplemented()
        {
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            await service.AddLoginAsync(null, null, tokenSource.Token);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), AllowDerivedTypes = false)]
        public async Task RemoveLoginAsyncTest_NotImplemented()
        {
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            await service.RemoveLoginAsync(null, null, null, tokenSource.Token);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), AllowDerivedTypes = false)]
        public async Task GetLoginsAsyncTest_NotImplemented()
        {
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            IList<UserLoginInfo> result = await service.GetLoginsAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), AllowDerivedTypes = false)]
        public async Task FindByLoginAsyncTest_NotImplemented()
        {
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser result = await service.FindByLoginAsync(null, null, tokenSource.Token);
            Assert.Fail("result is " + result.UserName);
        }

        #endregion
        
        #region GetPhoneNumberAsync

        [TestMethod]
        public async Task GetPhoneNumberAsyncTest_HappyPath()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetPhoneNumberAsync(GetUserObj(true), tokenSource.Token);
            Assert.AreEqual(_phoneNumber, result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task GetPhoneNumberAsyncTest_NullUser()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetPhoneNumberAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result);
        }
        #endregion

        #region SetPasswordHashAsync

        [TestMethod]
        public async Task SetPhoneNumberAsyncTest_HappyPath()
        {
            string phoneNumber = "719-555-1234";
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationUser user = GetUserObj(true);
            await service.SetPhoneNumberAsync(user, phoneNumber, tokenSource.Token);
            Assert.AreEqual(phoneNumber, user.PhoneNumber);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task SetPhoneNumberAsyncTest_NullUser()
        {
            string phoneNumber = "719-555-1234";
            ISecurityBusiness business = new SecurityBusinessMock();
            UserService service = new UserService(business);

            var tokenSource = new CancellationTokenSource();
            await service.SetPhoneNumberAsync(null, phoneNumber, tokenSource.Token);
            Assert.Fail();
        }
        #endregion
 
        //GetPhoneNumberConfirmedAsync
        //SetPhoneNumberConfirmedAsync
        //SetTwoFactorEnabledAsync
        //GetTwoFactorEnabledAsync
    }
}
