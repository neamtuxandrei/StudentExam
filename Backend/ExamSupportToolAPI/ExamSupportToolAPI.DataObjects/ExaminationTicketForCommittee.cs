namespace ExamSupportToolAPI.DataObjects
{
    public class ExaminationTicketForCommittee
    {
        public Guid Id { get; set; }
        public int TicketNo { get; set; }
        public bool IsActive { get; set; }
        public string Question1 { get; set; } = string.Empty;
        public string Question2 { get; set; } = string.Empty;
        public string Question3 { get; set; } = string.Empty;
        public string Answer1 { get; set; } = string.Empty;
        public string Answer2 { get; set; } = string.Empty;
        public string Answer3 { get; set; } = string.Empty;
    }
}
