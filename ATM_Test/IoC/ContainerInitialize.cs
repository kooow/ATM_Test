using ATM_Test.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ATM_Test.IoC;

/// <summary>
/// ContainerInitialize class is responsible for setting up the dependency injection container
/// </summary>
public static class ContainerInitialize
{
    /// <summary>
    /// Initialize method is used to set up the dependency injection container with the required services.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="config">Configuration</param>
    /// <param name="testInit">Test init</param>
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