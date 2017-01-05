using System.Collections.Generic;
using Mvc6Default.Security.Models;

namespace Mvc6Default.Security.Interfaces
{
    public interface ISecurityData
    {
        ApplicationUser GetUserById(int userId);
        ApplicationUser GetUserByUserName(string userName);
        IList<ApplicationUser> SearchUsers(string firstName = null, string lastName = null, string email = null);
        ApplicationUser CreateUser(ApplicationUser user);
        ApplicationUser UpdateUser(ApplicationUser user);
        bool DeleteUser(int userId);
        ApplicationRole GetRoleById(int roleId);
        ApplicationRole GetRoleByName(string roleName);
        IList<ApplicationRole> GetRoles();
        ApplicationRole CreateRole(ApplicationRole role);
        ApplicationRole UpdateRole(ApplicationRole role);
        bool DeleteRole(int roleId);
    }
}
