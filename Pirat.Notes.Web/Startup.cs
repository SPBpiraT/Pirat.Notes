using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pirat.Notes.DAL.Contracts;
using Pirat.Notes.DAL.Implementations;
using Pirat.Notes.DAL.Implementations.Extensions;
using Pirat.Notes.Domain.Contracts.Interfaces;
using Pirat.Notes.Domain.Implementations.Extensions;
using Pirat.Notes.Domain.Implementations.Services;
using Pirat.Notes.Shared.Authorization;
using Pirat.Notes.Shared.Helpers;
using Serilog;

namespace Pirat.Notes.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options
                    .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                        assembly =>
                            assembly.MigrationsAssembly("Pirat.Notes.DAL.Migrations"));
            });

            services.AddCors();

            services.AddControllers();

            services.AddHttpClient(); // why?

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Startup));

            services.AddAuthServices();

            services.AddRepositories();

            services.AddApplicationServices();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Write streamlined request completion events, instead of the more verbose ones from the framework.
            // To use the default framework request logging instead, remove this line and set the "Microsoft"
            // level in appsettings.json to "Information".

            app.UseSerilogRequestLogging();

            // migrate any database changes on startup (includes initial db creation)
            //dataContext.Database.Migrate();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // global error handler

            app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware

            app.UseJwtAuth();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}