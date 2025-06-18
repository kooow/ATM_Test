using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ATM_Test;

/// <summary>
/// Program class serves as the entry point for the ATM Test application.
/// </summary>
public class Program
{
    private const string AppDataDirectory = "App_Data";

    private static void CheckAppDataExistsAndCreateIfNot()
    {
        if (!Directory.Exists(AppDataDirectory))
        {
            Directory.CreateDirectory(AppDataDirectory);
            Console.WriteLine($"{nameof(AppDataDirectory)} directory created in this directory: {Directory.GetCurrentDirectory()}");
        }
    }

    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    /// <param name="args">Arguments</param>
    public static void Main(string[] args)
    {
        CheckAppDataExistsAndCreateIfNot();
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Host builder configuration for the application.
    /// </summary>
    /// <param name="args">Arguments</param>
    /// <returns>Host builder</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
             .ConfigureLogging(logging =>
             {
                 logging.ClearProviders();
                 logging.AddConsole();
             })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
