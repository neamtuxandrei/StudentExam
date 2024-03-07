using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamSupportToolAPI.DataAccess.Repositories
{
    public class CommitteeRepository : BaseRepository<CommitteeMember>, ICommitteeRepository
    {
        private readonly ExaminationSessionDbContext _dbContext;
        public CommitteeRepository(ExaminationSessionDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CommitteeMember> GetByExternalId(Guid externalId)
        {
            return await _dbContext.CommitteeMembers.Where(cm => cm.ExternalId == externalId)
                                             .FirstAsync();
        }
        public async Task<List<CommitteeMember>> GetCommitteeMembersByExaminationSessionId(Guid id)
        {
            var committeeMemberIds = await _dbContext.CommitteeExaminationSessions
                .Where(ces => ces.ExaminationSessionId == id)
                .Select(c => c.CommitteeMemberId)
                .ToListAsync();

            var committeeMembersBySessionId = await _dbContext.CommitteeMembers
                .Where(c => committeeMemberIds.Contains(c.Id))
                .ToListAsync();

            return committeeMembersBySessionId;
        }

        public void AssignCommitteeToExaminationSession(Guid committeeId, Guid examinationSessionId)
        {
            _dbContext.CommitteeExaminationSessions.Add(new CommitteeExaminationSession()
            {
                CommitteeMemberId = committeeId,
                ExaminationSessionId = examinationSessionId
            });
        }

        public async Task<bool> ExaminationSessionHasCommitteeHead(Guid examinationSessionId)
        {
            var session = await _dbContext.ExaminationSessions.FirstAsync(s=> s.Id == examinationSessionId);
            Guid? hasHeadOfCommittee = session.HeadOfCommitteeMemberId;
            return hasHeadOfCommittee != null;

            //.Where(session => session.Id == examinationSessionId)
            //.SelectMany(session => session.CommitteeMembers)
            // .AnyAsync(committeeMember => committeeMember.IsHeadOfCommittee);
            //return hasHeadOfCommittee;
        }

        public async Task<CommitteeExaminationSession> GetCommitteeSessionRelation(Guid committeeMemberId, Guid examinationSessionId)
        {
            var committeeSessionRel = await _dbContext.CommitteeExaminationSessions.Where(cs => cs.CommitteeMemberId == committeeMemberId && cs.ExaminationSessionId == examinationSessionId).FirstAsync();
            return committeeSessionRel;

        }

        public void RemoveCommitteeMemberFromSession(CommitteeExaminationSession committeeExaminationSession)
        {
            _dbContext.Remove(committeeExaminationSession);
        }

        

        public async Task<CommitteeMember> GetCommitteeMemberById(Guid id)
        {
            var committeeMember = await _dbContext.CommitteeMembers.FirstAsync(cm => cm.Id == id);
            return committeeMember;

        }
        public bool CheckIfMemberHasSessionsAssigned(Guid committeMemberId)
        {
            return !_dbContext.CommitteeExaminationSessions.Any(cs => cs.CommitteeMemberId == committeMemberId);
        }
        public void RemoveCommitteeMemberFromSystem(CommitteeMember committeeMember)
        {
            _dbContext.Remove(committeeMember);


        }
    }
}
