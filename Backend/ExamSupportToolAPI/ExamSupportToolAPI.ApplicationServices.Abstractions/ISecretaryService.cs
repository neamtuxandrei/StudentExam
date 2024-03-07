using ExamSupportToolAPI.ApplicationRequests.PresentationSchedule;
using ExamSupportToolAPI.ApplicationRequests.ExaminationSession;
using ExamSupportToolAPI.ApplicationRequests.ExaminationTicket;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain.Enums;
using ExamSupportToolAPI.ApplicationRequests.StudentPresentation;

namespace ExamSupportToolAPI.ApplicationServices.Abstractions
{
    public interface ISecretaryService
    {
        Task<ICollection<ExaminationSessionListForSecretary>> GetExaminationSessions(Guid userId);
        Task<ExaminationSessionForSecretary> GetExaminationSession(Guid userId, Guid examinationSessionId);
        Task<ExaminationSessionPresentationForSecretary> GetExaminationSessionForPresentation(Guid userId, Guid examinationSessionId);
        Task SetExaminationSessionStatus(Guid userId, Guid examinationSessionId, SessionStatus status);
        Task AddExaminationSession(Guid userId, InsertExaminationSessionRequest request);
        Task<ICollection<ExaminationTicketForSecretary>> GetExaminationTicketsBySessionId(Guid id);
        Task RemoveExaminationTicketById(Guid id);
        Task AddTicketToSession(InsertTicketRequest request);
        Task UpdateExaminationTicket(Guid id, UpdateTicketRequest request);
        Task ImportExaminationTicketsFromAnotherSession(Guid importSessionId, InsertExaminationTicketBulkRequest request);
        Task<ICollection<StudentPresentationForSecretary>> GetStudentPresentations(Guid examinationSessionId);
        Task<PresentationScheduleForSecretary?> GetPresentationSchedule(Guid examinationSessionId, Guid secretaryExternalId);
        Task GeneratePresentationSchedule(Guid examinationSessionId, Guid secretaryExternalId, InsertPresentationSchedule insertPresentationSchedule);
        Task MovePresentationScheduleEntry(Guid examinationSessionId, Guid secretaryExternalId, MovePresentationScheduleEntryRequest movePresentationScheduleEntryRequest);
        Task GenerateStudentPresentation(Guid examinationSessionId, Guid secretaryExternalId);
        Task<StudentPresentationForSecretary> GetStudentPresentation(Guid userId, Guid examinationSessionId);
        Task UpdateAbsentStatus(Guid userExternalId, Guid examinationSessionId, UpdateStudentPresentationRequest request);
        Task<ICollection<StudentForSecretaryDropdown>> GetRemainingStudentsToPresent(Guid userId, Guid examinationSessionId);
        Task ChooseNextStudent(Guid userId, Guid examinationSessionId, Guid nextStudentPresentationId);
        Task<bool> CheckIfExaminationSessionHasSchedule(Guid secretaryExternalId, Guid examinationSessionId);
        Task PublishGrades(Guid userId, Guid examinationSessionId);
    }
}
