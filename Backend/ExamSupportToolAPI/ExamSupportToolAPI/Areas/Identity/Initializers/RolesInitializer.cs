using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ExamSupportToolAPI.Areas.Identity.Initializers
{

    public class RolesInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesInitializer(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        private async Task CreateRoleIfDoesntExist(string roleName)
        {
            if(!await _roleManager.RoleExistsAsync(roleName))
            { 
                var result  = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if(!result.Succeeded)
                {
                    throw new Exception("Failed to create role");
                }
            }
        }

        public async Task CreateRolesThatDontExist()
        {
            await CreateRoleIfDoesntExist("Admin");
            await CreateRoleIfDoesntExist("Secretary");
            await CreateRoleIfDoesntExist("Committee");
            await CreateRoleIfDoesntExist("Student");
        }

    }
}

