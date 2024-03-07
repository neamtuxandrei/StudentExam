using ExamSupportToolAPI.Domain;

namespace ExamSupportToolAPI.Abstractions
{
    public interface ICommitteeRepository : IBaseRepository<CommitteeMember>
    {
        Task<CommitteeMember> GetByExternalId(Guid externalId);
        Task<List<CommitteeMember>> GetCommitteeMembersByExaminationSessionId(Guid id);
        void AssignCommitteeToExaminationSession(Guid id, Guid examinationSessionId);
        Task<bool> ExaminationSessionHasCommitteeHead(Guid examinationSessionId);
        Task<CommitteeExaminationSession> GetCommitteeSessionRelation(Guid committeeMemberId, Guid examinationSessionId);
        void RemoveCommitteeMemberFromSession(CommitteeExaminationSession committeeExaminationSession);
        Task<CommitteeMember> GetCommitteeMemberById(Guid id);
         bool CheckIfMemberHasSessionsAssigned(Guid committeMemberId);
         void RemoveCommitteeMemberFromSystem(CommitteeMember committeeMember);
    }
}
