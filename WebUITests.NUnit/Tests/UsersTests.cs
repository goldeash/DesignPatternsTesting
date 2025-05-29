using Allure.NUnit.Attributes;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUITests.NUnit.Models;
using WebUITests.NUnit.Utilities;

namespace WebUITests.NUnit.Tests
{
    [TestFixture]
    [AllureFeature("Users API")]
    public class UsersTests : BaseApiTest
    {
        private User _testUser;
        private string _createdUserId;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _testUser = new User.UserBuilder()
                .WithUsername($"testuser_{DateTime.Now.Ticks}")
                .WithPassword("TestPass123")
                .WithEmail($"test_{DateTime.Now.Ticks}@example.com")
                .IsAdmin(false)
                .Build();
        }

        [Test]
        [Order(1)]
        [AllureStory("Create User")]
        public async Task CreateUser_ValidData_ReturnsCreated()
        {
            var response = await ApiClient.PostAsync("/Users", _testUser);
            var responseUser = await response.Content.ReadFromJsonAsync<User>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(responseUser.Username, Is.EqualTo(_testUser.Username));

            _createdUserId = responseUser.Id;
        }

        [Test]
        [Order(2)]
        [AllureStory("Get User")]
        public async Task GetUserById_ValidId_ReturnsUser()
        {
            Assume.That(_createdUserId, Is.Not.Null);

            var response = await ApiClient.GetAsync($"/Users/{_createdUserId}");
            var responseUser = await response.Content.ReadFromJsonAsync<User>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseUser.Id, Is.EqualTo(_createdUserId));
        }

        [Test]
        [Order(3)]
        [AllureStory("Update User")]
        public async Task UpdateUser_ValidData_ReturnsNoContent()
        {
            Assume.That(_createdUserId, Is.Not.Null);
            var updatedUser = new User.UserBuilder()
                .WithUsername($"updated_{DateTime.Now.Ticks}")
                .WithEmail($"updated_{DateTime.Now.Ticks}@example.com")
                .Build();

            var response = await ApiClient.PutAsync($"/Users/{_createdUserId}", updatedUser);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        [Order(4)]
        [AllureStory("Delete User")]
        public async Task DeleteUser_ValidId_ReturnsNoContent()
        {
            Assume.That(_createdUserId, Is.Not.Null);

            var response = await ApiClient.DeleteAsync($"/Users/{_createdUserId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}