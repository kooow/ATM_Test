using ATM_Test.IoC;
using ATM_Test.Models;
using ATM_Test.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Text;

namespace ATM_Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
                        Email = "kovacsrobert@windowslive.com"
                    },
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

                LogDatabaseModels(context.Set<DepositModel>(), logger);
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void LogDatabaseModels(DbSet<DepositModel> models, ILogger<Startup> logger)
        {
            StringBuilder logBuilder = new StringBuilder();

            var denomationUnits = Denomation.GetAll<Denomation>().Select(d => d.Unit).ToList();
            var depositModels = models.Where(dm => denomationUnits.Contains(dm.Unit)).ToList();

            logBuilder.AppendLine("------- Database ------");

            foreach (var model in depositModels)
            {
                logBuilder.AppendLine(model.Unit.ToString() + " - quantity:" + model.Quantity.ToString());
            }

            logBuilder.Append("------- Database ------");

            logger.LogInformation(logBuilder.ToString());
        }

    }
}
