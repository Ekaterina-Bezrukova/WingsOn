using System.Collections.Generic;
using WingsOn.Domain;

namespace WingsOn.API.BusinessLogic.Contracts
{
    public interface IBookingService
    {
        List<Person> GetPassengersByFlightNumber(string flightNumber);

        Booking CreateBookingForFlight(int customerId, int personId, string flightNumber);
    }
}
