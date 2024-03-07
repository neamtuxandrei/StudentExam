namespace ExamSupportToolAPI.DataObjects
{
    public class PresentationScheduleEntryForCommittee
    {
        public Guid? Id { get; set; }
        public DateTime Date { get; set; }
        public StudentInPresentationScheduleForCommittee? Student { get; set; }
    }
}
