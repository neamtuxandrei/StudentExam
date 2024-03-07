namespace ExamSupportToolAPI.DataObjects
{
    public class CommitteeMemberData
    {
        public Guid? Id { get; set; }
        //public bool IsHeadOfCommittee { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
