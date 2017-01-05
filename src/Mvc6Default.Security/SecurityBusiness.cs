using System.Collections.Generic;
using Mvc6Default.Security.Interfaces;
using Mvc6Default.Security.Models;

namespace Mvc6Default.Security
{
    public class SecurityBusiness : ISecurityBusiness
    {
        private readonly ISecurityData _data;

        public SecurityBusiness():this(new SecurityDataFake()){}
        public SecurityBusiness(ISecurityData data)
        {
            _data = data;
        }

        public ApplicationUser GetUser(string userId = null, string userName = null)
        {
            if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(userName)) return null;

            int uId;
            return int.TryParse(userId, out uId) ? _data.GetUserById(uId) : _data.GetUserByUserName(userName);
        }

        public IList<ApplicationUser> SearchUsers(string firstName = null, string lastName = null, string email = null)
        {
            return (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(email))
                ? new List<ApplicationUser>() 
                : _data.SearchUsers(firstName, lastName, email);
        }

        public bool ValidateUser(ApplicationUser user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName)) return false;
            if (string.IsNullOrWhiteSpace(user.Email)) return false;
            if (string.IsNullOrWhiteSpace(user.PasswordHash)) return false;

            return true;
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            int uId;
            return ValidateUser(user) &&
                   (string.IsNullOrWhiteSpace(user.UserId) || (int.TryParse(user.UserId, out uId) && uId <= 0))
                ? _data.CreateUser(user)
                : null;
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            int uId;
            return ValidateUser(user) &&
                   !string.IsNullOrWhiteSpace(user.UserId) && int.TryParse(user.UserId, out uId)
                ? _data.UpdateUser(user)
                : null;
        }

        public bool DeleteUser(string userId)
        {
            int uId;
            return int.TryParse(userId, out uId) && _data.DeleteUser(uId);
        }

        public ApplicationRole GetRole(string roleId = null, string roleName = null)
        {
            if (string.IsNullOrWhiteSpace(roleId) && string.IsNullOrWhiteSpace(roleName)) return null;

            int rId;
            return int.TryParse(roleId, out rId) ? _data.GetRoleById(rId) : _data.GetRoleByName(roleName);
        }

        public IList<ApplicationRole> GetRoles()
        {
            return _data.GetRoles();
        }

        public bool ValidateRole(ApplicationRole role)
        {
            return !string.IsNullOrWhiteSpace(role.RoleName);
        }

        public ApplicationRole CreateRole(ApplicationRole role)
        {
            int rId;
            return ValidateRole(role) &&
                   (string.IsNullOrWhiteSpace(role.RoleId) || (int.TryParse(role.RoleId, out rId) && rId <= 0)) &&
                   _data.GetRoleByName(role.RoleName) == null
                ? _data.CreateRole(role)
                : null;
        }

        public ApplicationRole UpdateRole(ApplicationRole role)
        {
            int rId;
            return ValidateRole(role) &&
                   !string.IsNullOrWhiteSpace(role.RoleId) && int.TryParse(role.RoleId, out rId)
                ? _data.UpdateRole(role)
                : null;
        }

        public bool DeleteRole(string roleId)
        {
            int rId;
            return int.TryParse(roleId, out rId) && _data.DeleteRole(rId);
        }
    }
}
