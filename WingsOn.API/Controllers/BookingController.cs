﻿using System.Collections.Generic;
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
        /// <param name="flightNumber">Number of flight. Example - PZ696</param>
        /// <response code="200">Successful response</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Something went wrong on server</response>
        [ProducesResponseType(typeof(List<Person>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
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
        /// <param name="customerId">An owner of booking</param>
        /// <param name="targetPersonId">A person who will be registered</param>
        /// <param name="flightNumber">Number of flight. Example - PZ696</param>
        /// <response code="200">Successful response</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Something went wrong on server</response>
        [ProducesResponseType(typeof(List<Person>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        [Route("book/{customerId:int}/{targetPersonId:int}/{flightNumber}")]
        [HttpPost]
        public ActionResult CreateBookingForFlight(int customerId, int targetPersonId, string flightNumber)
        {
            var result = _bookingService.CreateBookingForFlight(customerId, targetPersonId, flightNumber);
            return Ok(result);
        }
    }
}
