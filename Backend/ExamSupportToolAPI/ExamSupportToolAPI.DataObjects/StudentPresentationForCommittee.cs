namespace ExamSupportToolAPI.DataObjects
{
    public class StudentPresentationForCommittee
    {
        public Guid Id { get; set; }
        public Boolean IsAbsent { get; set; }
        public DateTime StartingTime { get; set; }
        public StudentPresentationStatus Status { get; set; }
        public StudentForCommittee? Student { get; set; }
        public ExaminationTicketForCommittee? ExaminationTicket { get; set; }
        public int StudentPresentationDuration { get; set; }

        public Guid CurrentCommitteeId { get; set; }

    }
    public enum StudentPresentationStatus
    {
        Created,
        Presenting,
        Completed,
        Graded
    }
}
