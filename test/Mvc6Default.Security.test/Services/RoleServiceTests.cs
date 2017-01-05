using System;
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
    public class RoleServiceTests
    {
        #region CreateAsync

        [TestMethod]
        public async Task CreateAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            ((SecurityBusinessMock)business).RolesToReturn.Add(new ApplicationRole { RoleId = roleId, RoleName = roleName });
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.CreateAsync(new ApplicationRole { RoleName = roleName }, tokenSource.Token);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("CreateRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task CreateAsyncTest_NoData()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.CreateAsync(new ApplicationRole(), tokenSource.Token);
            Assert.AreEqual("CreateRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual("CreateRoleFailed", result.Errors.ToList()[0].Code);
            Assert.AreEqual("There was a problem saving the new role.", result.Errors.ToList()[0].Description);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task CreateAsyncTest_ThrowException()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            ((SecurityBusinessMock)business).RolesToReturn.Add(new ApplicationRole { RoleId = roleId, RoleName = roleName });
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.CreateAsync(new ApplicationRole { RoleName = roleName }, tokenSource.Token);
            Assert.Fail("result is " + result.Succeeded);
        }
        #endregion

        #region UpdateAsync

        [TestMethod]
        public async Task UpdateAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            ((SecurityBusinessMock)business).RolesToReturn.Add(new ApplicationRole { RoleId = roleId, RoleName = roleName });
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.UpdateAsync(new ApplicationRole { RoleName = roleName }, tokenSource.Token);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("UpdateRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task UpdateAsyncTest_NoData()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.UpdateAsync(new ApplicationRole(), tokenSource.Token);
            Assert.AreEqual("UpdateRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual("UpdateRoleFailed", result.Errors.ToList()[0].Code);
            Assert.AreEqual("There was a problem saving the role.", result.Errors.ToList()[0].Description);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task UpdateAsyncTest_ThrowException()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            ((SecurityBusinessMock)business).RolesToReturn.Add(new ApplicationRole { RoleId = roleId, RoleName = roleName });
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.UpdateAsync(new ApplicationRole { RoleName = roleName }, tokenSource.Token);
            Assert.Fail("result is " + result.Succeeded);
        }
        #endregion

        #region DeleteAsync

        [TestMethod]
        public async Task DeleteAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock { DeleteRoleReturnValue = true };
            ((SecurityBusinessMock)business).RolesToReturn.Add(new ApplicationRole { RoleId = roleId, RoleName = roleName });
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.DeleteAsync(new ApplicationRole { RoleName = roleName }, tokenSource.Token);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("DeleteRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task DeleteAsyncTest_DeleteFailsWithoutException()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock { DeleteRoleReturnValue = false };
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.DeleteAsync(new ApplicationRole { RoleId = roleId, RoleName = roleName }, tokenSource.Token);
            Assert.AreEqual("DeleteRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual("DeleteRoleFailed", result.Errors.ToList()[0].Code);
            Assert.AreEqual("There was a problem deleting the role.", result.Errors.ToList()[0].Description);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task DeleteAsyncTest_NullRole()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.DeleteAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result.Succeeded);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task DeleteAsyncTest_ThrowException()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            ((SecurityBusinessMock)business).RolesToReturn.Add(new ApplicationRole { RoleId = roleId, RoleName = roleName });
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            IdentityResult result = await service.DeleteAsync(new ApplicationRole { RoleName = roleName }, tokenSource.Token);
            Assert.Fail("result is " + result.Succeeded);
        }
        #endregion

        #region GetRoleIdAsync

        [TestMethod]
        public async Task GetRoleIdAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetRoleIdAsync(new ApplicationRole { RoleId = roleId, RoleName = roleName }, tokenSource.Token);
            Assert.AreEqual(roleId, result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task GetRoleIdAsyncTest_NullRole()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetRoleIdAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result);
        }
        #endregion

        #region GetRoleNameAsync

        [TestMethod]
        public async Task GetRoleNameAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetRoleNameAsync(new ApplicationRole { RoleId = roleId, RoleName = roleName }, tokenSource.Token);
            Assert.AreEqual(roleName, result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task GetRoleNameAsyncTest_NullRole()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetRoleNameAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result);
        }
        #endregion

        #region SetRoleNameAsync

        [TestMethod]
        public async Task SetRoleNameAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            string newRoleName = "The New Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationRole role = new ApplicationRole { RoleId = roleId, RoleName = roleName };
            await service.SetRoleNameAsync(role, newRoleName, tokenSource.Token);
            Assert.AreEqual(newRoleName, role.RoleName);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task SetRoleNameAsyncTest_NullRole()
        {
            string newRoleName = "The New Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            await service.SetRoleNameAsync(null, newRoleName, tokenSource.Token);
            Assert.Fail();
        }
        #endregion

        #region GetNormalizedRoleNameAsync

        [TestMethod]
        public async Task GetNormalizedRoleNameAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetNormalizedRoleNameAsync(new ApplicationRole { RoleId = roleId, RoleName = roleName }, tokenSource.Token);
            Assert.AreEqual(roleName, result);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task GetNormalizedRoleNameAsyncTest_NullRole()
        {
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            string result = await service.GetNormalizedRoleNameAsync(null, tokenSource.Token);
            Assert.Fail("result is " + result);
        }
        #endregion

        #region SetNormalizedRoleNameAsync

        [TestMethod]
        public async Task SetNormalizedRoleNameAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            string normalizedRoleName = "The New Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationRole role = new ApplicationRole { RoleId = roleId, RoleName = roleName };
            await service.SetNormalizedRoleNameAsync(role, normalizedRoleName, tokenSource.Token);
            Assert.AreEqual(normalizedRoleName, role.RoleName);
            Assert.AreEqual(0, ((SecurityBusinessMock)business).MethodsCalled.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), AllowDerivedTypes = false)]
        public async Task SetNormalizedRoleNameAsyncTest_NullRole()
        {
            string normalizedRoleName = "The New Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            await service.SetNormalizedRoleNameAsync(null, normalizedRoleName, tokenSource.Token);
            Assert.Fail();
        }
        #endregion

        #region FindByIdAsync

        [TestMethod]
        public async Task FindByIdAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            ((SecurityBusinessMock)business).RolesToReturn.Add(new ApplicationRole { RoleId = roleId, RoleName = roleName });
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationRole result = await service.FindByIdAsync(roleId, tokenSource.Token);
            Assert.AreEqual(roleId, result.RoleId);
            Assert.AreEqual(roleName, result.RoleName);
            Assert.AreEqual("GetRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task FindByIdAsyncTest_NoData()
        {
            string roleId = "123455";
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationRole result = await service.FindByIdAsync(roleId, tokenSource.Token);
            Assert.IsNull(result);
            Assert.AreEqual("GetRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task FindByIdAsyncTest_ThrowException()
        {
            string roleId = "123455";
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationRole result = await service.FindByIdAsync(roleId, tokenSource.Token);
            Assert.Fail("result is " + result.RoleId + " | " + result.RoleName);
        }
        #endregion

        #region FindByNameAsync

        [TestMethod]
        public async Task FindByNameAsyncTest_HappyPath()
        {
            string roleId = "123455";
            string roleName = "The Role Name";
            ISecurityBusiness business = new SecurityBusinessMock();
            ((SecurityBusinessMock)business).RolesToReturn.Add(new ApplicationRole { RoleId = roleId, RoleName = roleName });
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationRole result = await service.FindByNameAsync(roleName, tokenSource.Token);
            Assert.AreEqual(roleId, result.RoleId);
            Assert.AreEqual(roleName, result.RoleName);
            Assert.AreEqual("GetRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        public async Task FindByNameAsyncTest_NoData()
        {
            string roleName = "TheRole";
            ISecurityBusiness business = new SecurityBusinessMock();
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationRole result = await service.FindByNameAsync(roleName, tokenSource.Token);
            Assert.IsNull(result);
            Assert.AreEqual("GetRole", ((SecurityBusinessMock)business).MethodsCalled[0]);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public async Task FindByNameAsyncTest_ThrowException()
        {
            string roleName = "TheRole";
            ISecurityBusiness business = new SecurityBusinessMock { ExceptionToThrow = new Exception("This is my Exception from CreateAsyncTest_ThrowException") };
            RoleService service = new RoleService(business);

            var tokenSource = new CancellationTokenSource();
            ApplicationRole result = await service.FindByNameAsync(roleName, tokenSource.Token);
            Assert.Fail("result is " + result.RoleId + " | " + result.RoleName);
        } 
        #endregion
    }
}
