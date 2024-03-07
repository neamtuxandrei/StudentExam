namespace ExamSupportToolAPI.DataObjects
{
    public class StudentForCommittee
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AnonymizationCode { get; set; } = string.Empty;
        public decimal YearsAverageGrade { get; set; }
        public string DiplomaProjectName { get; set; } = string.Empty;
        public string CoordinatorName { get; set; } = string.Empty;
    }
}
