using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pirat.Notes.DAL.Implementations;
using Pirat.Notes.DAL.Implementations.Extensions;
using Pirat.Notes.Domain.Implementations.Extensions;
using Pirat.Notes.Shared.Authorization;
using Pirat.Notes.Shared.Helpers;
using Serilog;
using System.Collections.Generic;

using Swashbuckle;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using Microsoft.Extensions.Options;
using System.IO;
using System;

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pirat.Notes API" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

            });

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();

            services.AddAuthServices();

            services.AddRepositories();

            services.AddApplicationServices();

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pirat.Notes.Web v1"));
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