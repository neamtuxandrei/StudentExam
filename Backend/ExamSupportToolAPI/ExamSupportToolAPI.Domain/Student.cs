using System.ComponentModel.DataAnnotations;

namespace ExamSupportToolAPI.Domain
{
    public class Student : User
    {
        private List<ExaminationSession> _examinationSessions = new List<ExaminationSession>();
        public IReadOnlyCollection<ExaminationSession> ExaminationSessions => _examinationSessions;

        private List<PresentationScheduleEntry> _presentationScheduleEntries = new List<PresentationScheduleEntry>();
        public IReadOnlyCollection<PresentationScheduleEntry> PresentationScheduleEntries => _presentationScheduleEntries;
        public string AnonymizationCode { get; set; } = string.Empty;
        public decimal YearsAverageGrade { get; set; }
        public string DiplomaProjectName { get; set; } = string.Empty;
        public string CoordinatorName { get; set; } = string.Empty;
        public Guid? CurrentExaminationSessionId { get; private set; }
        public Guid? CurrentPresentationScheduleId { get; private set; }
        private Student() { }

        public static Student Create(string name, string email, string anonymizationCode, decimal yearsAverageGrade,
            string diplomaProjectName, string coordinatorName)
        {
            Student student = new Student();
            student.SetName(name);
            student.SetEmail(email);
            student.AnonymizationCode = anonymizationCode;
            student.YearsAverageGrade = yearsAverageGrade;
            student.DiplomaProjectName = diplomaProjectName;
            student.CoordinatorName = coordinatorName;
            student.Id = Guid.NewGuid();
            return student;
        }

        public ExaminationTicket GenerateExaminationTicket()
        {
            var examinationSession = _examinationSessions.Where(es => es.Id == CurrentExaminationSessionId).First();
            var examinationTicketList = examinationSession.ExaminationTickets.Where(et => et.IsActive == true).ToList();

            if (examinationTicketList.Count == 0)
                throw new InvalidOperationException("There are no tickets available");

            Random random = new Random();
            
            var ticketNo = random.Next(0, examinationTicketList.Count);

            return examinationTicketList[ticketNo];
        }

        public void SetCurrentExaminationSessionId(Guid? id)
        {
            CurrentExaminationSessionId = id;
        }
        public void SetCurrentPresentationScheduleId(Guid? id)
        {
            CurrentPresentationScheduleId = id;
        }
        public void AddExaminationSession(ExaminationSession examinationSession)
        {
            _examinationSessions.Add(examinationSession);
        }
    }
}
