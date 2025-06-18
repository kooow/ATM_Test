using ATM_Test.Helpers;
using ATM_Test.IoC;
using ATM_Test.Models;
using ATM_Test.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace ATM_Test;

/// <summary>
/// Configures the application's startup behavior, including service registration and HTTP request pipeline setup.
/// </summary>
/// <remarks>The <see cref="Startup"/> class is used by the ASP.NET Core runtime to initialize and configure the
/// application. It defines methods for registering services and middleware components required for the
/// application.</remarks>
public class Startup
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configuration">Configuration</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Configuration settings for the application.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// ConfigureServices method is used to register services in the application's dependency injection container.
    /// </summary>
    /// <param name="services">Service collection</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEntityFrameworkSqlite().AddDbContext<APIDbContext>();

        ContainerInitialize.Init(services, Configuration);

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ATM Test API",
                Version = "v1",
                Description = "Description for the API goes here.",
                Contact = new OpenApiContact
                {
                    Name = "Robert Kovacs",
                    Email = ""
                },
            });
            c.SchemaFilter<EndpointExampleSchemaFilter>();
        });
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <param name="env">Web host environment</param>
    /// <param name="logger">Logger</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ATM_Test v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthorization();

        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<APIDbContext>();
            context.Database.EnsureCreated();

            var denomationUnits = Denomation.GetAll<Denomation>().Select(d => d.Unit).ToList();
            var bankNotes = context.Set<BankNote>().Where(bn => denomationUnits.Contains(bn.Value)).ToList();

            Helper.LogModels(bankNotes, logger);
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
