using System.Globalization;
using OpenIddict.Abstractions;
using ExamSupportToolAPI.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ExamSupportToolAPI
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            await RegisterApplicationsAsync(scope.ServiceProvider);

            static async Task RegisterApplicationsAsync(IServiceProvider provider)
            {
                var manager = provider.GetRequiredService<IOpenIddictApplicationManager>();

                // Angular UI client
                if (await manager.FindByClientIdAsync("examsupporttoolui") is null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "examsupporttoolui",
                        ConsentType = ConsentTypes.Explicit,
                        DisplayName = "Exam Support Tool UI",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("en-EN")] = "Angular UI Client"
                        },
                        PostLogoutRedirectUris =
                        {
                            new Uri("https://localhost:4200")
                        },
                        RedirectUris =
                        {
                            new Uri("https://localhost:4200")
                        },
                        Permissions =
                        {
                            Permissions.Endpoints.Authorization,
                            Permissions.Endpoints.Logout,
                            Permissions.Endpoints.Token,
                            Permissions.Endpoints.Revocation,
                            Permissions.GrantTypes.AuthorizationCode,
                            Permissions.GrantTypes.RefreshToken,
                            Permissions.ResponseTypes.Code,
                            Permissions.Scopes.Email,
                            Permissions.Scopes.Profile,
                            Permissions.Scopes.Roles,
                            Permissions.Prefixes.Scope
                        },
                        Requirements =
                        {
                            Requirements.Features.ProofKeyForCodeExchange
                        }
                    });
                }

                //swagger
                if (await manager.FindByClientIdAsync("swaggerui") is null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "swaggerui",
                        ConsentType = ConsentTypes.Explicit,
                        DisplayName = "Swagger UI",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("en-EN")] = "Swagger UI"
                        },
                        PostLogoutRedirectUris =
                        {
                            new Uri("https://localhost:44395/swagger/oauth2-redirect.html")
                        },
                        RedirectUris =
                        {
                            new Uri("https://localhost:44395/swagger/oauth2-redirect.html")
                        },
                        Permissions =
                        {
                            Permissions.Endpoints.Authorization,
                            Permissions.Endpoints.Logout,
                            Permissions.Endpoints.Token,
                            Permissions.Endpoints.Revocation,
                            Permissions.GrantTypes.AuthorizationCode,
                            Permissions.GrantTypes.RefreshToken,
                            Permissions.ResponseTypes.Code,
                            Permissions.Scopes.Email,
                            Permissions.Scopes.Profile,
                            Permissions.Scopes.Roles,
                            Permissions.Prefixes.Scope
                        },
                        Requirements =
                        {
                            Requirements.Features.ProofKeyForCodeExchange
                        }
                    });
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
