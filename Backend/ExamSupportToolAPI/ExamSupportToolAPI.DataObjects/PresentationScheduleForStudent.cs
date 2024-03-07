namespace ExamSupportToolAPI.DataObjects
{
    public class PresentationScheduleForStudent
    {
        public int StudentPresentationDuration { get; set; }
        public PresentationScheduleEntryForStudent? PresentationScheduleEntry { get; set; }
    }
}