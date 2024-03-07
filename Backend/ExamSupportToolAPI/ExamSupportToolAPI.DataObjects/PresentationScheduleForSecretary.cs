namespace ExamSupportToolAPI.DataObjects
{
    public class PresentationScheduleForSecretary
    {
        public Guid? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? BreakStart { get; set; }
        public int? BreakDuration { get; set; }
        public int? StudentPresentationDuration { get; set; }
        public ICollection<PresentationScheduleEntryForSecretary>? PresentationScheduleEntries { get; set; } = new List<PresentationScheduleEntryForSecretary>();
    }
}
