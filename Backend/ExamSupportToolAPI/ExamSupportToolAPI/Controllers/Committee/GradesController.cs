using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OpenIddict.Validation.AspNetCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamSupportToolAPI.Controllers.Committee
{
    [Route("api/committee/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class GradesController : UserBasedController
    {

        private readonly ICommitteeService _committeeService;
        private readonly IHubContext<SignalRHub> _hub;

        public GradesController(
            UserManager<ApplicationUser> userManager,
            ICommitteeService committeeService,
            IHubContext<SignalRHub> hub
            ) : base(userManager)
        {
            _committeeService = committeeService;
            _hub = hub;
        }
        // GET: api/<GradesController>
        [HttpGet("{examinationSessionId}")]
        public async Task<IActionResult> GetCommitteeExaminationGrades(Guid examinationSessionId)
        {
            var userExternalId = GetCurrentUserId();
            if (userExternalId == null)
                return Unauthorized();
            try
            {
                return Ok(await _committeeService.GetCommitteeGradesForStudents(userExternalId.Value, examinationSessionId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

       

        // POST api/<GradesController>
        [HttpPost("{examinationSessionId}")]
        public async Task<IActionResult> SetStudentGrades([FromRoute]Guid examinationSessionId, [FromBody] StudentGrade studentGrades)
        {
            var externalUserId = GetCurrentUserId();
            if (externalUserId == null)
                return Unauthorized();
            studentGrades = await _committeeService.SetStudentGradeFromCommitteeMember(externalUserId.Value, examinationSessionId, studentGrades);
            await _hub.Clients.All.SendAsync("OnStudentGradeUpdate");
            return Ok(studentGrades);
        }

        // PUT api/<GradesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GradesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
