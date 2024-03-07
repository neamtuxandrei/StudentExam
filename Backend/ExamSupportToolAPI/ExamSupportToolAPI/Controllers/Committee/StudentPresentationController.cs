using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ExamSupportToolAPI.Controllers.Committee
{
    [Route("api/committee/examination-sessions/{id}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class StudentPresentationController : UserBasedController
    {
        private readonly ICommitteeService _committeeService;
        public StudentPresentationController(UserManager<ApplicationUser> userManager, ICommitteeService committeeService) : base(userManager)
        {
            _committeeService = committeeService;
        }

        [HttpGet("list")]
        [SwaggerOperation(Summary = "Returns student presentations for committee")]
        public async Task<IActionResult> GetStudentPresentations(Guid id)
        {
            var userExternalId = GetCurrentUserId();
            if (userExternalId == null)
                return Unauthorized();
            try
            {
                return Ok(await _committeeService.GetStudentPresentations(userExternalId.Value, id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Returns the current presenting student for committee")]
        public async Task<IActionResult> GetCurrentPresentingStudent(Guid id)
        {
            var userExternalId = GetCurrentUserId();
            if (userExternalId == null)
                return Unauthorized();
            try
            {
                return Ok(await _committeeService.GetCurrentPresentingStudent(userExternalId.Value, id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
