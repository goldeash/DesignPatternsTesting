using Allure.NUnit.Attributes;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUITests.NUnit.Models;
using WebUITests.NUnit.Utilities;

namespace WebUITests.NUnit.Tests
{
    [TestFixture]
    [AllureFeature("Reading Lists API")]
    public class ReadingListsTests : BaseApiTest
    {
        private ReadingList _testReadingList;
        private Guid _createdListId;
        private string _ownerId = "test-owner-id";

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _testReadingList = new ReadingList
            {
                Name = "Test Reading List",
                OwnerId = Guid.NewGuid()
            };
        }

        [Test]
        [Order(1)]
        [AllureStory("Create Reading List")]
        public async Task CreateReadingList_ValidData_ReturnsCreated()
        {
            var response = await ApiClient.PostAsync("/ReadingLists", _testReadingList);
            var responseList = await response.Content.ReadFromJsonAsync<ReadingList>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(responseList.Name, Is.EqualTo(_testReadingList.Name));

            _createdListId = responseList.Id;
        }

        [Test]
        [Order(2)]
        [AllureStory("Get Reading List")]
        public async Task GetReadingListById_ValidId_ReturnsReadingList()
        {
            Assume.That(_createdListId, Is.Not.EqualTo(Guid.Empty));

            var response = await ApiClient.GetAsync($"/ReadingLists/{_createdListId}");
            var responseList = await response.Content.ReadFromJsonAsync<ReadingList>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseList.Id, Is.EqualTo(_createdListId));
        }

        [Test]
        [Order(3)]
        [AllureStory("Update Reading List")]
        public async Task UpdateReadingList_ValidData_ReturnsNoContent()
        {
            Assume.That(_createdListId, Is.Not.EqualTo(Guid.Empty));
            _testReadingList.Name = "Updated Reading List Name";

            var response = await ApiClient.PutAsync($"/ReadingLists/{_createdListId}", _testReadingList);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        [Order(4)]
        [AllureStory("Delete Reading List")]
        public async Task DeleteReadingList_ValidId_ReturnsSuccess()
        {
            Assume.That(_createdListId, Is.Not.EqualTo(Guid.Empty));

            var response = await ApiClient.DeleteAsync($"/ReadingLists/{_createdListId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}