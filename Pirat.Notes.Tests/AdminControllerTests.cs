using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Pirat.Notes.Domain.Contracts.Models.Users;
using System.Net.Http.Json;
using System.Text.Json;
using Pirat.Notes.Domain.Contracts.Models.Notes;

namespace Pirat.Notes.Tests
{
    [TestFixture]
    public class AdminControllerTests : BaseControllerTests
    {
        [Test]
        public async Task CheckStatus_SendRequest_ShouldReturnOk() 
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            //Act

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var json = authResponse.Content.ReadAsStringAsync().Result;

            TestUser user = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            HttpResponseMessage response = await _client.GetAsync("admin/check-status");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task DeleteUser_SendRequest_ShouldReturnOk() 
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var json = authResponse.Content.ReadAsStringAsync().Result;

            TestUser user = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            var testUser = _dbContext.Users.ToList().Find(x => x.Username == "Tester");

            HttpResponseMessage response = await _client.DeleteAsync($"admin/deleteuser/{testUser.Id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task DeleteNote_SendRequest_ShouldReturnOk() 
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var userJson = authResponse.Content.ReadAsStringAsync().Result;

            TestUser user = JsonSerializer.Deserialize<TestUser>(userJson);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            await _client.PostAsJsonAsync("note/create", _noteModel);

            _client.DefaultRequestHeaders.Clear();

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var json = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            var testNote = _dbContext.Notes.ToList().Find(x => x.UserName == "Tester");

            HttpResponseMessage response = await _client.DeleteAsync($"admin/deletenote/{testNote.Id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task SetUserRole_SendRequest_ShouldReturnOk() 
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            var userRoleUpdateRequest = new UserRoleUpdateRequest()
            {
                UserRole = "1"
            };

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest); //

            var json = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(json); //Можно вынести в метод аутентификации в базовый класс. Будет принимать 
                                                                             //authRequest ниче не возвращать а тока добавлять хедер. В начале будет очищать хедеры.

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}"); //

            var testUser = _dbContext.Users.ToList().Find(x => x.Username == "Tester");

            HttpResponseMessage response = await _client.PutAsJsonAsync($"admin/setuserrole/{testUser.Id}", userRoleUpdateRequest);

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task UpdateNote_SendRequest_ShouldReturnOk()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            var noteUpdateRequest = new NoteUpdateRequest()
            {
                Note = "hello"
            };

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var userAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var userJson = userAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser user = JsonSerializer.Deserialize<TestUser>(userJson);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            await _client.PostAsJsonAsync("note/create", _noteModel);

            _client.DefaultRequestHeaders.Clear();

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var json = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            var testNote = _dbContext.Notes.ToList().Find(x => x.UserName == "Tester");

            HttpResponseMessage response = await _client.PutAsJsonAsync($"admin/updatenote/{testNote.Id}", noteUpdateRequest);

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task UpdateUser_SendRequest_ShouldReturnOk()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            var userUpdateRequest = new UpdateRequest()
            {
                FirstName = "Patrick",
                LastName = "Patrick",
                Username = "Patrick",
                Password = "Pat"
            };

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var json = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            var testUser = _dbContext.Users.ToList().Find(x => x.Username == "Tester");

            HttpResponseMessage response = await _client.PutAsJsonAsync($"admin/updateuser/{testUser.Id}", userUpdateRequest);

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            await _client.DeleteAsync($"admin/deleteuser/{testUser.Id}");
        }
    }
}
