using AutoMapper;
using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.ApplicationServices.Abstractions;
using ExamSupportToolAPI.ApplicationServices;
using ExamSupportToolAPI.ApplicationServices.Mappings;
using ExamSupportToolAPI.DataAccess.Repositories;
using ExamSupportTooAPI.DataAccess.Repositories;
using ExamSupportToolAPI.Data;
using RazorPagesReporting;

namespace ExamSupportToolAPI.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterApplication(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ISecretaryRepository, SecretaryRepository>();
            services.AddScoped<ICommitteeRepository, CommitteeRepository>();
            services.AddScoped<IExaminationSessionRepository, ExaminationSessionRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            services.AddScoped<ISecretaryService, SecretaryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICommitteeService, CommitteeService>();
            services.AddScoped<RazorPagesReportingEngine>();

            var config = new MapperConfiguration(c =>
            c.AddProfile<MappingProfile>());
            services.AddSingleton<IMapper>(s => config.CreateMapper());
        }
    }
}
