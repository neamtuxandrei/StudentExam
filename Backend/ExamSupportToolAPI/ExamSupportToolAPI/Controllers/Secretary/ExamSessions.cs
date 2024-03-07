using ExamSupportToolAPI.ApplicationRequests.ExaminationSession;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain.Enums;
using ExamSupportToolAPI.Models;
using ExamSupportToolAPI.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ExamSupportToolAPI.Controllers.Secretary
{
    [Route("api/secretary/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class ExamSessions : UserBasedController
    {
        private readonly ISecretaryService _secretaryService;
        private readonly IHubContext<SignalRHub> _hub;

        public ExamSessions(
            UserManager<ApplicationUser> _userManager,
            ISecretaryService secretaryService,
            IHubContext<SignalRHub> hub)
            : base(_userManager)
        {
            _secretaryService = secretaryService;
            _hub = hub;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExaminationSessionForSecretary))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns examination session by id")]
        public async Task<IActionResult> GetExaminationSession(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var examinationSession = await _secretaryService.GetExaminationSession(userExternalId.Value, id);
                return Ok(examinationSession);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExaminationSessionListForSecretary))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns the list of examination sessions")]
        public async Task<IActionResult> GetExaminationSessions()
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var examinationSessions = await _secretaryService.GetExaminationSessions(userExternalId.Value);

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

        [HttpGet]
        [Route("{id}/presentation")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns examination session details for presentation by session id")]
        public async Task<IActionResult> GetExaminationSessionForPresentation(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var examinationSession = await _secretaryService.GetExaminationSessionForPresentation(userExternalId.Value, id);
                return Ok(examinationSession);
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

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExaminationSessionForSecretary))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Sets new status to an examination session")]
        public async Task<IActionResult> SetExaminationSessionStatus(Guid id, [FromBody] SessionStatus status)
        {
            if (id == Guid.Empty)
                return BadRequest();

            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                await _secretaryService.SetExaminationSessionStatus(userExternalId.Value, id, status);

                if (status == SessionStatus.Presenting)
                {
                    await _secretaryService.GenerateStudentPresentation(id, userExternalId.Value);
                    await _hub.Clients.All.SendAsync("onPresentationStart");
                }
                else if (status == SessionStatus.Closed)
                {
                    await _secretaryService.PublishGrades(userExternalId.Value, id);
                    await _hub.Clients.All.SendAsync("onSessionStop");
                }

                return Ok();

            }
            catch (InvalidOperationException)
            {
                return NotFound(new ErrorResult() { Description = "Could not find the information associated with the curent user" });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Adds new examination session")]
        public async Task<IActionResult> InsertExaminationSession([FromBody] InsertExaminationSessionRequest request)
        {
            if (request is null) throw new Exception($"Request {typeof(InsertExaminationSessionRequest)} is null");
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                await _secretaryService.AddExaminationSession(userExternalId.Value, request);
                return Ok();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new ErrorResult() { Description = "The secretary member doesn't exist." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }
    }
}
