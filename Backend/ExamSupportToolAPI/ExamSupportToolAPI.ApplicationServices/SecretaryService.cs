using AutoMapper;
using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.ApplicationRequests.ExaminationSession;
using ExamSupportToolAPI.ApplicationRequests.ExaminationTicket;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain;
using ExamSupportToolAPI.Domain.Enums;
using ExamSupportToolAPI.ApplicationRequests.PresentationSchedule;
using ExamSupportToolAPI.ApplicationRequests.StudentPresentation;

namespace ExamSupportToolAPI.ApplicationServices
{
    public class SecretaryService : ISecretaryService
    {
        private readonly ICommitteeRepository _committeeRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISecretaryRepository _secretaryRepository;
        private readonly IExaminationSessionRepository _examinationSessionRepository;
        private readonly IMapper _mapper;

        public SecretaryService(
            IMapper mapper,
            ICommitteeRepository committeeRepository,
            IStudentRepository studentRepository,
            ISecretaryRepository secretaryRepository,
            IExaminationSessionRepository examinationSessionRepository)
        {
            _mapper = mapper;
            _secretaryRepository = secretaryRepository;
            _examinationSessionRepository = examinationSessionRepository;
            _committeeRepository = committeeRepository;
            _studentRepository = studentRepository;
        }
        public async Task<ICollection<ExaminationSessionListForSecretary>> GetExaminationSessions(Guid userId)
        {
            var examinationSessions = await _examinationSessionRepository.GetExaminationSessionsWithStudentsAndCommittee(userId);
            var result = _mapper.Map<ICollection<ExaminationSessionListForSecretary>>(examinationSessions);
            return result;
        }

        public async Task<ExaminationSessionForSecretary> GetExaminationSession(Guid userId, Guid examinationSessionId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithBaseInformation(userId, examinationSessionId);
            var result = _mapper.Map<ExaminationSessionForSecretary>(examinationSession);
            return result;
        }

        public async Task<ExaminationSessionPresentationForSecretary> GetExaminationSessionForPresentation(Guid userId, Guid examinationSessionId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithStudentPresentation(userId, examinationSessionId);
            var presentingStudent = examinationSession.StudentPresentations.SingleOrDefault(s => s.Status == Domain.Enums.StudentPresentationStatus.Presenting);


            var result = new ExaminationSessionPresentationForSecretary()
            {
                Status = (int)examinationSession.Status,
                PresentationSchedule = new PresentationScheduleForSecretary()
                {
                    StudentPresentationDuration = examinationSession.PresentationSchedule?.StudentPresentationDuration
                },
                StudentPresentation = _mapper.Map<StudentPresentationForSecretary>(presentingStudent)
            };
            return result;
        }

