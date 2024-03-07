using ExamSupportToolAPI.ApplicationRequests.ExaminationTicket;
using ExamSupportToolAPI.ApplicationRequests.Student;
using ExamSupportToolAPI.ApplicationServices;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using ExamSupportToolAPI.Domain;
using ExamSupportToolAPI.ApplicationRequests.ExaminationTicket;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ExamSupportToolAPI.Controllers.Secretary
{
    [Route("api/secretary/[controller]")]
    [ApiController]
    public class ExaminationTickets : ControllerBase
    {
        private readonly ISecretaryService _secretaryService;

        public ExaminationTickets(ISecretaryService secretaryService)
        {
            _secretaryService = secretaryService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExaminationTicketForSecretary))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Returns the list of examination tickets by session id")]
        public async Task<IActionResult> GetExaminationTickets(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            try
            {
                var tickets = await _secretaryService.GetExaminationTicketsBySessionId(id);
                return Ok(tickets);
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

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Removes a ticket by id")]
        public async Task<IActionResult> RemoveExaminationTicket(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id is required");
            try
            {
                await _secretaryService.RemoveExaminationTicketById(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ErrorResult() { Description = "Could not find the ticket" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Adds a ticket to a session")]
        public async Task<IActionResult> AddTicketToSession([FromBody] InsertTicketRequest request)
        {
            var serialized = JsonSerializer.Serialize(request);
            
            if (request is null) throw new Exception($"Request {typeof(InsertTicketRequest)} is null");
            try
            {
                await _secretaryService.AddTicketToSession(request);
                return Ok();
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
        [SwaggerOperation(Summary = "Updates an examination ticket")]
        public async Task<IActionResult> UpdateExaminationTicket(Guid id,[FromBody]UpdateTicketRequest request)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id can't be empty");
            }
            try
            {
                await _secretaryService.UpdateExaminationTicket(id,request);
                return Ok();   
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ErrorResult() { Description = "Could not find the examination ticket" });
            }
             catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }
        [HttpPost]
        [Route("bulk/{importToSessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [SwaggerOperation(Summary = "Import tickets from an examination session to another")]
        public async Task<IActionResult> ImportTickets([FromBody] InsertExaminationTicketBulkRequest request, [FromRoute] Guid importToSessionId)
        {
            if (request is null) throw new Exception($"Request {typeof(InsertStudentRequest)} is null");
            try
            {
                await _secretaryService.ImportExaminationTicketsFromAnotherSession(importToSessionId, request);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ErrorResult() { Description = "Could not find the ticket" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult() { Description = ex.Message });
            }
        }


    }
}
