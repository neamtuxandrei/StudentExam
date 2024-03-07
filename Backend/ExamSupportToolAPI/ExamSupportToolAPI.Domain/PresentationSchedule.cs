namespace ExamSupportToolAPI.Domain
{
    public class PresentationSchedule : BaseEntity
    {
        public Guid ExaminationSessionId { get; set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime BreakStart { get; private set; }
        public int BreakDuration { get; private set; }
        public int StudentPresentationDuration { get; private set; }

        private List<PresentationScheduleEntry> _presentationScheduleEntries = new List<PresentationScheduleEntry>();

        public IReadOnlyCollection<PresentationScheduleEntry> PresentationScheduleEntries => _presentationScheduleEntries;

        private PresentationSchedule() { }

        public static PresentationSchedule Create(DateTime startDate, DateTime endDate, DateTime breakStart, int breakDuration, int studentPresentationDuration)
        {
            var presentationSchedule = new PresentationSchedule();

            presentationSchedule.Id = Guid.NewGuid();
            presentationSchedule.StartDate = startDate;
            presentationSchedule.EndDate = endDate;
            presentationSchedule.BreakDuration = breakDuration;
            presentationSchedule.StudentPresentationDuration = studentPresentationDuration;
            presentationSchedule.BreakStart = breakStart;

            return presentationSchedule;
        }

        public void AddPresentationScheduleEntry(PresentationScheduleEntry presentationScheduleEntry)
        {
            _presentationScheduleEntries.Add(presentationScheduleEntry);
        }

        public void SetPresentationScheduleEntries(List<PresentationScheduleEntry> presentationScheduleEntries)
        {
            _presentationScheduleEntries = presentationScheduleEntries;
        }

        public void AddPresentationScheduleEntry(List<PresentationScheduleEntry> presentationScheduleEntries)
        {
            _presentationScheduleEntries.AddRange(presentationScheduleEntries);
        }
    }
}