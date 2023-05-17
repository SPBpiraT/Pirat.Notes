using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Pirat.Notes.DAL.Implementations;
using Pirat.Notes.Domain.Contracts.Models.Users;
using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Tests.ConfigureHost;
using Pirat.Notes.Web;
using System.Transactions;


namespace Pirat.Notes.Tests
{
    public class BaseControllerTests
    {
        protected readonly HttpClient _client;

        protected readonly DataContext _dbContext;

        protected readonly CustomWebApplicationFactory<Program> _factory;

        protected readonly RegisterRequest _userRegisterRequest = new ()
        {
            FirstName = "Tester",
            LastName = "Tester",
            Username = "Tester",
            Password = "Test"
        };

        protected readonly NoteModel _noteModel = new()
        {
            Note = "test"
        };

        protected readonly AuthenticateRequest _userAuthenticateRequest = new ()
        {
            Username = "Tester",
            Password = "Test"
        };

        protected readonly AuthenticateRequest _adminAuthenticateRequest = new ()
        {
            Username = "Admin",
            Password = "Admin"
        };

        public BaseControllerTests()
        {
            _factory = new CustomWebApplicationFactory<Program>();

            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _dbContext = _factory.Services.CreateScope().ServiceProvider.GetService<DataContext>();
        }
    }
}