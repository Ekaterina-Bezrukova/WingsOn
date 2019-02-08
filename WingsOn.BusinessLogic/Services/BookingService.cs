using System;
using System.Collections.Generic;
using System.Linq;
using WingsOn.API.BusinessLogic.Contracts;
using WingsOn.Dal;
using WingsOn.Domain;

namespace WingsOn.API.BusinessLogic.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;

        private readonly IRepository<Person> _personRepository;

        private readonly IRepository<Flight> _flightRepository;

        public BookingService(IRepository<Person> personRepository, IRepository<Booking> bookingRepository, IRepository<Flight> flightRepository)
        {
            _personRepository = personRepository;
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
        }

        public List<Person> GetPassengersByFlightNumber(string flightNumber)
        {
            return _bookingRepository.GetAll().Where(x => x.Flight.Number == flightNumber)
                                           .SelectMany(x => x.Passengers).ToList();
        }

        public Booking CreateBookingForFlight(int customerId, int targetPersonId, string flightNumber)
        {
            var customer = _personRepository.Get(customerId);
            if (customer == null)
            {
                throw new NotSupportedException($"A customer with id={customerId} was not found");
            }

            var targetPerson = _personRepository.Get(targetPersonId);
            if (targetPerson == null)
            {
                throw new NotSupportedException($"A person with id={targetPersonId} was not found");
            }

            var allBookingInfo = _bookingRepository.GetAll();
            var booking = allBookingInfo.Where(x => x.Flight.Number == flightNumber).ToList();

            if (booking.Any(x => x.Passengers.Any(y => y.Id == targetPersonId)))
            {
                throw new NotSupportedException("A passenger has already registered for this flight");
            }

            var flightInfo = _flightRepository.GetAll().SingleOrDefault(x => x.Number == flightNumber);
            if (flightInfo == null)
            {
                throw new NotSupportedException($"A flight with number={flightNumber} does not exist");
            }

            var newBookingId = allBookingInfo.Max(x => x.Id) + 1;
            var newBooking = CreateBookingInfo(customer, targetPerson, flightInfo, newBookingId);

            _bookingRepository.Save(newBooking);
            return newBooking;
        }

        private Booking CreateBookingInfo(Person customer, Person targetPerson, Flight flightInfo, int bookingId)
        {
            return new Booking
            {
                Customer = customer,
                Passengers = new[] { targetPerson },
                DateBooking = DateTime.Now,
                Flight = flightInfo,
                Number = GenerateBookingNumber(),
                Id = bookingId,
            };
        }

        private string GenerateBookingNumber()
        {
            var number = new Random().Next(0, 1000000).ToString("D6");
            return $"WO-{number}";
        }
    }
}
