using AutoMapper;
using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.ApplicationRequests.Student;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain;
using ExamSupportToolAPI.Domain.Enums;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace ExamSupportToolAPI.ApplicationServices
{
    public class StudentService : IStudentService
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly ISecretaryRepository _secretaryRepository;
        private readonly IExaminationSessionRepository _examinationSessionRepository;
        private readonly IUserService _userService;

        public StudentService(
            IMapper mapper,
            IStudentRepository studentRepository,
            ISecretaryRepository secretaryRepository,
            IExaminationSessionRepository examinationSessionRepository,
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
            _studentRepository = studentRepository;
            _secretaryRepository = secretaryRepository;
            _examinationSessionRepository = examinationSessionRepository;
        }
        public async Task<IEnumerable<StudentForSecretary>> GetAllStudents(Guid userId, Guid examinationSessionId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithStudents(userId, examinationSessionId);
            var result = _mapper.Map<ICollection<StudentForSecretary>>(examinationSession.Students);
            return result;
        }

        public async Task AddStudentToExaminationSession(InsertStudentRequest request)
        {
            try
            {
                var student = await _studentRepository.GetByEmail(request.Email);
                _studentRepository.AssignStudentToExaminationSession(student.Id, request.ExaminationSessionId);
                student.SetCurrentExaminationSessionId(request.ExaminationSessionId);
            }
            catch (InvalidOperationException)
            {
                var studentToCreate = Student.Create(request.Name, request.Email, request.AnonymizationCode,
                    request.YearsAverageGrade, request.DiplomaProjectName, request.CoordinatorName);
                studentToCreate.SetCurrentExaminationSessionId(request.ExaminationSessionId);
                _studentRepository.Add(studentToCreate);
                _studentRepository.AssignStudentToExaminationSession(studentToCreate.Id, request.ExaminationSessionId);
            }
            finally
            {
                await _studentRepository.SaveChangesAsync();
            }
        }
        public async Task UpdateStudent(Guid id, UpdateStudentRequest request)
        {
            var student = _studentRepository.GetById(id);
            student.Name = request.Name;
            student.Email = request.Email;
            student.AnonymizationCode = request.AnonymizationCode;
            student.YearsAverageGrade = request.YearsAverageGrade;
            student.DiplomaProjectName = request.DiplomaProjectName;
            student.CoordinatorName = request.CoordinatorName;
            _studentRepository.Update(student);
            await _studentRepository.SaveChangesAsync();
        }

        public async Task AddStudentsToExaminationSessionBulk(List<InsertStudentsBulkRequest> requests, Guid examinationSessionId)
        {
            List<Student> studentsToAdd = new List<Student>();

            foreach (var request in requests)
            {
                var student = await _studentRepository.GetByEmailFirstOrDefault(request.Email);
                if (student != null)
                {
                    _studentRepository.AssignStudentToExaminationSession(student.Id, examinationSessionId);
                    student.SetCurrentExaminationSessionId(examinationSessionId);
                }
                else
                {
                    var studentToCreate = Student.Create(request.Name, request.Email, request.AnonymizationCode,
                    request.YearsAverageGrade, request.DiplomaProjectName, request.CoordinatorName);
                    studentToCreate.SetCurrentExaminationSessionId(examinationSessionId);
                    studentsToAdd.Add(studentToCreate);
                }
            }

            List<Guid> studentsIds = studentsToAdd.Select(student => student.Id).ToList();

            _studentRepository.AddRange(studentsToAdd);
            _studentRepository.AssignStudentsToExaminationSessionBulk(studentsIds, examinationSessionId);

            await _studentRepository.SaveChangesAsync();
        }
        public async Task RemoveStudentFromExaminationSession(Guid studentId, Guid examinationSessionId)
        {
            var student = _studentRepository.GetById(studentId);

            student.SetCurrentExaminationSessionId(null);

            await _studentRepository.RemoveStudentFromExaminationSession(studentId, examinationSessionId);

            if (!await _studentRepository.HasOtherExaminationSessions(studentId))
            {
                _studentRepository.Remove(student);

                if (student.ExternalId != null)
                    await _userService.RemoveUser(student.ExternalId.Value);
            }

            await _studentRepository.SaveChangesAsync();
        }

        public async Task<ExaminationSessionForStudent> GetExaminationSession(Guid userId)
        {
            var student = await _studentRepository.GetByExternalIdWithPresentationScheduleAndStudentPresentation(userId);
            var examinationSession = student.ExaminationSessions.First(es => es.Id == student.CurrentExaminationSessionId);

            var result = _mapper.Map<ExaminationSession, ExaminationSessionForStudent>(examinationSession, opt => opt.Items["studentId"] = student.Id);
            var presentationScheduleEntry = examinationSession?.PresentationSchedule?.PresentationScheduleEntries.First(pse => pse.StudentId == student.Id);

            if (examinationSession?.PresentationSchedule != null)
            {
                var presentationScheduleForStudent = new PresentationScheduleForStudent
                {
                    StudentPresentationDuration = examinationSession.PresentationSchedule.StudentPresentationDuration,
                    PresentationScheduleEntry = _mapper.Map<PresentationScheduleEntryForStudent>(presentationScheduleEntry)
                };

                result.PresentationSchedule = presentationScheduleForStudent;
            }

            return result;
        }

        public async Task<PresentationScheduleEntryForStudent> GetSchedule(Guid userId)
        {
            var student = await _studentRepository.GetByExternalId(userId);
            var schedule = student.PresentationScheduleEntries.First(ps => ps.PresentationScheduleId == student.CurrentPresentationScheduleId);
            var result = _mapper.Map<PresentationScheduleEntryForStudent>(schedule);
            return result;
        }

        public async Task<StudentPresentationForStudent> GetStudentPresentation(Guid userId)
        {
            var student = await _studentRepository.GetByExternalId(userId);
            var examinationSession = student.ExaminationSessions
                                            .First(es => es.Id == student.CurrentExaminationSessionId);
            var studentPresentation = examinationSession.StudentPresentations
                                                        .Where(s => s.StudentId == student.Id)
                                                        .First();
            await _studentRepository.SaveChangesAsync();
            return _mapper.Map<StudentPresentationForStudent>(studentPresentation);
        }

        public async Task<ExaminationTicketForStudent> GenerateExaminationTicket(Guid userId)
        {
            var student = await _studentRepository.GetByExternalIdWithStudentPresentationAndTickets(userId);

            var examinationSession = student.ExaminationSessions
                                            .First(es => es.Id == student.CurrentExaminationSessionId);

            var studentPresentation = examinationSession.StudentPresentations
                                                        .Where(s => s.StudentId == student.Id)
                                                        .First();

            if (studentPresentation.ExaminationTicket == null)
            {
                // generate a new ticket
                var generatedTicket = student.GenerateExaminationTicket(); ;
                generatedTicket.SetActiveStatus(false);

                studentPresentation.SetExaminationTicket(generatedTicket);
                studentPresentation.SetStatus(Domain.Enums.StudentPresentationStatus.Presenting);

                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest");
                studentPresentation.SetStartTimer(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone));

                await _studentRepository.SaveChangesAsync();
                return _mapper.Map<ExaminationTicketForStudent>(generatedTicket);

            }
            else
            {
                // else return the current ticket assigned
                return _mapper.Map<ExaminationTicketForStudent>(studentPresentation.ExaminationTicket);
            }
        }

        public async Task<StudentGradesReport> GetGradesReport(Guid secretaryMemberId, Guid examinationSessionId)
        {
            var secretary = await _secretaryRepository.GetByExternalId(secretaryMemberId);
            var examinationSession = await _examinationSessionRepository.GetExaminationSesssionWithCommitteeAndStudentPresentations(examinationSessionId);
            if (examinationSession.SecretaryMemberId != secretary.Id)
            {
                throw new ArgumentException("There is no match between secretary and examination session id", "secretaryMemberId");
            }

            var presentations = examinationSession.StudentPresentations
                              .Where(sp => sp.Status == Domain.Enums.StudentPresentationStatus.Graded
                                    || sp.Status == Domain.Enums.StudentPresentationStatus.Completed);

            var report = new StudentGradesReport
            {
                PublicationDate = DateTime.Now,
                ReportName = $"Examination Results {examinationSession.StartDate:MMMM, yyyy}",
                ReportEntries = presentations
                .Where(presentation => presentation.Student != null)
                .Select(
                        presentation => new StudentGradesReportEntry
                        {
                            Absent = presentation.IsAbsent,
                            Code = presentation.Student!.AnonymizationCode,
                            Name = presentation.Student.Name,
                            ProjectGrade = presentation.ProjectGrade,
                            TheoryGrade = presentation.TheoryGrade
                        }
                    ).ToList()
            };

            return report;

        }
    }
}
