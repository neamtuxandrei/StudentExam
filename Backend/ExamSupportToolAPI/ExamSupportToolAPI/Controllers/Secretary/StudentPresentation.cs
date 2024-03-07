using ExamSupportToolAPI.ApplicationRequests.StudentPresentation;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Models;
using ExamSupportToolAPI.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class StudentPresentation : UserBasedController
    {
        private readonly ISecretaryService _secretaryService;
        private readonly IHubContext<SignalRHub> _hub;

        public StudentPresentation(
            UserManager<ApplicationUser> _userManager,
            ISecretaryService secretaryService,
            IHubContext<SignalRHub> hub)
            : base(_userManager)
        {
            _secretaryService = secretaryService;
            _hub = hub;
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentPresentationForSecretary))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns the list of students in a presentation")]
        public async Task<IActionResult> GetStudentPresentations([FromRoute(Name = "id")] Guid examinationSessionId)
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var studentPresentations = await _secretaryService.GetStudentPresentations(examinationSessionId);

                return Ok(studentPresentations);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentPresentationForSecretary))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns the details of a student in presentation")]
        public async Task<IActionResult> GetStudentPresentation([FromRoute(Name = "id")] Guid examinationSessionId)
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                var studentPresentations = await _secretaryService.GetStudentPresentation(userExternalId.Value, examinationSessionId);

                return Ok(studentPresentations);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Updates the absent status to the database")]
        public async Task<IActionResult> UpdateAbsentStatus([FromRoute(Name = "id")] Guid examinationSessionId, [FromBody] UpdateStudentPresentationRequest request)
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                await _secretaryService.UpdateAbsentStatus(userExternalId.Value, examinationSessionId, request);

                return Ok();
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
        [Route("{nextStudentId}/next-student")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentPresentationForSecretary[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns the list with remaining students to present")]
        public async Task<IActionResult> ChooseNextStudent([FromRoute(Name = "id")] Guid examinationSessionId, Guid nextStudentId)
        {
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }

                await _secretaryService.ChooseNextStudent(userExternalId.Value, examinationSessionId, nextStudentId);
                await _hub.Clients.All.SendAsync("onStudentStatusChange");

                return Ok();
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
