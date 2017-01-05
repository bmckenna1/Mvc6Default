using Mvc6Default.Security.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using Mvc6Default.Security.Interfaces;

namespace Mvc6Default.Security.Services
{
    public class RoleService : IRoleStore<ApplicationRole>
    {
        private readonly ISecurityBusiness _business;

        public RoleService():this(new SecurityBusiness()){}
        public RoleService(ISecurityBusiness business)
        {
            _business = business;
        }

        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return _business.CreateRole(role) == null
                ? Task.FromResult(IdentityResult.Failed(new IdentityError { Code="CreateRoleFailed", Description = "There was a problem saving the new role."}))
                : Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return _business.UpdateRole(role) == null
                ? Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "UpdateRoleFailed", Description = "There was a problem saving the role." }))
                : Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return _business.DeleteRole(role.RoleId)
                ? Task.FromResult(IdentityResult.Success)
                : Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "DeleteRoleFailed", Description = "There was a problem deleting the role." }));
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.RoleId);
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.RoleName);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            role.RoleName = roleName;
            return Task.FromResult(true);
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.RoleName);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.RoleName = normalizedName;
            return Task.FromResult(true);
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_business.GetRole(roleId:roleId));
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_business.GetRole(roleName: normalizedRoleName));
        }
        
        public void Dispose() { }
    }
}
