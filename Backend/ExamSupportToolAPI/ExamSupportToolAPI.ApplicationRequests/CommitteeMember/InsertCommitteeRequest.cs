
namespace ExamSupportToolAPI.ApplicationRequests.CommitteeMember
{
    public class InsertCommitteeRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsHeadOfCommittee { get; set; }
        public Guid ExaminationSessionId { get; set; }
    }
}
