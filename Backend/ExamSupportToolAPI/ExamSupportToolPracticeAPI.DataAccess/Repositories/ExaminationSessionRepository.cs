using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamSupportToolAPI.DataAccess.Repositories
{
    public class ExaminationSessionRepository : IExaminationSessionRepository
    {
        private readonly ExaminationSessionDbContext _dbContext;
        public ExaminationSessionRepository(ExaminationSessionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExaminationSession> GetExaminationSessionById(Guid examinationSessionId)
        {
            var session = await _dbContext.ExaminationSessions
                .Include(es => es.CommitteeMembers)
                .Where(es => es.Id == examinationSessionId).FirstAsync();
            return session;
        }
        public async Task<ExaminationSession> GetExaminationSession(Guid secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }

        public async Task<ICollection<ExaminationSession>> GetExaminationSessionsWithStudentsAndCommittee(Guid secretaryId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.Students)
                                    .Include(es => es.CommitteeMembers)
                                    .Include(es=> es.ExaminationTickets)
                                    .Where(es => es.SecretaryMemberId == secretary.Id)
                                    .ToListAsync();
        }
        public async Task<ICollection<ExaminationSession>> GetExaminationSessionsForCommitteeWithStudentsAndCommittee(Guid committeeId)
        {
            var committee = await _dbContext.CommitteeMembers
                .Include(e => e.ExaminationSessions)
                .FirstAsync(s => s.ExternalId == committeeId);

            return committee.ExaminationSessions.ToList();
        }

        public async Task<ExaminationSession> GetExaminationSessionWithPresentationSchedule(Guid secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.PresentationSchedule)
                                        .ThenInclude(p => p.PresentationScheduleEntries)
                                        .ThenInclude(p => p.Student)
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }
        public async Task<ExaminationSession> GetExaminationSessionWithPresentationScheduleForCommittee(Guid committeeId, Guid examinationSessionId)
        {
            var committee = await _dbContext.CommitteeMembers
                .Include(e => e.ExaminationSessions)
                   .ThenInclude(es => es.PresentationSchedule)
                   .ThenInclude(p => p.PresentationScheduleEntries)
                   .ThenInclude(p => p.Student)
                .FirstAsync(s => s.ExternalId == committeeId);

            return committee.ExaminationSessions.Single(e => e.Id == examinationSessionId);
        }

        public async Task<ExaminationSession> GetExaminationSessionWithBaseInformation(Guid secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.Students)
                                    .Include(es => es.CommitteeMembers)
                                    .Include(es => es.ExaminationTickets)
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }

        public async Task<ExaminationSession> GetExaminationSessionWithStudents(Guid secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.Students)
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }

        public async Task<ExaminationSession> GetExaminationSessionWithCommittee(Guid secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.CommitteeMembers)
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }

        public async Task<ExaminationSession> GetExaminationSessionWithAllInformation(Guid secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.Students)
                                    .Include(es => es.CommitteeMembers)
                                    .Include(es => es.ExaminationTickets)
                                    .Include(es => es.PresentationSchedule)
                                    .ThenInclude(p => p.PresentationScheduleEntries)
                                    .ThenInclude(p => p.Student)
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }
        public async Task<ExaminationSession> GetExaminationSessionWithStudentPresentation(Guid secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.PresentationSchedule)
                                    .Include(es => es.StudentPresentations)
                                        .ThenInclude(sp => sp.Student)
                                    .Include(es => es.StudentPresentations)
                                        .ThenInclude(sp => sp.ExaminationTicket)
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }
        public async Task<ExaminationSession> GetExaminationSessionWithStudentsAndSchedule(Guid secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.Students)
                                    .Include(es => es.PresentationSchedule)
                                        .ThenInclude(p => p.PresentationScheduleEntries)
                                        .ThenInclude(p => p.Student)
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }

        public async Task SetCommitteeIdToNullAfterDeletingTheHeadOfCommittee(Guid? committeeId, Guid examinationSessionId)
        {
            var session = await _dbContext.ExaminationSessions.Where(es => es.Id == examinationSessionId).FirstAsync();
            if (session.HeadOfCommitteeMemberId == committeeId)
            {
                session.SetHeadOfCommitteeToNull();
            }

        }
        public async Task<ExaminationSession> GetExaminationSesssionWithCommitteeAndStudentPresentations(Guid examinationSessionId)
        {
            return await _dbContext.ExaminationSessions
                             .Where(es => es.Id == examinationSessionId)
                             .Include(es => es.CommitteeMembers)
                             .Include(es => es.StudentPresentations)
                                 .ThenInclude(p => p.Student)
                             .Include(es => es.StudentPresentations)
                                 .ThenInclude(p => p.CommitteeMemberGrades)
                             .FirstAsync();

        }
        public async Task<ExaminationSession> GetExaminationSesssionWithTicketsAndStudentPresentationsForCommittee(Guid examinationSessionId)
        {
            return await _dbContext.ExaminationSessions
                             .Where(es => es.Id == examinationSessionId)
                             .Include(es => es.CommitteeMembers)
                             .Include(es => es.StudentPresentations)
                                 .ThenInclude(p => p.Student)
                             .Include(es => es.StudentPresentations)
                                 .ThenInclude(p => p.ExaminationTicket)
                             .Include(es => es.PresentationSchedule)
                             .FirstAsync();
        }
        public async Task<ExaminationSession> GetExaminationSessionWithTicketsAndStudentPresentationsForSecretary(Guid? secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.ExaminationTickets)
                                    .Include(es => es.StudentPresentations)
                                        .ThenInclude(p => p.Student)
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }

        public async Task<ExaminationSession> GetExaminationSessionWithStudentPresentationAndSchedule(Guid secretaryId, Guid examinationSessionId)
        {
            var secretary = await _dbContext.SecretaryMembers.FirstAsync(s => s.ExternalId == secretaryId);

            return await _dbContext.ExaminationSessions
                                    .Include(es => es.PresentationSchedule)
                                        .ThenInclude(p => p.PresentationScheduleEntries)
                                        .ThenInclude(pse => pse.Student)
                                     .Include(es => es.StudentPresentations)
                                    .Where(es => es.SecretaryMemberId == secretary.Id && es.Id == examinationSessionId)
                                    .FirstAsync();
        }

        public async Task<List<ExaminationSession>> GetExaminationSessions()
        {
            return await _dbContext.ExaminationSessions
                                    .Include(es => es.CommitteeMembers)
                                    .ToListAsync();
        }
    }
}
