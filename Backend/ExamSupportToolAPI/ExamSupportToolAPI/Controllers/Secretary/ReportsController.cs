using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RazorPagesReporting;

namespace ExamSupportToolAPI.Controllers.Secretary
{
    [Route("[controller]")]
    public class ReportsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentService _studentsService;
        private readonly RazorPagesReportingEngine _reportingEngine;
        public ReportsController(UserManager<ApplicationUser> userManager, 
                                 IStudentService studentsService,
                                 RazorPagesReportingEngine reportingEngine) 
        {
            _userManager = userManager;
            _studentsService = studentsService;
            _reportingEngine = reportingEngine; 
        }

        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> StudentGradesReport([FromQuery] Guid examinationSessionId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();
            var report = await _studentsService.GetGradesReport(Guid.Parse(userId), examinationSessionId);

            return await _reportingEngine.RenderViewAsPdf("Reports/StudentGradesReportView", report, $"StudentGrades_{report.ReportName}.pdf");

        }

       
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> AnonymousStudentGradesReport([FromQuery]Guid examinationSessionId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();
            var report = await _studentsService.GetGradesReport(Guid.Parse(userId), examinationSessionId);

            return await _reportingEngine.RenderViewAsPdf("Reports/AnonStudentGradesReportView", report, $"AnonStudentGrades_{report.ReportName}.pdf");
                
        }

        
    }
}
