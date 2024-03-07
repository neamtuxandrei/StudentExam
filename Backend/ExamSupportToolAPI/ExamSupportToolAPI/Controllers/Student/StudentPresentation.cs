using ExamSupportToolAPI.ApplicationServices;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain.Enums;
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
    public class StudentPresentation : UserBasedController
    {
        private readonly IStudentService _studentService;

        public StudentPresentation(
            UserManager<ApplicationUser> _userManager,
            IStudentService studentService)
            : base(_userManager)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentPresentationForStudent))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns student presentation details for student")]
        public async Task<IActionResult> GetStudentPresentation()
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var studentPresentation = await _studentService.GetStudentPresentation(userExternalId.Value);
                return Ok(studentPresentation);
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
