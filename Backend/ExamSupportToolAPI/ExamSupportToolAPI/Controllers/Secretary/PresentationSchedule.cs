using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.ApplicationRequests.PresentationSchedule;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Models;
using ExamSupportToolAPI.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ExamSupportToolAPI.Controllers.Secretary
{
    [Route("api/secretary/examination-sessions/{id}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class PresentationSchedule : UserBasedController
    {
        private readonly ISecretaryService _secretaryService;
        private readonly IHubContext<SignalRHub> _hub;

        public PresentationSchedule(
            ISecretaryService secretaryService,
            UserManager<ApplicationUser> userManager,
            IHubContext<SignalRHub> hub
            ) : base(userManager)
        {
            _secretaryService = secretaryService;
            _hub = hub;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PresentationScheduleForSecretary))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(PresentationScheduleForSecretary))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns presentation schedule")]
        public async Task<IActionResult> GetPresentationSchedule([FromRoute(Name = "id")] Guid examinationSessionId)
        {
            if (examinationSessionId == Guid.Empty)
                return BadRequest("Guid can't be empty.");
            try
            {
                var userExternalId = GetCurrentUserId();
                if (userExternalId == null)
                    return Unauthorized();

                var result = await _secretaryService.GetPresentationSchedule(examinationSessionId, userExternalId.Value);
                if (result == null)
                    return NotFound();

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

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new presentation schedule")]
        public async Task<IActionResult> GeneratePresentationSchedule([FromRoute(Name = "id")] Guid examinationSessionId, [FromBody] InsertPresentationSchedule insertPresentationSchedule)
        {
            try
            {
                var userExternalId = GetCurrentUserId();
                if (userExternalId == null)
                    return Unauthorized();

                await _secretaryService.GeneratePresentationSchedule(examinationSessionId, userExternalId.Value, insertPresentationSchedule);
                await _hub.Clients.All.SendAsync("onScheduleUpdate");
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Updates the current presentation schedule")]
        public async Task<IActionResult> MovePresentationScheduleEntry([FromRoute(Name = "id")] Guid examinationSessionId, [FromBody] MovePresentationScheduleEntryRequest movePresentationScheduleRequest)
        {
            try
            {
                var userExternalId = GetCurrentUserId();
                if (userExternalId == null)
                    return Unauthorized();

                await _secretaryService.MovePresentationScheduleEntry(examinationSessionId, userExternalId.Value, movePresentationScheduleRequest);
                await _hub.Clients.All.SendAsync("onScheduleUpdate");
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }

        [HttpGet]
        [Route("remaining-students")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentPresentationForSecretary[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns the list with remaining students to present")]
        public async Task<IActionResult> GetRemainingStudentsToPresent([FromRoute(Name = "id")] Guid examinationSessionId)
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var remainingStudents = await _secretaryService.GetRemainingStudentsToPresent(userExternalId.Value, examinationSessionId);

                return Ok(remainingStudents);
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

        [HttpGet]
        [Route("status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns true if the examination session has a schedule.")]
        public async Task<IActionResult> CheckIfExaminationSessionHasSchedule([FromRoute(Name = "id")] Guid examinationSessionId)
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var hasSchedule = await _secretaryService.CheckIfExaminationSessionHasSchedule(userExternalId.Value, examinationSessionId);

                return Ok(hasSchedule);
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
