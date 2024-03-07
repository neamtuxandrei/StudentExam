namespace ExamSupportToolAPI.DataObjects
{
    public class ExaminationTicketForStudent
    {
        public int TicketNo { get; set; }
        public string Question1 { get; set; } = string.Empty;
        public string Question2 { get; set; } = string.Empty;
        public string Question3 { get; set; } = string.Empty;
    }
}
