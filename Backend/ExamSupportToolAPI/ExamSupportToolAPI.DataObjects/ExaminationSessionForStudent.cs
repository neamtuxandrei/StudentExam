namespace ExamSupportToolAPI.DataObjects
{
    public class ExaminationSessionForStudent
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public StudentPresentationForStudent? StudentPresentation { get; set; }
        public PresentationScheduleForStudent? PresentationSchedule { get; set; }
        
    }
}

