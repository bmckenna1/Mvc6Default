using System;
using System.Collections.Generic;
using System.Linq;
using Mvc6Default.Security.Interfaces;
using Mvc6Default.Security.Models;

namespace Mvc6Default.Security
{
    public class SecurityDataFake : ISecurityData
    {
        private static readonly List<ApplicationUser> Users = new List<ApplicationUser>();
        private static readonly List<ApplicationRole> Roles = new List<ApplicationRole>();
        private static int _nextUserId = 1000;
        private static int _nextRoleId = 1;
        public ApplicationUser GetUserById(int userId)
        {
            return GetCleanUser(Users.FirstOrDefault(u => u.UserId == userId.ToString()));
        }

        public ApplicationUser GetUserByUserName(string userName)
        {
            return GetCleanUser(Users.FirstOrDefault(u => u.UserName == userName));
        }

        public IList<ApplicationUser> SearchUsers(string firstName = null, string lastName = null, string email = null)
        {
            //todo: implement first and last names in ApplicationUser, and include in search
            return Users.Where(u => u.Email == email).Select(GetCleanUser).ToList();
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            int userId = _nextUserId;
            _nextUserId++;
            user.UserId = userId.ToString();
            Users.Add(GetCleanUser(user));
            return GetUserById(userId);
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            int userId = int.Parse(user.UserId);
            ApplicationUser existingUser = Users.FirstOrDefault(u => u.UserId == user.UserId);
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.TwoFactorEnabled = user.TwoFactorEnabled;

            return GetUserById(userId);
        }

        public bool DeleteUser(int userId)
        {
            ApplicationUser existingUser = Users.FirstOrDefault(u => u.UserId == userId.ToString());
            if (existingUser == null) return false;
            Users.Remove(existingUser);
            return true;
        }

        public ApplicationRole GetRoleById(int roleId)
        {
            return GetCleanRole(Roles.FirstOrDefault(r => r.RoleId == roleId.ToString()));
        }

        public ApplicationRole GetRoleByName(string roleName)
        {
            return GetCleanRole(Roles.FirstOrDefault(r => r.RoleName == roleName));
        }

        public IList<ApplicationRole> GetRoles()
        {
            return Roles.Select(GetCleanRole).ToList();
        }

        public ApplicationRole CreateRole(ApplicationRole role)
        {
            int roleId = _nextRoleId;
            _nextRoleId++;
            role.RoleId = roleId.ToString();
            Roles.Add(GetCleanRole(role));
            return GetRoleById(roleId);
        }

        public ApplicationRole UpdateRole(ApplicationRole role)
        {
            int roleId = int.Parse(role.RoleId);
            ApplicationRole existingRole = Roles.FirstOrDefault(r => r.RoleId == role.RoleId);
            existingRole.RoleName = role.RoleName;
            return GetRoleById(roleId);
        }

        public bool DeleteRole(int roleId)
        {
            ApplicationRole existingRole = Roles.FirstOrDefault(r => r.RoleId == roleId.ToString());
            if (existingRole == null) return false;
            Roles.Remove(existingRole);
            return true;
        }

        private ApplicationUser GetCleanUser(ApplicationUser user)
        {
            return user == null ? null : new ApplicationUser
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                PasswordHash = user.PasswordHash,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
        }

        private ApplicationRole GetCleanRole(ApplicationRole role)
        {
            return role == null ? null : new ApplicationRole
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }
    }
}
