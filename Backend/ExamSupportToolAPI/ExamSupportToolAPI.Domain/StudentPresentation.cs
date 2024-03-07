using ExamSupportToolAPI.Domain.Enums;

namespace ExamSupportToolAPI.Domain
{
    public class StudentPresentation: BaseEntity
    {
        public Guid ExaminationSessionId { get; private set; }
        public Guid? StudentId { get; private set; }
        public Student? Student { get; private set; }
        public Guid? ExaminationTicketId { get; private set; }
        public ExaminationTicket? ExaminationTicket { get; private set; }
        public Boolean IsAbsent { get; private set; } 
        public StudentPresentationStatus Status { get; private set; }
        public DateTime? StartingTime { get; private set; }
        private List<CommitteeMemberGrade> _committeeMemberGrades = new();
        public IReadOnlyCollection<CommitteeMemberGrade> CommitteeMemberGrades => _committeeMemberGrades;

        public decimal TheoryGrade { get; private set; }
        public decimal ProjectGrade { get; private set; }
        private StudentPresentation() { }
        public static StudentPresentation Create()
        {
            StudentPresentation studentPresentation = new StudentPresentation();
            studentPresentation.Id = Guid.NewGuid();
            studentPresentation.IsAbsent = false;
            studentPresentation.Status = StudentPresentationStatus.Created;
            studentPresentation.StartingTime = null;

            return studentPresentation;
        }
        public void SetStudent(Student? student)
        {
            Student = student;
        }

        public void AssignStudentGrade(Guid committeeMemberId, int theoryGrade, int projectGrade)
        {
            var committeeGrade = _committeeMemberGrades
                                                    .Where(cg => cg.CommitteeMemberId == committeeMemberId)
                                                    .FirstOrDefault();
            if (committeeGrade == null) 
            {
                committeeGrade = CommitteeMemberGrade.Create(committeeMemberId, theoryGrade, projectGrade);
                _committeeMemberGrades.Add(committeeGrade);
                
            }
            else
                committeeGrade.UpdateGrades(theoryGrade, projectGrade);

            TheoryGrade = (decimal)_committeeMemberGrades.Average(cg => cg.TheoryGrade);
            ProjectGrade = (decimal)_committeeMemberGrades.Average(cg => cg.ProjectGrade);
        }
            
    
   

        public void SetExaminationTicket(ExaminationTicket examinationTicket)
        {
            ExaminationTicket = examinationTicket;
            ExaminationTicketId = examinationTicket.Id;
        }
        public void SetStatus(StudentPresentationStatus status)
        {
            Status = status;
        }

        public void SetStartTimer(DateTime startTimer)
        {
            StartingTime = startTimer;
        }

        public void SetIsAbsent(Boolean isAbsent)
        {
            IsAbsent = isAbsent;
        }
    }

}
