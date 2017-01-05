using System.Collections.Generic;
using Mvc6Default.Security.Models;

namespace Mvc6Default.Security.Interfaces
{
    public interface ISecurityBusiness
    {
        ApplicationUser GetUser(string userId = null, string userName = null);
        IList<ApplicationUser> SearchUsers(string firstName = null, string lastName = null, string email = null);
        bool ValidateUser(ApplicationUser user);
        ApplicationUser CreateUser(ApplicationUser user);
        ApplicationUser UpdateUser(ApplicationUser user);
        bool DeleteUser(string userId);

        ApplicationRole GetRole(string roleId = null, string roleName = null);
        IList<ApplicationRole> GetRoles();
        bool ValidateRole(ApplicationRole role);
        ApplicationRole CreateRole(ApplicationRole role);
        ApplicationRole UpdateRole(ApplicationRole role);
        bool DeleteRole(string roleId);
    }
}
