using ExamSupportToolAPI.ApplicationRequests.CommitteeMember;
using ExamSupportToolAPI.DataObjects;

namespace ExamSupportToolAPI.ApplicationServices.Abstractions
{
    public interface ICommitteeService
    {
        Task<ICollection<CommitteeMemberData>> GetCommitteeMembersByExaminationSessionId(Guid id);
        Task RemoveCommitteeMemberFromSession(Guid committeeMemberId, Guid sessionId);
        Task<CommitteeMemberData?> GetHeadOfCommitteeBySessionId(Guid userId, Guid sessionId);
        Task AddCommitteeToExaminationSession(Guid userId, InsertCommitteeRequest request);
        Task UpdateCommittee(Guid userId, Guid committeeId, UpdateCommitteeRequest request);
        Task<ICollection<ExaminationSessionListForCommittee>> GetExaminationSessions(Guid userId);
        Task<PresentationScheduleForCommittee?> GetPresentationSchedule(Guid examinationSessionId, Guid committeeExternalId);
        Task<IEnumerable<StudentGrade>> GetCommitteeGradesForStudents(Guid externalCommitteeId, Guid examinationSessionId);
        Task<StudentGrade> SetStudentGradeFromCommitteeMember(Guid externalCommitteeId, Guid examinationSessionId, StudentGrade grade);
        Task<ICollection<StudentPresentationForCommittee>> GetStudentPresentations(Guid externalCommitteeId, Guid examinationSessionId);
        Task<StudentPresentationForCommittee?> GetCurrentPresentingStudent(Guid externalCommitteeId, Guid examinationSessionId);
    }
}
