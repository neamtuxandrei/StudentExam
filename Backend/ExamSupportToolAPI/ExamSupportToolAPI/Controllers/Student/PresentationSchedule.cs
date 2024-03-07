using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ExamSupportToolAPI.Controllers.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class PresentationSchedule : UserBasedController
    {
        private readonly IStudentService _studentService;
        public PresentationSchedule(
            UserManager<ApplicationUser> userManager,
            IStudentService studentService)
            : base(userManager)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PresentationScheduleEntryForStudent))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns current presentation schedule entry for current student")]
        public async Task<IActionResult> GetPresentationSchedule()
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var presentationSchedule = await _studentService.GetSchedule(userExternalId.Value);
                return Ok(presentationSchedule);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ErrorResult() { Description = "Could not find the information associated with the curent user" });

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }
    }
}
