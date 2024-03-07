namespace ExamSupportToolAPI.DataObjects
{
    public class PresentationScheduleEntryForSecretary
    {
        public Guid? Id { get; set; }
        public DateTime Date { get; set; }
        public StudentInPresentationScheduleForSecretary? Student { get; set; }
    }
}