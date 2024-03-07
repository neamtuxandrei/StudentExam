

namespace ExamSupportToolAPI.DataObjects
{
    public class StudentPresentationForSecretary
    {
        public Guid Id { get; set; }
        public Boolean IsAbsent { get;  set; }
        public DateTime StartingTime { get;  set; }
        public StudentForSecretary? Student { get; set; }
        public ExaminationTicketForSecretary? ExaminationTicket { get; set; }
    }
}
