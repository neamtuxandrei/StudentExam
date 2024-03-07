
namespace ExamSupportToolAPI.DataObjects
{
    public class StudentPresentationForStudent
    {
        public int Status { get; set; }
        public decimal TheoryGrade { get; set; }
        public decimal ProjectGrade { get; set; }
        public DateTime? StartingTime { get; set; }
        public ExaminationTicketForStudent? ExaminationTicket { get; set; }
    }
}
