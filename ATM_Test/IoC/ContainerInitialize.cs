
using ATM_Test.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ATM_Test.IoC
{
    public static class ContainerInitialize
    {
        public static void Init(IServiceCollection services, IConfiguration config, bool testInit = false)
        {
            SetupServices(services);
        }

        private static void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IDepositService, DepositService>();
            services.AddScoped<IWithdrawService, WithdrawService>();
        }
    }
}