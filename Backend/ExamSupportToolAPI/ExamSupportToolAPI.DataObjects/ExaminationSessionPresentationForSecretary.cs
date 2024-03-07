
namespace ExamSupportToolAPI.DataObjects
{
    public class ExaminationSessionPresentationForSecretary
    {
        public int Status { get; set; }
        public PresentationScheduleForSecretary? PresentationSchedule { get; set; }
        public StudentPresentationForSecretary? StudentPresentation {  get; set; }
    }
}
