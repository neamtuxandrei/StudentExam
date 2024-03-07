namespace ExamSupportToolAPI.Domain
{
    public class CommitteeMember : User
    {
        private List<ExaminationSession> _examinationSessions = new List<ExaminationSession>();
        public IReadOnlyCollection<ExaminationSession> ExaminationSessions => _examinationSessions;
        public Guid CurrentExaminationSessionId { get; set; }
        public CommitteeMember() { }
        public static CommitteeMember Create(string name, string email)
        {
            CommitteeMember committee = new CommitteeMember();
            committee.SetName(name);
            committee.SetEmail(email);
            return committee;
        }

        public void SetCurrentExaminationSessionId(Guid id)
        {
            CurrentExaminationSessionId = id;
        }
        public void AssignStudentGrade(Student student, int theory, int project)
        { 
            
        }
    }
}

