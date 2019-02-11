using System;
using System.Linq;
using Moq;
using WingsOn.API.BusinessLogic.Services;
using WingsOn.Dal;
using WingsOn.Domain;
using Xunit;

namespace WingsOn.API.Tests.Services
{
    public class BookingServiceTests
    {
        [Fact]
        public void TestGetPassengersByFlightNumber_ShouldReturnFilteredListOfPassengers()
        {
            // Arrange
            var bookingRepository = new Mock<IRepository<Booking>>();
            var passengersList = new[]
            {
                new Booking
                {
                    Passengers = new[] {new Person(), new Person(),},
                    Flight = new Flight {Number = "testFlight"},
                },
                new Booking
                {
                    Passengers = new[] {new Person(), new Person(),},
                    Flight = new Flight {Number = "testFlight1"},
                }
            };
            bookingRepository.Setup(x => x.GetAll()).Returns(passengersList);
            var bookingService = new BookingService(null, bookingRepository.Object, null);


            // Act
            var result = bookingService.GetPassengersByFlightNumber("testFlight");

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void TestCreateBookingForFlight_WhenCustomerDoesNotExist_ShouldThrowAnException()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            personRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(() => null);
            var bookingService = new BookingService(personRepository.Object, null, null);

            // Act
            var exception = Assert.Throws<NotSupportedException>(() => bookingService.CreateBookingForFlight(2, 2, "testFlight"));

            // Assert
            Assert.Equal("A customer with id=2 was not found", exception.Message);
        }

        [Fact]
        public void TestCreateBookingForFlight_WhenTargetPersonDoesNotExist_ShouldThrowAnException()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            personRepository.Setup(x => x.Get(1)).Returns(() => new Person { Id = 1, Name = "testName" });
            personRepository.Setup(x => x.Get(2)).Returns(() => null);
            var bookingService = new BookingService(personRepository.Object, null, null);

            // Act
            var exception = Assert.Throws<NotSupportedException>(() => bookingService.CreateBookingForFlight(1, 2, "testFlight"));

            // Assert
            Assert.Equal("A person with id=2 was not found", exception.Message);
        }

        [Fact]
        public void TestCreateBookingForFlight_WhenPersonHasAlreadyRegistered_ShouldThrowAnException()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            personRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Person { Id = 1, Name = "testName" });
            var bookingRepository = new Mock<IRepository<Booking>>();
            var passengersList = new[]
            {
                new Booking
                {
                    Passengers = new[] { new Person { Id = 1 }, new Person(), },
                    Flight = new Flight { Number = "testFlight" },
                }
            };
            bookingRepository.Setup(x => x.GetAll()).Returns(passengersList);
            var bookingService = new BookingService(personRepository.Object, bookingRepository.Object, null);

            // Act
            var exception = Assert.Throws<NotSupportedException>(() => bookingService.CreateBookingForFlight(1, 1, "testFlight"));

            // Assert
            Assert.Equal("A passenger has already registered for this flight", exception.Message);
        }

        [Fact]
        public void TestCreateBookingForFlight_WhenFlightDoesNotExist_ShouldThrowAnException()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            personRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Person { Id = 1, Name = "testName" });
            var bookingRepository = new Mock<IRepository<Booking>>();
            var passengersList = new[]
            {
                new Booking
                {
                    Passengers = new[] { new Person(), new Person(), },
                    Flight = new Flight { Number = "testFlight" },
                }
            };
            bookingRepository.Setup(x => x.GetAll()).Returns(passengersList);
            var flightRepository = new Mock<IRepository<Flight>>();
            flightRepository.Setup(x => x.GetAll()).Returns(new Flight[] {});
            var bookingService = new BookingService(personRepository.Object, bookingRepository.Object, flightRepository.Object);

            // Act
            var exception = Assert.Throws<NotSupportedException>(() => bookingService.CreateBookingForFlight(1, 1, "testFlight"));

            // Assert
            Assert.Equal("A flight with number=testFlight does not exist", exception.Message);
        }

        [Fact]
        public void TestCreateBookingForFlight_WhenInputValuesAreValid_ShouldCreateBookingCorrectly()
        {
            // Arrange
            var personRepository = new Mock<IRepository<Person>>();
            personRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Person { Id = 1, Name = "testName" });
            var bookingRepository = new Mock<IRepository<Booking>>();
            var passengersList = new[]
            {
                new Booking
                {
                    Id = 23,
                    Passengers = new[] {new Person(), new Person(),},
                    Flight = new Flight { Id = 1, Number = "testFlight" },
                }
            };
            bookingRepository.Setup(x => x.GetAll()).Returns(passengersList);
            var flightRepository = new Mock<IRepository<Flight>>();
            var flightInfo = new Flight {Id = 1, Number = "testFlight"};
            flightRepository.Setup(x => x.GetAll()).Returns(new[] { flightInfo });
            var bookingService =
                new BookingService(personRepository.Object, bookingRepository.Object, flightRepository.Object);

            // Act
            var newBookinInfo = bookingService.CreateBookingForFlight(1, 1, "testFlight");

            // Assert
            Assert.Equal(24, newBookinInfo.Id);
            Assert.Equal(flightInfo, newBookinInfo.Flight);
            Assert.Equal(1, newBookinInfo.Customer.Id);
            Assert.Single(newBookinInfo.Passengers);
            Assert.Contains(1, newBookinInfo.Passengers.Select(x=>x.Id));
        }
    }
}
