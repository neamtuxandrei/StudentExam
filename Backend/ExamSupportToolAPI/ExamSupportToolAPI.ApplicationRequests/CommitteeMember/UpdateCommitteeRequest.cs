
namespace ExamSupportToolAPI.ApplicationRequests.CommitteeMember
{
    public class UpdateCommitteeRequest
    {
        public string Name { get; set; }
        public bool IsHeadOfCommittee { get; set; }
        public Guid ExaminationSessionId { get; set; }
    }
}
