using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ATM_Test
{
    public class Program
    {
     
        private static void CheckAppDataExistsAndCreateIfNot()
        {
            var currentDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists("App_Data"))
            {
                Directory.CreateDirectory("App_Data");
                Console.WriteLine("App_Data directory created in this directory:" + currentDir);
            }
        }

        public static void Main(string[] args)
        {
            CheckAppDataExistsAndCreateIfNot();

            CreateHostBuilder(args).Build().Run();
        }

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
}
