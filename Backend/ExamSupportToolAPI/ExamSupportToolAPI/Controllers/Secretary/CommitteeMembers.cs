using ExamSupportToolAPI.ApplicationRequests.CommitteeMember;
using ExamSupportToolAPI.ApplicationRequests.Student;
using ExamSupportToolAPI.ApplicationServices;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.ApplicationServices.Errors;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ExamSupportToolAPI.Controllers.Secretary
{
    [Route("api/secretary/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]

    public class CommitteeMembers : UserBasedController
    {
        private readonly ICommitteeService _committeeService;

        public CommitteeMembers(UserManager<ApplicationUser> _userManager,
            ICommitteeService committeeService) : base(_userManager)
        {
            _committeeService = committeeService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommitteeMemberData))]
        [SwaggerOperation(Summary = "Returns committee member list by examination session id")]
        public async Task<IActionResult> GetCommitteeMembers(Guid id)
        {
            var committeeMembers = await _committeeService.GetCommitteeMembersByExaminationSessionId(id);
            return Ok(committeeMembers);
        }

        [HttpGet]
        [Route("{id}/HeadOfCommittee")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommitteeMemberData))]
        [SwaggerOperation(Summary = "Returns the head of committee by examination session id")]
        public async Task<IActionResult> GetHeadOfCommittee(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id can't be empty");

            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }
                var committeeMember = await _committeeService.GetHeadOfCommitteeBySessionId(userExternalId.Value,id);
                return Ok(committeeMember);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Adds new committee member / assigns an existing one to the examination session")]
        public async Task<IActionResult> InsertCommittee([FromBody] InsertCommitteeRequest request)
        {
            if (request is null) throw new Exception($"Request {typeof(InsertStudentRequest)} is null");
            try
            {
                var userExternalId = GetCurrentUserId();

                if (userExternalId == null)
                {
                    return Unauthorized();
                }
                await _committeeService.AddCommitteeToExaminationSession(userExternalId.Value,request);
                return Ok();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new ErrorResult() { Description = "The committee member is already added or the examination session does not exist." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        //TODO: Add a solution level project for errors
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(CommitteeHeadAlreadyExistsException))]
        [SwaggerOperation(Summary = "Updates the information for a committee member.")]
        public async Task<IActionResult> UpdateCommittee(Guid id, [FromBody] UpdateCommitteeRequest request)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id can't be empty");
            }
            if (request is null) throw new Exception($"Request {typeof(UpdateCommitteeRequest)} is null");

            try
            {
            var userExternalId = GetCurrentUserId();

            if (userExternalId == null)
            {
                return Unauthorized();
            }
                await _committeeService.UpdateCommittee(userExternalId.Value, id, request);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ErrorResult() { Description = "Could not find committee member." });
            }
            catch (CommitteeHeadAlreadyExistsException)
            {
                return Conflict(new ErrorResult() { Description = "Committee head already exists." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{committeeId}/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Removes the committee member from a session or from the system if he has no more sessions assigned")]
        public async Task<IActionResult> RemoveCommitteeFromSession(Guid committeeId, Guid sessionId)
        {
            try
            {
                await _committeeService.RemoveCommitteeMemberFromSession(committeeId, sessionId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }
    }   
}
