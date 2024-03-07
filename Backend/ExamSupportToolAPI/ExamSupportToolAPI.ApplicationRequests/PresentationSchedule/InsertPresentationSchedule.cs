using ExamSupportToolAPI.DataObjects; 

namespace ExamSupportToolAPI.ApplicationRequests.PresentationSchedule
{
    public class InsertPresentationSchedule
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? BreakStart { get; set; }
        public int? BreakDuration { get; set; }
        public int? StudentPresentationDuration { get; set; }
        public PresentationScheduleForSecretary? PresentationSchedule { get; set; } 
    }
}
