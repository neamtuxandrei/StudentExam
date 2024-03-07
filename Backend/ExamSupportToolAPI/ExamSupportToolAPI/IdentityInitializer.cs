using Microsoft.AspNetCore.Identity;

namespace ExamSupportToolAPI
{
    public class IdentityInitializer
    {
        private readonly UserManager<IdentityUser>  _userManager;
        public IdentityInitializer(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task InitializeDefaultUser(string username,string password)
        {
            var user = new IdentityUser(username);
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception("Can't create user");
            }

            await _userManager.AddToRoleAsync(user, "admin");
            var userId = user.Id;
        }
    }
}
