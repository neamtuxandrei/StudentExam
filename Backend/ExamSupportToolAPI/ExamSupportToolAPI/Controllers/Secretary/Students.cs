using ExamSupportToolAPI.ApplicationServices.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using ExamSupportToolAPI.ApplicationRequests.Student;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using ExamSupportToolAPI.Data;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace ExamSupportToolAPI.Controllers.Secretary
{
    [Route("api/secretary/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class Students : UserBasedController
    {
        private readonly IStudentService _studentService;
        public Students(
            UserManager<ApplicationUser> _userManager,
            IStudentService studentService)
            : base(_userManager)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Route("{examinationSessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentForSecretary))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns the list of students by session id")]
        public async Task<IActionResult> GetStudents(Guid examinationSessionId)
        {
            if (examinationSessionId == Guid.Empty)
                return BadRequest("Id can't be empty");

            var userExternalId = GetCurrentUserId();

            if (userExternalId == null)
                return Unauthorized();

            try
            {
                var students = await _studentService.GetAllStudents(userExternalId.Value, examinationSessionId);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Adds new/assign existing student to an examination session")]
        public async Task<IActionResult> InsertStudent([FromBody] InsertStudentRequest request)
        {
            if (request is null) throw new Exception($"Request {typeof(InsertStudentRequest)} is null");
            try
            {
                await _studentService.AddStudentToExaminationSession(request);
                return Ok();
            }
            catch (DbUpdateException)
            {
                return NotFound(new ErrorResult() { Description = "Could not find student" });
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
        [SwaggerOperation(Summary = "Updates an existing student")]
        public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentRequest request)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id can't be empty");
            }
            try
            {
                if (request is null) throw new Exception($"Request {typeof(UpdateStudentRequest)} is null");
                await _studentService.UpdateStudent(id, request);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ErrorResult() { Description = "Could not find student" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }

        [HttpPost]
        [Route("bulk/{examinationSessionid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Adds students (bulk) to examination session.")]
        public async Task<IActionResult> InsertStudentsBulk([FromBody] List<InsertStudentsBulkRequest> requests, [FromRoute] Guid examinationSessionid)
        {
            if (requests is null) throw new Exception($"Request {typeof(List<InsertStudentsBulkRequest>)} is null");
            try
            {
                await _studentService.AddStudentsToExaminationSessionBulk(requests, examinationSessionid);
                return Ok();
            }
            catch (DbUpdateException)
            {
                return NotFound(new ErrorResult() { Description = "Examination session could not be found." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResult() { Description = $"Student information imported from the excel file is incorrect. (Column: {ex.ParamName})" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Removes a student from an examination session")]
        public async Task<IActionResult> RemoveStudentFromExaminationSession([FromQuery] Guid studentId, Guid examinationSessionId)
        {
            if (studentId == Guid.Empty || examinationSessionId == Guid.Empty)
            {
                return BadRequest(new ErrorResult() { Description = "Student id or examination session id can't be null." });
            }

            try
            {
                await _studentService.RemoveStudentFromExaminationSession(studentId, examinationSessionId);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ErrorResult() { Description = "Student or examination session not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }
    }
}
