using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pirat.Notes.DAL.Implementations;
using Pirat.Notes.Web;

namespace Pirat.Notes.Tests.ConfigureHost
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<DataContext>));

                services.Remove(dbContextDescriptor);

                services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(
                        "Server=localhost;Database=TestDataBase;Trusted_Connection=True;");
                });
            });
        }
    }
}