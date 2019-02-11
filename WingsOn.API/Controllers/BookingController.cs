using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WingsOn.API.BusinessLogic.Contracts;
using WingsOn.Domain;

namespace WingsOn.API.Controllers
{
    [Produces("application/json")]
    [Route("booking")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Gets persons registered for flight.
        /// </summary>
        /// <response code="200">Successful response</response>
        /// <response code="404">No people registered for flight</response>
        /// <response code="400">One of the required parameters is not transmitted</response>
        [ProducesResponseType(typeof(List<Person>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 400)]
        [Route("getPassengersForFlight/{flightNumber}")]
        [HttpGet]
        public ActionResult GetPassengersByFlightNumber(string flightNumber)
        {
            var result = _bookingService.GetPassengersByFlightNumber(flightNumber);
            return Ok(result);
        }

        /// <summary>
        /// Create a booking for person.
        /// </summary>
        /// <response code="200">Successful response</response>
        /// <response code="400">One of the required parameters is not transmitted</response>
        [ProducesResponseType(typeof(List<Person>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [Route("book/{customerId:int}/{targetPersonId:int}/{flightNumber}")]
        [HttpPost]
        public ActionResult CreateBookingForFlight(int customerId, int targetPersonId, string flightNumber)
        {
            var result = _bookingService.CreateBookingForFlight(customerId, targetPersonId, flightNumber);
            return Ok(result);
        }
    }
}
