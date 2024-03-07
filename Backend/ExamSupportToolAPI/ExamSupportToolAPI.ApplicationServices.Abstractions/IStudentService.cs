using ExamSupportToolAPI.ApplicationRequests.Student;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain;

namespace ExamSupportToolAPI.ApplicationServices.Abstractions
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentForSecretary>> GetAllStudents(Guid userId, Guid sessionId);
        Task UpdateStudent(Guid id, UpdateStudentRequest request);
        Task AddStudentToExaminationSession(InsertStudentRequest request);
        Task AddStudentsToExaminationSessionBulk(List<InsertStudentsBulkRequest> requests, Guid examinationSessionId);
        Task RemoveStudentFromExaminationSession(Guid studentId, Guid examinationSessionId);
        Task<ExaminationSessionForStudent> GetExaminationSession(Guid userId);
        Task<PresentationScheduleEntryForStudent> GetSchedule(Guid userId);
        Task<StudentPresentationForStudent> GetStudentPresentation(Guid userId);
        Task<ExaminationTicketForStudent> GenerateExaminationTicket(Guid userId);
        Task<StudentGradesReport> GetGradesReport(Guid secretaryMemberId, Guid examinationSessionId);
    }
}
