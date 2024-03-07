namespace ExamSupportToolAPI.Domain
{
    public class PresentationScheduleEntry : BaseEntity
    {
        public Guid PresentationScheduleId { get; set; }
        public DateTime Date { get; set; }
        public Guid? StudentId { get; set; }
        public Student? Student { get; set; }

        private PresentationScheduleEntry() { }

        public static PresentationScheduleEntry Create(DateTime date)
        {
            PresentationScheduleEntry presentationScheduleEntry = new PresentationScheduleEntry();

            presentationScheduleEntry.Id = Guid.NewGuid();
            presentationScheduleEntry.Date = date;

            return presentationScheduleEntry;
        }
        
        public void SetStudent(Student? student)
        {
            Student = student;
        }
    }
}
