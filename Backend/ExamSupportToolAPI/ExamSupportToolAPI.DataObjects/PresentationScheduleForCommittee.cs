namespace ExamSupportToolAPI.DataObjects
{
    public class PresentationScheduleForCommittee
    {
        public Guid? Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime BreakStart { get; set; }
        public int BreakDuration { get; set; }
        public int StudentPresentationDuration { get; set; }
        public ICollection<PresentationScheduleEntryForCommittee> PresentationScheduleEntries { get; set; } = new List<PresentationScheduleEntryForCommittee>();
    }
}
