using ExamSupportToolAPI.ApplicationServices;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Models;
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
    public class PresentationSchedule : UserBasedController
    {
        private readonly ICommitteeService _committeeService;

        public PresentationSchedule(
            UserManager<ApplicationUser> _userManager,
            ICommitteeService committeeService)
            : base(_userManager)
        {
            _committeeService = committeeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PresentationScheduleForCommittee))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(PresentationScheduleForCommittee))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns presentation schedule for committee")]
        public async Task<IActionResult> GetPresentationSchedule(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Guid can't be empty.");
            try
            {
                var userExternalId = GetCurrentUserId();
                if (userExternalId == null)
                    return Unauthorized();

                var result = await _committeeService.GetPresentationSchedule(id, userExternalId.Value);

                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ErrorResult() { Description = "There is no presentation schedule for this examination session." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }


    }
}

