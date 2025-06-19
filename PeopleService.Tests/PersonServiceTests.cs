using Moq;
using PeopleService.Application.DTOs;
using PeopleService.Application.Services;
using PeopleService.Domain.Entities;
using PeopleService.Infrastructure.Repositories;

namespace PeopleService.Tests
{
    public class PersonServiceTests
    {
        private readonly Mock<IPersonRepository> _mockRepo;
        private readonly PersonService _service;

        public PersonServiceTests()
        {
            _mockRepo = new Mock<IPersonRepository>();
            _service = new PersonService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllMappedPeople()
        {
            // Arrange
            var people = new List<Person>
    {
        new Person { Id = 1, FullName = "User1", Email = "user1@example.com", BirthDate = new DateTime(1978, 6, 30), City = "Vancouver" },
        new Person { Id = 2, FullName = "User2", Email = "user2@example.com", BirthDate = new DateTime(2003, 6, 24), City = "Montreal" }
    };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Email == "user1@example.com");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectPerson_WhenExists()
        {
            // Arrange
            var person = new Person { Id = 10, FullName = "User10", Email = "user10@example.com", BirthDate = new DateTime(1978, 6, 30), City = "Toronto" };
            _mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(person);

            // Act
            var result = await _service.GetByIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User10", result!.FullName);
            Assert.Equal("Toronto", result.City);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenPersonNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(9999)).ReturnsAsync((Person?)null);

            var result = await _service.GetByIdAsync(9999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedPerson()
        {
            var input = new PersonDto
            {
                FullName = "User1003",
                Email = "user1003@example.com",
                BirthDate = new DateTime(1999, 6, 25),
                City = "Vancouver"
            };

            var saved = new Person
            {
                Id = 1003,
                FullName = "User1003",
                Email = "user1003@example.com",
                BirthDate = new DateTime(1999, 6, 25),
                City = "Vancouver"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Person>())).ReturnsAsync(saved);

            var result = await _service.CreateAsync(input);

            Assert.Equal(1003, result.Id);
            Assert.Equal("Vancouver", result.City);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenUpdateSucceeds()
        {
            var updatedDto = new PersonDto { FullName = "Updated", Email = "up@example.com", BirthDate = DateTime.Today, City = "Montreal" };

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Person>())).ReturnsAsync(true);

            var result = await _service.UpdateAsync(1, updatedDto);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenPersonExists()
        {
            var updatedDto = new PersonDto
            {
                FullName = "UpdatedUser",
                Email = "updated@example.com",
                BirthDate = new DateTime(1985, 6, 28),
                City = "Ottawa"
            };

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Person>())).ReturnsAsync(true);

            var result = await _service.UpdateAsync(1, updatedDto);

            Assert.True(result);
        }
        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenRepoFails()
        {
            var updatedDto = new PersonDto
            {
                FullName = "GhostUser",
                Email = "ghost@example.com",
                BirthDate = new DateTime(2000, 1, 1),
                City = "Nowhere"
            };

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Person>())).ReturnsAsync(false);

            var result = await _service.UpdateAsync(9959, updatedDto);

            Assert.False(result);
        }
        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenPersonDeleted()
        {
            _mockRepo.Setup(r => r.DeleteAsync(2)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(2);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenPersonNotFound()
        {
            _mockRepo.Setup(r => r.DeleteAsync(9960)).ReturnsAsync(false);

            var result = await _service.DeleteAsync(9960);

            Assert.False(result);
        }
    }
}