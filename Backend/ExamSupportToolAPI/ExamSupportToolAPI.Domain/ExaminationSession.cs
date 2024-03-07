using ExamSupportToolAPI.Domain.Enums;

namespace ExamSupportToolAPI.Domain
{
    public class ExaminationSession
    {
        public Guid Id { get; private set; }
        public Guid SecretaryMemberId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SessionStatus Status { get; set; }
        public Guid? PresentationScheduleId { get; set; }

        public PresentationSchedule? PresentationSchedule {get; set;}
        public Guid? HeadOfCommitteeMemberId { get; set; }

        private List<StudentPresentation> _studentPresentations = new List<StudentPresentation>();
        public IReadOnlyCollection<StudentPresentation> StudentPresentations => _studentPresentations;

        private List<Student> _students = new List<Student>();
        public IReadOnlyCollection<Student> Students => _students;

        private List<CommitteeMember> _committees = new List<CommitteeMember>();
        public IReadOnlyCollection<CommitteeMember> CommitteeMembers => _committees;

        private List<ExaminationTicket> _examinationTickets = new List<ExaminationTicket>();
        public IReadOnlyCollection<ExaminationTicket> ExaminationTickets => _examinationTickets;

        private ExaminationSession() { }
        
        public static ExaminationSession Create(string name, DateTime startDate,DateTime endDate)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The name must not be empty", "name");
            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date");

            ExaminationSession examinationSession = new ExaminationSession();
            examinationSession.Name = name;
            examinationSession.StartDate = startDate;
            examinationSession.EndDate = endDate;
            examinationSession.Status = SessionStatus.Created;

            return examinationSession;
        }
        public void AddStudents(IEnumerable<Student> students)
        {
            _students.AddRange(students);
        }
        public void AddStudent(Student student)
        {
            _students.Add(student);
        }
        public void AddCommittees(IEnumerable<CommitteeMember> committees)
        {
            _committees.AddRange(committees);
        }
        public void AddCommitee(CommitteeMember committee)
        {
            _committees.Add(committee);
        }


        public void RemoveCommittee(CommitteeMember member)
        {
            _committees.Remove(member);
        }

        public void AddExaminationTickets(IEnumerable<ExaminationTicket> tickets)
        {
            _examinationTickets.AddRange(tickets);
        }

        public void AddExaminationTicket(ExaminationTicket ticket)
        {
            _examinationTickets.Add(ticket);
        }

        public void SetPresentationSchedule(PresentationSchedule presentationSchedule)
        {
            presentationSchedule.ExaminationSessionId = Id;
            PresentationSchedule = presentationSchedule;
        }

        public void SetSecretaryMemberId(Guid secretaryMemberId)
        {
            this.SecretaryMemberId = secretaryMemberId;
        }

        public void SetHeadOfCommittee(Guid? committeeMemberId)
        {
            this.HeadOfCommitteeMemberId = committeeMemberId;
        }

        public void SetHeadOfCommitteeToNull()
        {
            this.HeadOfCommitteeMemberId = null;
        }

        public bool CommitteeMemberExists(CommitteeMember member)
        {
            return _committees.Contains(member);
        }

        public void AddStudentPresentation(StudentPresentation studentPresentation)
        {
            _studentPresentations.Add(studentPresentation);
        }

        public void SetStudentPresentations(List<StudentPresentation> studentPresentations)
        {
            _studentPresentations = studentPresentations;
        }

    }
}
