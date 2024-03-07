namespace ExamSupportToolAPI.Domain
{
    public class User : BaseEntity
    {
        public Guid? ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        protected User()
        {

        }
        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The name must not be empty", "name");
            Name = name;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("The email must not be empty", "email");
            Email = email;
        }
        public void SetExternalId(Guid externalId)
        {
            ExternalId = externalId;
        }
    }
}
