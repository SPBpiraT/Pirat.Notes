using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Domain.Contracts.Models.Users;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pirat.Notes.Tests
{
    [TestFixture]
    public class NoteControllerTests : BaseControllerTests
    {
        [Test]
        public async Task CheckStatus_SendRequest_ShouldReturnOk()
        {
            // Arrange

            // Act

            var response = await _client.GetAsync("note/check-status");

            // Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetById_SendRequest_ShouldReturnOk()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            var model = new NoteModel() { Note = "i suck" };

            // Act
       
            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var json = authResponse.Content.ReadAsStringAsync().Result;

            var user = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            await _client.PostAsJsonAsync("note/create", model);

            var note = _dbContext.Notes.ToList().Find(x => x.UserName == _userRegisterRequest.Username);

            var response = await _client.GetAsync($"note/{note.Id}");

            await _client.DeleteAsync($"note/deletenote/{note.Id}");

            // Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            
        }

        [Test]
        public async Task GetById_SendRequest_ShouldReturnNotFound()
        {
            // Arrange

            // Act

            var response = await _client.GetAsync("note/0");

            // Assert

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetAll_SendRequest_ShouldReturnEmptyList()
        {
            //Arrange

            _client.DefaultRequestHeaders.Clear();

            var emptyList = new List<NoteModel>();

            // Act

            var response = await _client.GetAsync("note/");

            var jsonResponseList = await _client.GetFromJsonAsync<List<NoteModel>>("note/");

            if (jsonResponseList.Any()) 
            {
                var authResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

                var json = authResponse.Content.ReadAsStringAsync().Result;

                TestUser user = JsonSerializer.Deserialize<TestUser>(json);

                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

                foreach (var note in jsonResponseList)
                {
                    await _client.DeleteAsync($"admin/deletenote/{note.Id}");
                }

                jsonResponseList = await _client.GetFromJsonAsync<List<NoteModel>>("note/");
            }

            // Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Assert.AreEqual(emptyList, jsonResponseList);
        }

        [Test]
        public async Task Create_SendRequest_ShouldReturnBadRequest()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            var model = new NoteModel() { Note = "" };

            // Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var json = authResponse.Content.ReadAsStringAsync().Result;

            var user = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            var response = await _client.PostAsJsonAsync("note/create", model);

            // Assert

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Create_SendRequest_ShouldReturnOK()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            var model = new NoteModel() { Note = "hello" };

            // Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var json = authResponse.Content.ReadAsStringAsync().Result;

            var user = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            var response = await _client.PostAsJsonAsync("note/create", model);

            var note = _dbContext.Notes.ToList().Find(x => x.UserName == _userRegisterRequest.Username);

            await _client.DeleteAsync($"note/deletenote/{note.Id}");

            // Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            
        }
    }
}