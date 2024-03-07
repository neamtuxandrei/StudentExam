namespace ExamSupportToolAPI.DataObjects
{
    public class ExaminationSessionForSecretary
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public Guid? HeadOfCommitteeMemberId { get; set; }

        public ICollection<StudentForSecretary> Students { get; set; } = new List<StudentForSecretary>();
        public ICollection<CommitteeMemberData> CommitteeMembers { get; set; } = new List<CommitteeMemberData>();
        public ICollection<ExaminationTicketForSecretary> ExaminationTickets { get; set; } = new List<ExaminationTicketForSecretary>();
        public PresentationScheduleForSecretary? PresentationSchedule { get; set; }
    }
}
