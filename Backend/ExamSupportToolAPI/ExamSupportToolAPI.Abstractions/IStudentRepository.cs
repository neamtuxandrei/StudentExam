using ExamSupportToolAPI.Domain;

namespace ExamSupportToolAPI.Abstractions
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Student> GetByExternalId(Guid userId);
        Task<Student> GetByExternalIdWithPresentationScheduleAndStudentPresentation(Guid userId);
        Task<Student> GetByExternalIdWithStudentPresentationAndTickets(Guid userId);
        Task<List<Student>> GetStudentsByExaminationSession(Guid id);
        void AssignStudentToExaminationSession(Guid studentId, Guid examinationSessionId);
        void AssignStudentsToExaminationSessionBulk(ICollection<Guid> studentsId, Guid examinationSessionId);
        Task RemoveStudentFromExaminationSession(Guid studentId, Guid examinationSessionId);
        Task<bool> HasOtherExaminationSessions(Guid studentId);
        Task<StudentPresentation> GetStudentPresentation(Guid studentId);
    }
}
