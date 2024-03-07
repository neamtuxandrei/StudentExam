namespace ExamSupportToolAPI.ApplicationRequests.ExaminationSession
{
    public class InsertExaminationSessionRequest
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<CommitteeMemberForInsert> CommitteeMembers { get; set; }
    }
}