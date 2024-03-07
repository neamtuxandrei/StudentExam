namespace ExamSupportToolAPI.Abstractions
{
    public interface IApplicationUserRepository
    {
        Task RemoveUser(Guid userId);
    }
}
