using ExamSupportToolAPI.ApplicationRequests.ExaminationSession;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain.Enums;
using ExamSupportToolAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ExamSupportToolAPI.Controllers.Committee
{
    [Route("api/committee/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class ExaminationSession : UserBasedController
    {
        private readonly ICommitteeService _committeeService;

        public ExaminationSession(
            UserManager<ApplicationUser> _userManager,
            ICommitteeService committeeService)
            : base(_userManager)
        {
            _committeeService = committeeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExaminationSessionListForCommittee))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns the list of examination sessions for current committee member")]
        public async Task<IActionResult> GetExaminationSessions()
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var examinationSessions = await _committeeService.GetExaminationSessions(userExternalId.Value);

                return Ok(examinationSessions);
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
