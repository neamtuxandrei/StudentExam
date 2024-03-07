using ExamSupportToolAPI.Domain;

namespace ExamSupportToolAPI.Abstractions
{
    public interface ISecretaryRepository : IBaseRepository<SecretaryMember>
    {
        Task<SecretaryMember> GetByExternalId(Guid userId);
        Task<List<ExaminationSession>> GetExaminationSessions(Guid secretaryId);
        Task<List<ExaminationTicket>> GetExaminationTicketsBySessionId(Guid id);
        void UpdateExaminationTicket(ExaminationTicket examinationTicket);
        void AddExaminationTicket(ExaminationTicket examinationTicket);
        void AddExaminationSession(ExaminationSession session);
        void AddExaminationTicketsBulk(ICollection<ExaminationTicket> tickets);
        void RemoveExaminationTicket(ExaminationTicket ticket);
        void AddPresentationSchedule(PresentationSchedule presentationSchedule);
        void AddStudentPresentations(List<StudentPresentation> studentPresentations);
        void UpdatePresentationSchedule(PresentationSchedule presentationSchedule);
        bool CheckIfTicketNrExistsInSession(int ticketNo, Guid sessionId);
        Task<ExaminationTicket> GetExaminationTicketById(Guid id);
        Task<PresentationSchedule> GetPresentationSchedule(Guid id);
        Task RemovePresentationSchedule(Guid examinationSessionId);
        Task<List<StudentPresentation>> GetStudentPresentationsBySessionId(Guid id);
        
    }
}
