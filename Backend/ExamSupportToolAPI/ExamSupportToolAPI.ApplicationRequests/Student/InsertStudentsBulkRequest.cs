namespace ExamSupportToolAPI.ApplicationRequests.Student
{
    public class InsertStudentsBulkRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AnonymizationCode { get; set; } = string.Empty;
        public decimal YearsAverageGrade { get; set; } = 0M;
        public string DiplomaProjectName { get; set; } = string.Empty;
        public string CoordinatorName { get; set; } = string.Empty;
    }
}
