using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.Models;
using ExamSupportToolAPI.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OpenIddict.Validation.AspNetCore;

namespace ExamSupportToolAPI.Controllers.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class ExaminationTicket : UserBasedController
    {
        private readonly IStudentService _studentService;
        private readonly IHubContext<SignalRHub> _hub;

        public ExaminationTicket(UserManager<ApplicationUser> _userManager,
                                 IStudentService studentService,
                                 IHubContext<SignalRHub> hub)
        : base(_userManager)
        {
            _studentService = studentService;
            _hub = hub;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateExaminationTicket()
        {
            var userExternalId = GetCurrentUserId();

            if (userExternalId == null)
            {
                return Unauthorized();
            }

            try
            {
                var generatedTicket = await _studentService.GenerateExaminationTicket(userExternalId.Value);
                await _hub.Clients.All.SendAsync("onTicketDrawn");
                return Ok(generatedTicket);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ErrorResult() { Description = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
