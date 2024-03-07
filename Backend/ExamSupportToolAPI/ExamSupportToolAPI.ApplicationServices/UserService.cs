using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ExamSupportToolAPI.ApplicationServices
{
    public class UserService : IUserService
    {
        private readonly ICommitteeRepository _committeeRepository;
        private readonly ISecretaryRepository _secretaryRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public UserService(
            ICommitteeRepository committeeRepository,
            ISecretaryRepository secretaryRepository,
            IStudentRepository studentRepository,
            IApplicationUserRepository applicationUserRepository
            )
        {
            _committeeRepository = committeeRepository;
            _secretaryRepository = secretaryRepository;
            _studentRepository = studentRepository;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<UserForRegister?> GetUserByEmail(string email)
        {
            var secretaryUser = await _secretaryRepository.GetByEmailFirstOrDefault(email);
            var committeeUser = await _committeeRepository.GetByEmailFirstOrDefault(email);
            var studentUser = await _studentRepository.GetByEmailFirstOrDefault(email);

            if (secretaryUser != null) { return new UserForRegister() { Id = secretaryUser.Id, Role = "Secretary" }; };
            if (committeeUser != null) { return new UserForRegister() { Id = committeeUser.Id, Role = "Committee" }; };
            if (studentUser != null) { return new UserForRegister() { Id = studentUser.Id, Role = "Student" }; };

            return null;
        }

        public async Task SetUserExternalId(UserForRegister user, Guid externalId)
        {
            switch (user.Role)
            {
                case "Secretary":
                    var secretaryUser = _secretaryRepository.GetByIdFirstOrDefault(user.Id);
                    secretaryUser?.SetExternalId(externalId);
                    await _secretaryRepository.SaveChangesAsync();
                    break;
                case "Committee":
                    var committeeUser = _committeeRepository.GetByIdFirstOrDefault(user.Id);
                    committeeUser?.SetExternalId(externalId);
                    await _committeeRepository.SaveChangesAsync();
                    break;
                case "Student":
                    var studentUser = _studentRepository.GetByIdFirstOrDefault(user.Id);
                    studentUser?.SetExternalId(externalId);
                    await _studentRepository.SaveChangesAsync();
                    break;
                default:
                    throw new Exception("Role doesn't exist.");
            }
        }

        public async Task RemoveUser(Guid userId)
        {
            await _applicationUserRepository.RemoveUser(userId);
        }
    }
}
