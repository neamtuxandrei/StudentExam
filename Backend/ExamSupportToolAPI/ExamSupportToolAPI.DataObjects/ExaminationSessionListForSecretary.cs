namespace ExamSupportToolAPI.DataObjects
{
    public class ExaminationSessionListForSecretary
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int NumberOfStudents { get; set; }
        public int NumberOfCommitteeMembers { get; set; }
        public int NumberOfTickets { get; set; }
    }
}
