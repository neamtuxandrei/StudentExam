namespace ExamSupportToolAPI.ApplicationServices.Errors
{
    public class CommitteeHeadAlreadyExistsException : Exception
    {
        public CommitteeHeadAlreadyExistsException(string message = "Committee head already exists.") : base(message)
        {
        }
    }
}
