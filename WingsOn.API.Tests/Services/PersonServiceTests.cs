using Moq;
using System;
using WingsOn.API.BusinessLogic.Services;
using WingsOn.Dal;
using WingsOn.Domain;
using Xunit;

namespace WingsOn.API.Tests
{
    public class PersonServiceTests
    {
        [Fact]
        public void TestGetPersonById_WhenPersonExists_ShouldReturnPersonInfo()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            personRepository.Setup(x => x.Get(1)).Returns(new Person { Id = 1, Name = "testName" });
            var personService = new PersonService(personRepository.Object);
            

            // Act
            var result = personService.GetPersonById(1);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("testName", result.Name);
        }

        [Fact]
        public void TestGetPersonById_WhenPersonDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            personRepository.Setup(x => x.Get(1)).Returns(() => null);
            var personService = new PersonService(personRepository.Object);

            // Act
            var exception = Assert.Throws<NotSupportedException>(() => personService.GetPersonById(1));

            // Assert
            Assert.Equal("A person with id=1 does not exist", exception.Message);
        }

        [Fact]
        public void TestGetPersonsByGender_WhenFilterByMale_ShouldReturnCorrectlyFilteredValues()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            var persons = new[] {
                new Person { Id = 1, Name = "testName1", Gender = GenderType.Male },
                new Person { Id = 2, Name = "testName2", Gender = GenderType.Male },
                new Person { Id = 3, Name = "testName3", Gender = GenderType.Female }
            };
            personRepository.Setup(x => x.GetAll()).Returns(persons);
            var personService = new PersonService(personRepository.Object);


            // Act
            var result = personService.GetPersonsByGender(GenderType.Male);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void TestGetPersonsByGender_WhenFilterByFemale_ShouldReturnCorrectlyFilteredValues()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            var persons = new[] {
                new Person { Id = 1, Name = "testName1", Gender = GenderType.Male },
                new Person { Id = 2, Name = "testName2", Gender = GenderType.Male },
                new Person { Id = 3, Name = "testName3", Gender = GenderType.Female }
            };
            personRepository.Setup(x => x.GetAll()).Returns(persons);
            var personService = new PersonService(personRepository.Object);


            // Act
            var result = personService.GetPersonsByGender(GenderType.Female);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void TestUpdatePersonAddress_WhenPersonExist_ShouldReturnUpdatedPersonInfo()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            personRepository.Setup(x => x.Get(1)).Returns(new Person { Id = 1, Name = "testName", Address = "defaultAddress" });
            var personService = new PersonService(personRepository.Object);

            // Act
            var result = personService.UpdatePersonAddress(1, "newTestAddress");

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("newTestAddress", result.Address);
        }

        [Fact]
        public void TestUpdatePersonAddress_WhenPersonDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            personRepository.Setup(x => x.Get(1)).Returns(() => null);
            var personService = new PersonService(personRepository.Object);

            // Act
            var exception = Assert.Throws<NotSupportedException>(() => personService.UpdatePersonAddress(1, "newTestAddress"));

            // Assert
            Assert.Equal("A person with id=1 does not exist", exception.Message);
        }

 
    }
}
