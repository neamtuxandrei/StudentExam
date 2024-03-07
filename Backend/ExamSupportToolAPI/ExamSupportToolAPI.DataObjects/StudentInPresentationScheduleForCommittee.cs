namespace ExamSupportToolAPI.DataObjects
{
    public class StudentInPresentationScheduleForCommittee
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DiplomaProjectName { get; set; } = string.Empty;
        public string CoordinatorName { get; set; } = string.Empty;
    }
}
