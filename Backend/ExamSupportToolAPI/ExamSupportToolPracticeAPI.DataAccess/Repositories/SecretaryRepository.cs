using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.DataAccess;
using ExamSupportToolAPI.DataAccess.Repositories;
using ExamSupportToolAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamSupportTooAPI.DataAccess.Repositories
{
    public class SecretaryRepository : BaseRepository<SecretaryMember>, ISecretaryRepository
    {
        private readonly ExaminationSessionDbContext _dbContext;
        public SecretaryRepository(ExaminationSessionDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<List<ExaminationSession>> GetExaminationSessions(Guid secretaryId)
        {
            var examinationSessions = _dbContext.ExaminationSessions
                .Where(ex => ex.SecretaryMemberId == secretaryId)
                .ToListAsync();
            return examinationSessions;
        }
        public void AddExaminationTicketsBulk(ICollection<ExaminationTicket> tickets)
        {
            _dbContext.AddRange(tickets);
        }

        public void AddExaminationSession(ExaminationSession session)
        {
            _dbContext.ExaminationSessions.Add(session);
        }
        public async Task<SecretaryMember> GetByExternalId(Guid userId)
        {
            var result = await _dbContext.SecretaryMembers
                .FirstAsync(s => s.ExternalId != null && s.ExternalId.Equals(userId));

            return result;
        }

        public async Task<List<ExaminationTicket>> GetExaminationTicketsBySessionId(Guid id)
        {
            var examinationTickets = await _dbContext.ExaminationTickets
                .Where(et => et.ExaminationSessionId == id)
                .ToListAsync();
            return examinationTickets;
        }
        public async Task<ExaminationTicket> GetExaminationTicketById(Guid id)
        {
            var examinationTicket = await _dbContext.ExaminationTickets.FirstAsync(s => s.Id == id);
            return examinationTicket;
        }

        public void RemoveExaminationTicket(ExaminationTicket ticket)
        {
            _dbContext.Remove(ticket);
        }

        public void UpdateExaminationTicket(ExaminationTicket ticket)
        {
            _dbContext.Update(ticket);
        }

        public bool CheckIfTicketNrExistsInSession(int ticketNo,Guid sessionId)
        {
            return _dbContext.ExaminationTickets.Any(et => et.TicketNo == ticketNo && et.ExaminationSessionId == sessionId);
        }

        public void AddExaminationTicket(ExaminationTicket ticket)
        {
            _dbContext.ExaminationTickets.Add(ticket);
        }
        public async Task<PresentationSchedule> GetPresentationSchedule(Guid id)
        {
            var presentationSchedule = await _dbContext.PresentationSchedules
                .Include(ps => ps.PresentationScheduleEntries)
                    .ThenInclude(pse => pse.Student)
                .Where(ps => ps.ExaminationSessionId == id)
                .FirstAsync();
            return presentationSchedule;
        }
        public async Task<List<StudentPresentation>> GetStudentPresentationsBySessionId(Guid id)
        {
            var studentPresentations = await _dbContext.StudentPresentations
                .Include(sp => sp.Student)
                .Include(sp => sp.ExaminationTicket)
                .Where(sp => sp.ExaminationSessionId == id)
                .ToListAsync();
            return studentPresentations;
        }
        public void AddPresentationSchedule(PresentationSchedule presentationSchedule)
        {
            _dbContext.Add(presentationSchedule);
        }

        public void UpdatePresentationSchedule(PresentationSchedule presentationSchedule)
        {
            _dbContext.Update(presentationSchedule);
        }
        public async Task RemovePresentationSchedule(Guid examinationSessionId)
        {
            var presentationSchedule = await GetPresentationSchedule(examinationSessionId);
            _dbContext.Remove(presentationSchedule);
        }

        public void AddStudentPresentations(List<StudentPresentation> studentPresentations)
        {
            _dbContext.AddRange(studentPresentations);
        }
        
        
    }
}
