using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using Pirat.Notes.Domain.Contracts.Models.Users;

namespace Pirat.Notes.Tests
{
    [TestFixture]
    public class UserControllerTests : BaseControllerTests
    {
        [Test]
        public async Task Authenticate_SendRequest_ShouldReturnOk()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var response = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            _client.DefaultRequestHeaders.Clear();

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var json = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            var testUser = _dbContext.Users.ToList().Find(x => x.Username == "Tester");

            await _client.DeleteAsync($"admin/deleteuser/{testUser.Id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Register_SendRequest_ShouldReturnOk()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            //Act

            var response = await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            _client.DefaultRequestHeaders.Clear();

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var json = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            var testUser = _dbContext.Users.ToList().Find(x => x.Username == "Tester");

            await _client.DeleteAsync($"admin/deleteuser/{testUser.Id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetAll_SendRequest_ShouldReturnOk()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var userJson = authResponse.Content.ReadAsStringAsync().Result;

            var user = JsonSerializer.Deserialize<TestUser>(userJson);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            var response = await _client.GetAsync("user/");

            _client.DefaultRequestHeaders.Clear();

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var adminJson = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(adminJson);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            var testUser = _dbContext.Users.ToList().Find(x => x.Username == "Tester");

            await _client.DeleteAsync($"admin/deleteuser/{testUser.Id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [Test]
        public async Task GetById_SendRequest_ShouldReturnOk()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var userJson = authResponse.Content.ReadAsStringAsync().Result;

            TestUser user = JsonSerializer.Deserialize<TestUser>(userJson);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            var testUser = _dbContext.Users.ToList().Find(x => x.Username == "Tester");

            var response = await _client.GetAsync($"user/{testUser.Id}");

            _client.DefaultRequestHeaders.Clear();

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var adminJson = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(adminJson);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            await _client.DeleteAsync($"admin/deleteuser/{testUser.Id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Update_SendRequest_ShouldReturnOk()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            var updateRequestModel = new UpdateRequest
            {
                FirstName = "Testy",
                LastName = "Test",
                Username = "Hornblower",
                Password = "Test"
            };

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var json = authResponse.Content.ReadAsStringAsync().Result;

            TestUser user = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            var testUser = _dbContext.Users.ToList().Find(x => x.Username == "Tester");

            var response = await _client.PutAsJsonAsync($"user/updateuser/{testUser.Id}", updateRequestModel);

            _client.DefaultRequestHeaders.Clear();

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var adminJson = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(adminJson);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            await _client.DeleteAsync($"admin/deleteuser/{testUser.Id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Delete_SendRequest_ShouldReturnOk()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var json = authResponse.Content.ReadAsStringAsync().Result;

            TestUser user = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            var testUser = _dbContext.Users.ToList().Find(x => x.Username == "Tester");

            var response = await _client.DeleteAsync($"user/{testUser.Id}");

            _client.DefaultRequestHeaders.Clear();

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var adminJson = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(adminJson);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            await _client.DeleteAsync($"admin/deleteuser/{testUser.Id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Delete_SendRequest_ShouldReturn403Forbidden()
        {
            // Arrange

            _client.DefaultRequestHeaders.Clear();

            var testRegisterModel = new RegisterRequest
            {
                FirstName = "Tester1",
                LastName = "Fortests",
                Username = "Tester1",
                Password = "Test"
            };

            //Act

            await _client.PostAsJsonAsync("user/register", _userRegisterRequest);

            await _client.PostAsJsonAsync("user/register", testRegisterModel);

            var authResponse = await _client.PostAsJsonAsync("user/authenticate", _userAuthenticateRequest);

            var json = authResponse.Content.ReadAsStringAsync().Result;

            TestUser user = JsonSerializer.Deserialize<TestUser>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.token}");

            var toDeleteUserEntity = _dbContext.Users.ToList().Find(x => x.Username == testRegisterModel.Username);

            var activeUserEntity = _dbContext.Users.ToList().Find(x => x.Username == _userRegisterRequest.Username);

            var response = await _client.DeleteAsync($"user/{toDeleteUserEntity.Id}");

            _client.DefaultRequestHeaders.Clear();

            var adminAuthResponse = await _client.PostAsJsonAsync("user/authenticate", _adminAuthenticateRequest);

            var adminJson = adminAuthResponse.Content.ReadAsStringAsync().Result;

            TestUser admin = JsonSerializer.Deserialize<TestUser>(adminJson);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {admin.token}");

            await _client.DeleteAsync($"admin/deleteuser/{activeUserEntity.Id}");

            await _client.DeleteAsync($"admin/deleteuser/{toDeleteUserEntity.Id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}