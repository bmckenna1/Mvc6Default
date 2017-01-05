using System;
using System.Collections.Generic;
using Mvc6Default.Security.Interfaces;
using Mvc6Default.Security.Models;

namespace Mvc6Default.Security.test.Mocks
{
    public class SecurityBusinessMock : ISecurityBusiness
    {
        public IList<string> MethodsCalled { get; } = new List<string>();
        public IList<ApplicationUser> UsersToReturn { get; } = new List<ApplicationUser>();
        public Exception ExceptionToThrow { get; set; }

        private ApplicationUser ReturnUser(string methodName)
        {
            MethodsCalled.Add(methodName);
            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }
            ApplicationUser user = UsersToReturn.Count > 0 ? UsersToReturn[0] : null;
            UsersToReturn.Remove(user);
            return user;
        }
        private IList<ApplicationUser> ReturnUsers(string methodName, int count)
        {
            MethodsCalled.Add(methodName);
            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }
            IList<ApplicationUser> users = new List<ApplicationUser>();
            for (int i = 0; i < count; i++)
            {
                users.Add(UsersToReturn[i]);
            }

            foreach (ApplicationUser user in users)
            {
                UsersToReturn.Remove(user);
            }
            
            return users;
        }

        public ApplicationUser GetUser(string userId = null, string userName = null)
        {
            return ReturnUser("GetUser");
        }

        public IList<ApplicationUser> SearchUsers(string firstName = null, string lastName = null, string email = null)
        {
            return ReturnUsers("SearchUsers", 2);
        }

        public bool ValidateUserReturnValue { get; set; }
        public bool ValidateUser(ApplicationUser user)
        {
            MethodsCalled.Add("ValidateUser");
            return ValidateUserReturnValue;
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            return ReturnUser("CreateUser");
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            return ReturnUser("UpdateUser");
        }

        public bool DeleteUserReturnValue { get; set; }
        public bool DeleteUser(string userId)
        {
            MethodsCalled.Add("DeleteUser");
            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }
            return DeleteUserReturnValue;
        }
        public IList<ApplicationRole> RolesToReturn { get; } = new List<ApplicationRole>();

        private ApplicationRole ReturnRole(string methodName)
        {
            MethodsCalled.Add(methodName);
            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }
            ApplicationRole role = RolesToReturn.Count > 0 ? RolesToReturn[0] : null;
            RolesToReturn.Remove(role);
            return role;
        }
        private IList<ApplicationRole> ReturnRoles(string methodName, int count)
        {
            MethodsCalled.Add(methodName);
            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }
            IList<ApplicationRole> roles = new List<ApplicationRole>();
            for (int i = 0; i < count; i++)
            {
                roles.Add(RolesToReturn[i]);
            }

            foreach (ApplicationRole role in roles)
            {
                RolesToReturn.Remove(role);
            }

            return roles;
        }

        public ApplicationRole GetRole(string roleId = null, string roleName = null)
        {
            return ReturnRole("GetRole");
        }

        public IList<ApplicationRole> GetRoles()
        {
            return ReturnRoles("GetRoles", 2);
        }

        public bool ValidateRoleReturnValue { get; set; }
        public bool ValidateRole(ApplicationRole role)
        {
            MethodsCalled.Add("ValidateRole");
            return ValidateRoleReturnValue;
        }

        public ApplicationRole CreateRole(ApplicationRole role)
        {
            return ReturnRole("CreateRole");
        }

        public ApplicationRole UpdateRole(ApplicationRole role)
        {
            return ReturnRole("UpdateRole");
        }

        public bool DeleteRoleReturnValue { get; set; }
        public bool DeleteRole(string roleId)
        {
            MethodsCalled.Add("DeleteRole");
            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }
            return DeleteRoleReturnValue;
        }
    }
}
