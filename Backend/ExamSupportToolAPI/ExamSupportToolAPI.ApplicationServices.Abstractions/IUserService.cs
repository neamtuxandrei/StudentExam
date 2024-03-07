using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain;

namespace ExamSupportToolAPI.Abstractions
{
    public interface IUserService
    {
        Task<UserForRegister?> GetUserByEmail(string email);
        Task SetUserExternalId(UserForRegister user, Guid externalId);
        Task RemoveUser(Guid userId);
    }
}
