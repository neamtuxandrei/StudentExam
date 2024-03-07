using AutoMapper;
using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.ApplicationRequests.CommitteeMember;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.ApplicationServices.Errors;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain;
using System.Collections.Generic;

namespace ExamSupportToolAPI.ApplicationServices
{
    public class CommitteeService : ICommitteeService
    {
        private readonly ICommitteeRepository _committeeRepository;
        private readonly IExaminationSessionRepository _examinationSessionRepository;
        private readonly ISecretaryRepository _secretaryRepository;
        private readonly IMapper _mapper;

        public CommitteeService(
            IMapper mapper,
            ICommitteeRepository committeeRepository,
            IExaminationSessionRepository examinationSessionRepository,
            ISecretaryRepository secretaryRepository)
        {
            _committeeRepository = committeeRepository;
            _examinationSessionRepository = examinationSessionRepository;
            _secretaryRepository = secretaryRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<CommitteeMemberData>> GetCommitteeMembersByExaminationSessionId(Guid id)
        {
            var committeeMembers = await _committeeRepository.GetCommitteeMembersByExaminationSessionId(id);
            return _mapper.Map<ICollection<CommitteeMemberData>>(committeeMembers);
        }
        public async Task<CommitteeMemberData?> GetHeadOfCommitteeBySessionId(Guid userId, Guid sessionId)
        {
            var session = await _examinationSessionRepository.GetExaminationSessionWithCommittee(userId, sessionId);
            var headOfCommittee = session.CommitteeMembers.FirstOrDefault(c => c.Id == session.HeadOfCommitteeMemberId);
            var headOfCommitteeMapped = _mapper.Map<CommitteeMemberData?>(headOfCommittee);
            return headOfCommitteeMapped;
        }

        public async Task AddCommitteeToExaminationSession(Guid userId, InsertCommitteeRequest request)
        {
            var session = await _examinationSessionRepository.GetExaminationSessionWithCommittee(userId, request.ExaminationSessionId);
            var committeeMember = await _committeeRepository.GetByEmailFirstOrDefault(request.Email);
            if (committeeMember == null)
            {
                committeeMember = CommitteeMember.Create(request.Name, request.Email);
            }
            session.AddCommitee(committeeMember);
            await _committeeRepository.SaveChangesAsync();

            committeeMember.SetCurrentExaminationSessionId(request.ExaminationSessionId);

            if (request.IsHeadOfCommittee == true && session.HeadOfCommitteeMemberId == null)
                session.HeadOfCommitteeMemberId = committeeMember.Id;

            await _committeeRepository.SaveChangesAsync();
        }

        public async Task UpdateCommittee(Guid userId, Guid committeeId, UpdateCommitteeRequest request)
        {
            var session = await _examinationSessionRepository.GetExaminationSessionWithCommittee(userId, request.ExaminationSessionId);
            var committeeMember = _committeeRepository.GetById(committeeId);

            if (request.IsHeadOfCommittee)
            {
                if (session.HeadOfCommitteeMemberId != null && committeeMember.Id != session.HeadOfCommitteeMemberId)
                    throw new CommitteeHeadAlreadyExistsException();

                session.SetHeadOfCommittee(committeeId);
            }
            else if (committeeId == session.HeadOfCommitteeMemberId)
                session.SetHeadOfCommittee(null);

            committeeMember.Name = request.Name;
            _committeeRepository.Update(committeeMember);
            await _committeeRepository.SaveChangesAsync();
        }

        public async Task RemoveCommitteeMemberFromSession(Guid committeeMemberId, Guid sessionId)
        {   bool noSession = true;
            var examSession = await _examinationSessionRepository.GetExaminationSessionById(sessionId);
            var committeeMember = await _committeeRepository.GetCommitteeMemberById(committeeMemberId);
            examSession.RemoveCommittee(committeeMember);

            var sessions = await _examinationSessionRepository.GetExaminationSessions();
            //sessions.Remove(examSession);
            
            foreach(var session in sessions)
            {
                if(session.CommitteeMemberExists(committeeMember) == true)
                { 
                    noSession = false;
                    break;
                }
            }
            if (noSession == true)
            {
                _committeeRepository.RemoveCommitteeMemberFromSystem(committeeMember);
            }
            await _examinationSessionRepository.SetCommitteeIdToNullAfterDeletingTheHeadOfCommittee(committeeMemberId, sessionId);
            await _committeeRepository.SaveChangesAsync();
            
        }
        public async Task<ICollection<ExaminationSessionListForCommittee>> GetExaminationSessions(Guid userId)
        {
            var examinationSessions = await _examinationSessionRepository.GetExaminationSessionsForCommitteeWithStudentsAndCommittee(userId);
            var result = _mapper.Map<ICollection<ExaminationSessionListForCommittee>>(examinationSessions);
            return result;
        }
        public async Task<PresentationScheduleForCommittee?> GetPresentationSchedule(Guid examinationSessionId, Guid committeeExternalId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithPresentationScheduleForCommittee(committeeExternalId, examinationSessionId);
            var presentationSchedule = examinationSession.PresentationSchedule;

            var presentationScheduleForCommittee = _mapper.Map<PresentationScheduleForCommittee>(presentationSchedule);

            if (presentationScheduleForCommittee != null)
            {
                presentationScheduleForCommittee.PresentationScheduleEntries =
                    presentationScheduleForCommittee.PresentationScheduleEntries
                    .OrderBy(pse => pse.Date)
                    .ToList();
            }
            return presentationScheduleForCommittee;
        }
        private int GetTheoryGradeForMember(Guid memberId, StudentPresentation studentPresentation)
        { 
            var commiteeMemberData = studentPresentation.CommitteeMemberGrades
                                                        .FirstOrDefault(mg => mg.CommitteeMemberId == memberId);
            if (commiteeMemberData != null) 
            {
                return commiteeMemberData.TheoryGrade;
            }
            return 0;
        }

        private int GetProjectGradeForMember(Guid memberId, StudentPresentation studentPresentation)
        {
            var commiteeMemberData = studentPresentation.CommitteeMemberGrades
                                                        .FirstOrDefault(mg => mg.CommitteeMemberId == memberId);
            if (commiteeMemberData != null)
            {
                return commiteeMemberData.ProjectGrade;
            }
            return 0;
        }

       
        public async Task<IEnumerable<StudentGrade>> GetCommitteeGradesForStudents(Guid externalCommitteeId, Guid examinationSessionId)
        {
            var committeeMember = await _committeeRepository.GetByExternalId(externalCommitteeId);           
            var examinationSession = await _examinationSessionRepository.GetExaminationSesssionWithCommitteeAndStudentPresentations(examinationSessionId);
            if (!examinationSession.CommitteeMembers.Any(cm => cm.Id == committeeMember.Id))
                throw new ArgumentException("Committee Member not found in the specified examination session","externalCommitteeId");
         
            var studentsGrades = examinationSession.StudentPresentations
                                                    .Where(sp => sp.Student != null)
                                                    .Select(sp => new StudentGrade
                                                    {
                                                        Id = sp.Id,
                                                        Name = sp.Student.Name,
                                                        CoordinatorName = sp.Student.CoordinatorName,
                                                        DiplomaProjectName = sp.Student.DiplomaProjectName,
                                                        TheoryGrade = GetTheoryGradeForMember(committeeMember.Id, sp),
                                                        ProjectGrade = GetProjectGradeForMember(committeeMember.Id, sp),
                                                        ProjectAverage = sp.ProjectGrade,
                                                        TheoryAverage = sp.TheoryGrade

                                                    }).ToList();
            if (studentsGrades == null)
                return new List<StudentGrade>();

            return studentsGrades;
        }
        public async Task<StudentGrade> SetStudentGradeFromCommitteeMember(Guid externalCommitteeId, Guid examinationSessionId, StudentGrade grade)
        {
            var committeeMember = await _committeeRepository.GetByExternalId(externalCommitteeId);
            var examinationSession = await _examinationSessionRepository.GetExaminationSesssionWithCommitteeAndStudentPresentations(examinationSessionId);
            if (!examinationSession.CommitteeMembers.Any(cm => cm.Id == committeeMember.Id))
                throw new ArgumentException("Committee Member not found in the specified examination session", "externalCommitteeId");

            var presentation = examinationSession.StudentPresentations
                                                .Where(sp => sp.Id == grade.Id)
                                                .First();
            presentation.AssignStudentGrade(committeeMember.Id, grade.TheoryGrade, grade.ProjectGrade);
            grade.TheoryAverage = presentation.TheoryGrade;
            grade.ProjectAverage = presentation.ProjectGrade;
            await _committeeRepository.SaveChangesAsync();

            return grade;
        }

        public async Task<ICollection<StudentPresentationForCommittee>> GetStudentPresentations(Guid externalCommitteeId, Guid examinationSessionId)
        {
            var committeeMember = await _committeeRepository.GetByExternalId(externalCommitteeId);
            var examinationSession = await _examinationSessionRepository.GetExaminationSesssionWithTicketsAndStudentPresentationsForCommittee(examinationSessionId);
            if (!examinationSession.CommitteeMembers.Any(cm => cm.Id == committeeMember.Id))
                throw new ArgumentException("Committee Member not found in the specified examination session", "externalCommitteeId");

            var studentPresentations = _mapper.Map<ICollection<StudentPresentationForCommittee>>(examinationSession.StudentPresentations);

            return studentPresentations;
        }

        public async Task<StudentPresentationForCommittee?> GetCurrentPresentingStudent(Guid externalCommitteeId, Guid examinationSessionId)
        {
            var committeeMember = await _committeeRepository.GetByExternalId(externalCommitteeId);
            var examinationSession = await _examinationSessionRepository.GetExaminationSesssionWithTicketsAndStudentPresentationsForCommittee(examinationSessionId);
            if (!examinationSession.CommitteeMembers.Any(cm => cm.Id == committeeMember.Id))
                throw new ArgumentException("Committee Member not found in the specified examination session", "externalCommitteeId");
            
            var studentPresentation = examinationSession.StudentPresentations.SingleOrDefault(s => s.Status == Domain.Enums.StudentPresentationStatus.Presenting);
            if (studentPresentation == null)
                return null;

            var studentPresentationMapped = _mapper.Map<StudentPresentationForCommittee>(studentPresentation);
            if(examinationSession.PresentationSchedule != null)
               studentPresentationMapped.StudentPresentationDuration = examinationSession.PresentationSchedule.StudentPresentationDuration;
            studentPresentationMapped.CurrentCommitteeId = committeeMember.Id;

            return studentPresentationMapped;
        }
    }
}
