using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamSupportToolAPI.DataAccess.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private readonly ExaminationSessionDbContext _dbContext;
        public StudentRepository(ExaminationSessionDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Student> GetByExternalId(Guid userId)
        {
            var result = await _dbContext.Students
                //.Include(e => e.ExaminationSessions).ThenInclude(s => s.Students)
                .Include(e => e.ExaminationSessions)
                    .ThenInclude(c => c.CommitteeMembers)
                .Include(e => e.ExaminationSessions)
                    .ThenInclude(t => t.ExaminationTickets)
                .Include(e => e.ExaminationSessions)
                    .ThenInclude(e => e.PresentationSchedule)
                    .ThenInclude(ps => ps.PresentationScheduleEntries)
                    .ThenInclude(pse => pse.Student)
                .Include(e => e.ExaminationSessions)
                    .ThenInclude(es => es.StudentPresentations)
                //.ThenInclude(sp => sp.ExaminationTicket)
                .FirstAsync(s => s.ExternalId != null && s.ExternalId.Equals(userId));

            return result;
        }

        public async Task<Student> GetByExternalIdWithPresentationScheduleAndStudentPresentation(Guid userId)
        {
            var result = await _dbContext.Students
                                            .Include(e => e.ExaminationSessions)
                                                .ThenInclude(e => e.PresentationSchedule)
                                                .ThenInclude(ps => ps.PresentationScheduleEntries)
                                            .Include(e => e.ExaminationSessions)
                                                .ThenInclude(es => es.StudentPresentations)
                                                .ThenInclude(es => es.ExaminationTicket)
                                            .FirstAsync(s => s.ExternalId != null && s.ExternalId.Equals(userId));

            return result;
        }

        public async Task<Student> GetByExternalIdWithStudentPresentationAndTickets(Guid userId)
        {
            var result = await _dbContext.Students
                                .Include(e => e.ExaminationSessions)
                                    .ThenInclude(es => es.StudentPresentations)
                                .Include(e => e.ExaminationSessions)
                                    .ThenInclude(es => es.ExaminationTickets)
                                .FirstAsync(s => s.ExternalId != null && s.ExternalId.Equals(userId));

            return result;
        }

        public async Task<List<Student>> GetStudentsByExaminationSession(Guid id)
        {
            var studentsIds = await _dbContext.StudentExaminationSessions
                .Where(s => s.ExaminationSessionId == id)
                .Select(s => s.StudentId).ToListAsync();

            var students = await _dbContext.Students
                .Where(stud => studentsIds.Contains(stud.Id))
                .ToListAsync();

            return students;
        }
        public void AssignStudentToExaminationSession(Guid studentId, Guid examinationSessionId)
        {
            _dbContext.StudentExaminationSessions.Add(new StudentExaminationSession()
            {
                StudentId = studentId,
                ExaminationSessionId = examinationSessionId
            });
        }
        public void AssignStudentsToExaminationSessionBulk(ICollection<Guid> studentsId, Guid examinationSessionId)
        {
            foreach (var studentId in studentsId)
            {
                _dbContext.StudentExaminationSessions.Add(new StudentExaminationSession()
                {
                    StudentId = studentId,
                    ExaminationSessionId = examinationSessionId
                });
            }
        }

        public async Task RemoveStudentFromExaminationSession(Guid studentId, Guid examinationSessionId)
        {
            var studentExaminationSession = await _dbContext.StudentExaminationSessions
                                .Where(ses => ses.StudentId == studentId && ses.ExaminationSessionId == examinationSessionId)
                                .FirstAsync();

            _dbContext.Remove(studentExaminationSession);
        }

        public async Task<bool> HasOtherExaminationSessions(Guid studentId)
        {
            var sessions = await _dbContext.StudentExaminationSessions
                .Where(ses => ses.StudentId == studentId)
                .ToListAsync();

            return sessions.Count > 1;
        }

        public async Task<StudentPresentation> GetStudentPresentation(Guid studentId)
        {
            var studentPresentation = await _dbContext.StudentPresentations
                .FirstAsync(sp => sp.StudentId == studentId);
            return studentPresentation;
        }
    }
}
