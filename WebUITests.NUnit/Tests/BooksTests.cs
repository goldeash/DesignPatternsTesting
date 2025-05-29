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
    [AllureFeature("Books API")]
    public class BooksTests : BaseApiTest
    {
        private Book _testBook;
        private Guid _createdBookId;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _testBook = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                Isbn = $"978-{new Random().Next(100000000, 999999999)}",
                PublishedDate = DateTime.UtcNow
            };
        }

        [Test]
        [Order(1)]
        [AllureStory("Create Book")]
        public async Task CreateBook_ValidData_ReturnsCreated()
        {
            var response = await ApiClient.PostAsync("/Books", _testBook);
            var responseBook = await response.Content.ReadFromJsonAsync<Book>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(responseBook.Title, Is.EqualTo(_testBook.Title));

            _createdBookId = responseBook.Id;
        }

        [Test]
        [Order(2)]
        [AllureStory("Get Book")]
        public async Task GetBookById_ValidId_ReturnsBook()
        {
            Assume.That(_createdBookId, Is.Not.EqualTo(Guid.Empty));

            var response = await ApiClient.GetAsync($"/Books/{_createdBookId}");
            var responseBook = await response.Content.ReadFromJsonAsync<Book>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseBook.Id, Is.EqualTo(_createdBookId));
        }
    }
}