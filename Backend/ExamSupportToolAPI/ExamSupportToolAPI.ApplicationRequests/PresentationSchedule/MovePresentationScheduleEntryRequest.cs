namespace ExamSupportToolAPI.ApplicationRequests.PresentationSchedule
{
    public class MovePresentationScheduleEntryRequest
    {
        public Guid EntryId { get; set; }
        public string Direction { get; set; } = string.Empty;
    }
}
