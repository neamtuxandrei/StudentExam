namespace ExamSupportToolAPI.ApplicationRequests.Student
{
    public class InsertStudentRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string AnonymizationCode { get; set; }
        public decimal YearsAverageGrade { get; set; }
        public string DiplomaProjectName { get; set; }
        public string CoordinatorName { get; set; }
        public Guid ExaminationSessionId { get; set; }
    }
}