        public async Task AddExaminationSession(Guid userId, InsertExaminationSessionRequest request)
        {
            var secretaryMember = await _secretaryRepository.GetByExternalId(userId);

            var committeesToAdd = new List<CommitteeMember>();
            var examinationSessionToAdd = ExaminationSession.Create(request.Name, request.StartDate, request.EndDate);
            secretaryMember.AddExaminationSession(examinationSessionToAdd);
            await _secretaryRepository.SaveChangesAsync();

            examinationSessionToAdd.SetSecretaryMemberId(secretaryMember.Id);

            foreach (var committeeMember in request.CommitteeMembers)
            {
                var committeeExists = await _committeeRepository.GetByEmailFirstOrDefault(committeeMember.Email);
                if (committeeExists != null)
                {
                    committeeExists.SetCurrentExaminationSessionId(examinationSessionToAdd.Id);
                    committeesToAdd.Add(committeeExists);
                }
                else
                {
                    var committeeToCreate = CommitteeMember.Create(committeeMember.Name, committeeMember.Email);
                    committeesToAdd.Add(committeeToCreate);
                    await _committeeRepository.SaveChangesAsync();

                    committeeToCreate.SetCurrentExaminationSessionId(examinationSessionToAdd.Id);
                }
            }
            examinationSessionToAdd.AddCommittees(committeesToAdd);

            secretaryMember.AddExaminationSession(examinationSessionToAdd);
            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task SetExaminationSessionStatus(Guid userId, Guid examinationSessionId, SessionStatus status)
        {
            var secretaryMember = await _examinationSessionRepository.GetExaminationSession(userId, examinationSessionId);
            var examinationSession = await _examinationSessionRepository.GetExaminationSession(userId, examinationSessionId);
            examinationSession.Status = status;
            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task<ICollection<ExaminationTicketForSecretary>> GetExaminationTicketsBySessionId(Guid id)
        {
            var examinationTickets = await _secretaryRepository.GetExaminationTicketsBySessionId(id);

            return _mapper.Map<ICollection<ExaminationTicketForSecretary>>(examinationTickets);
        }
        public async Task ImportExaminationTicketsFromAnotherSession(Guid toSessionId, InsertExaminationTicketBulkRequest request)
        {
            var tickets = await _secretaryRepository.GetExaminationTicketsBySessionId(request.ImportFromSessionId);
            List<ExaminationTicket> ticketsToImport = new List<ExaminationTicket>();
            foreach (var ticket in tickets)
            {
                var item = ExaminationTicket.Create(ticket.TicketNo, true, ticket.Question1, ticket.Question2, ticket.Question3,
                                                    ticket.Answer1, ticket.Answer2, ticket.Answer3);

                item.ExaminationSessionId = toSessionId;
                ticketsToImport.Add(item);
            }

            _secretaryRepository.AddExaminationTicketsBulk(ticketsToImport);
            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task RemoveExaminationTicketById(Guid id)
        {
            var examinationTicket = await _secretaryRepository.GetExaminationTicketById(id);
            _secretaryRepository.RemoveExaminationTicket(examinationTicket);
            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task AddTicketToSession(InsertTicketRequest request)
        {

            if (_secretaryRepository.CheckIfTicketNrExistsInSession(request.TicketNo, request.ExaminationSessionId))
            {
                throw new ArgumentException("A ticket with this number already exists in the session");
            }

            var examinationTicket = ExaminationTicket.Create(request.TicketNo, true, request.Question1, request.Question2, request.Question3, request.Answer1, request.Answer2, request.Answer3);
            examinationTicket.AssignTicketToSession(request.ExaminationSessionId);
            _secretaryRepository.AddExaminationTicket(examinationTicket);
            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task UpdateExaminationTicket(Guid id, UpdateTicketRequest request)
        {
            var examinationTicket = await _secretaryRepository.GetExaminationTicketById(id);
            if (examinationTicket.Question1 == request.Question1 && examinationTicket.Question2 == request.Question2 && examinationTicket.Question3 == request.Question3

            && examinationTicket.Answer1 == request.Answer1 && examinationTicket.Answer2 == request.Answer2 && examinationTicket.Answer3 == request.Answer3)
            {
                throw new ArgumentException("The entered values are the same as the values in the ticket");
            }

            examinationTicket.Question1 = request.Question1;
            examinationTicket.Question2 = request.Question2;
            examinationTicket.Question3 = request.Question3;
            examinationTicket.Answer1 = request.Answer1;
            examinationTicket.Answer2 = request.Answer2;
            examinationTicket.Answer3 = request.Answer3;
            _secretaryRepository.UpdateExaminationTicket(examinationTicket);
            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task<PresentationScheduleForSecretary?> GetPresentationSchedule(Guid examinationSessionId, Guid secretaryExternalId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithPresentationSchedule(secretaryExternalId, examinationSessionId);
            var presentationSchedule = examinationSession.PresentationSchedule;

            var presentationScheduleForSecretary = _mapper.Map<PresentationScheduleForSecretary>(presentationSchedule);

            if (presentationScheduleForSecretary != null)
            {
                presentationScheduleForSecretary.PresentationScheduleEntries =
                    presentationScheduleForSecretary.PresentationScheduleEntries
                    .OrderBy(pse => pse.Date)
                    .ToList();
            }
            return presentationScheduleForSecretary;
        }
        public async Task<ICollection<StudentPresentationForSecretary>> GetStudentPresentations(Guid examinationSessionId)
        {
            var studentPresentations = await _secretaryRepository.GetStudentPresentationsBySessionId(examinationSessionId);
            return _mapper.Map<ICollection<StudentPresentationForSecretary>>(studentPresentations);
        }
        public async Task GeneratePresentationSchedule(Guid examinationSessionId, Guid secretaryExternalId, InsertPresentationSchedule insertPresentationSchedule)
        {
            var secretary = await _secretaryRepository.GetByExternalId(secretaryExternalId);
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithStudentsAndSchedule(secretaryExternalId, examinationSessionId);

            if (examinationSession.Students.Count == 0)
                throw new Exception("There are no students in this examination session");

            if (examinationSession?.PresentationSchedule != null)
            {
                await _secretaryRepository.RemovePresentationSchedule(examinationSessionId);
            }

            var presentationSchedule = secretary.GeneratePresentationSchedule(
                examinationSessionId,
                insertPresentationSchedule.StartDate,
                insertPresentationSchedule.EndDate,
                insertPresentationSchedule.BreakStart,
                insertPresentationSchedule.BreakDuration,
                insertPresentationSchedule.StudentPresentationDuration);

            examinationSession?.SetPresentationSchedule(presentationSchedule);
            _secretaryRepository.AddPresentationSchedule(presentationSchedule);
            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task MovePresentationScheduleEntry(Guid examinationSessionId, Guid secretaryExternalId, MovePresentationScheduleEntryRequest movePresentationScheduleEntryRequest)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithPresentationSchedule(secretaryExternalId, examinationSessionId);
            var presentationSchedule = examinationSession.PresentationSchedule;


            var presentationScheduleEntries = presentationSchedule?.PresentationScheduleEntries.OrderBy(pse => pse.Date).ToList();
            int presentationScheduleEntryIndex = presentationScheduleEntries.FindIndex(entry => entry.Id == movePresentationScheduleEntryRequest.EntryId);
            var newIndex = movePresentationScheduleEntryRequest.Direction == "up" ? presentationScheduleEntryIndex - 1 : presentationScheduleEntryIndex + 1;

            if ((movePresentationScheduleEntryRequest.Direction == "up" && presentationScheduleEntryIndex > 0) ||
                (movePresentationScheduleEntryRequest.Direction == "down" && presentationScheduleEntryIndex < presentationScheduleEntries.Count - 1))
            {
                //swap entries in array
                var temp = presentationScheduleEntries[presentationScheduleEntryIndex];
                presentationScheduleEntries[presentationScheduleEntryIndex] = presentationScheduleEntries[newIndex];
                presentationScheduleEntries[newIndex] = temp;

                //swap entries date
                var tempDate = presentationScheduleEntries[presentationScheduleEntryIndex].Date;
                presentationScheduleEntries[presentationScheduleEntryIndex].Date = presentationScheduleEntries[newIndex].Date;
                presentationScheduleEntries[newIndex].Date = tempDate;
            }
            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task GenerateStudentPresentation(Guid examinationSessionId, Guid secretaryExternalId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithPresentationSchedule(secretaryExternalId, examinationSessionId);
            var studentInPresentationList = new List<StudentPresentation>();
            var presentationScheduleEntries = examinationSession.PresentationSchedule?.PresentationScheduleEntries.OrderBy(pse => pse.Date);
            bool makeFirstStudentPresenting = false;

            foreach (var entry in presentationScheduleEntries)
            {
                var studentPresentation = StudentPresentation.Create();
                studentPresentation.SetStudent(entry.Student);

                if (!makeFirstStudentPresenting)
                {
                    studentPresentation.SetStatus(Domain.Enums.StudentPresentationStatus.Presenting);
                    makeFirstStudentPresenting = true;
                }

                studentInPresentationList.Add(studentPresentation);
            }

            _secretaryRepository.AddStudentPresentations(studentInPresentationList);
            examinationSession.SetStudentPresentations(studentInPresentationList);
            await _secretaryRepository.SaveChangesAsync();
        }
        public async Task<StudentPresentationForSecretary> GetStudentPresentation(Guid userId, Guid examinationSessionId)
        {
            var secretary = await _secretaryRepository.GetByExternalId(userId);
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithTicketsAndStudentPresentationsForSecretary(userId, examinationSessionId);

            var studentPresentation = examinationSession.StudentPresentations.SingleOrDefault(sp => sp.Status == Domain.Enums.StudentPresentationStatus.Presenting);
            if (studentPresentation == null)
                return null;

            return _mapper.Map<StudentPresentationForSecretary>(studentPresentation);
        }

        public async Task UpdateAbsentStatus(Guid userId, Guid examinationSessionId, UpdateStudentPresentationRequest request)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithTicketsAndStudentPresentationsForSecretary(userId, examinationSessionId);
            var studentPresentation = examinationSession.StudentPresentations.First(sp => sp.Id == request.Id);
            studentPresentation.SetIsAbsent(request.IsAbsent);
            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task<ICollection<StudentForSecretaryDropdown>> GetRemainingStudentsToPresent(Guid userId, Guid examinationSessionId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithStudentPresentationAndSchedule(userId, examinationSessionId);
            var studentPresentations = examinationSession.StudentPresentations
                                                            .Where(sp => sp.Status == Domain.Enums.StudentPresentationStatus.Created && sp.StudentId != null)
                                                            .ToList();

            var allStudents = examinationSession.PresentationSchedule?.PresentationScheduleEntries
                                                            .OrderBy(pse => pse.Date)
                                                            .Where(pse => pse.StudentId != null)
                                                            .Select(pse => pse.Student)
                                                            .ToList();

            var remainingStudents = allStudents.Where(s => allStudents.Any(st => studentPresentations.Any(sp => sp.StudentId == s?.Id))).ToList();

            ICollection<StudentForSecretaryDropdown> result = new List<StudentForSecretaryDropdown>();
            foreach (var sp in remainingStudents)
            {
                result.Add(_mapper.Map<StudentForSecretaryDropdown>(sp));
            }

            return result;
        }

        public async Task ChooseNextStudent(Guid userId, Guid examinationSessionId, Guid nextStudentId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithStudentPresentation(userId, examinationSessionId);
            var studentPresentations = examinationSession.StudentPresentations;

            var currentStudent = studentPresentations.First(sp => sp.Status == Domain.Enums.StudentPresentationStatus.Presenting);
            currentStudent.SetStatus(Domain.Enums.StudentPresentationStatus.Completed);

            var nextStudent = studentPresentations.First(sp => sp.StudentId == nextStudentId);
            nextStudent.SetStatus(Domain.Enums.StudentPresentationStatus.Presenting);

            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task PublishGrades(Guid userId, Guid examinationSessionId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithStudentPresentation(userId, examinationSessionId);

            foreach (var studentPresentation in examinationSession.StudentPresentations)
            {
                studentPresentation.SetStatus(Domain.Enums.StudentPresentationStatus.Graded);
            }

            await _secretaryRepository.SaveChangesAsync();
        }

        public async Task<bool> CheckIfExaminationSessionHasSchedule(Guid secretaryExternalId, Guid examinationSessionId)
        {
            var examinationSession = await _examinationSessionRepository.GetExaminationSessionWithPresentationSchedule(secretaryExternalId, examinationSessionId);
            if (examinationSession.PresentationSchedule != null)
                return true;
            else return false;
        }
    }
}
