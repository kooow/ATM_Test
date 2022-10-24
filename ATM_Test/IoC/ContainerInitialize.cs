
using ATM_Test.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ATM_Test.IoC
{
    public static class ContainerInitialize
    {
        public static void Init(IServiceCollection services, IConfiguration config, bool testInit = false)
        {
            SetupRepositories(services);
        }

        private static void SetupRepositories(IServiceCollection services)
        {
            services.AddScoped<IDepositService, DepositService>();
        }
    }
}