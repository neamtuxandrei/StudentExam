using ExamSupportToolAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamSupportToolAPI.Controllers
{
    public class UserBasedController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;

        public UserBasedController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        protected Guid? GetCurrentUserId()
        {
            var userId = userManager.GetUserId(User);
            Guid userIdGuid;
            if (Guid.TryParse(userId, out userIdGuid))
                return userIdGuid;

            return null;
        }
    }
}
