using ExamSupportToolAPI.Domain;

namespace ExamSupportToolAPI.Abstractions
{
    public interface IExaminationSessionRepository
    {
        Task<ICollection<ExaminationSession>> GetExaminationSessionsWithStudentsAndCommittee(Guid secretaryId);
        Task<ExaminationSession> GetExaminationSession(Guid secretaryId, Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionWithStudents(Guid secretaryId, Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionWithCommittee(Guid secretaryId, Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionWithBaseInformation(Guid secretaryId, Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionWithPresentationSchedule(Guid secretaryId, Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionWithStudentsAndSchedule(Guid secretaryId, Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionWithAllInformation(Guid secretaryId, Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSesssionWithCommitteeAndStudentPresentations(Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionWithStudentPresentation(Guid secretaryId, Guid examinationSessionId);
        Task<ICollection<ExaminationSession>> GetExaminationSessionsForCommitteeWithStudentsAndCommittee(Guid committeeId);
        Task<ExaminationSession> GetExaminationSessionWithPresentationScheduleForCommittee(Guid committeeId, Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionWithTicketsAndStudentPresentationsForSecretary(Guid? secretaryId, Guid examinationSessionId);
        Task SetCommitteeIdToNullAfterDeletingTheHeadOfCommittee(Guid? committeeId, Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionById(Guid examinationSessionId);
        Task<List<ExaminationSession>> GetExaminationSessions();
        Task<ExaminationSession> GetExaminationSesssionWithTicketsAndStudentPresentationsForCommittee(Guid examinationSessionId);
        Task<ExaminationSession> GetExaminationSessionWithStudentPresentationAndSchedule(Guid secretaryId, Guid examinationSessionId);

    }
}
