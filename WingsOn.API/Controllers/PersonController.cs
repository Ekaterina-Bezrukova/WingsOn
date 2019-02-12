using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WingsOn.API.BusinessLogic.Contracts;
using WingsOn.Domain;

namespace WingsOn.API.Controllers
{
    [Produces("application/json")]
    [Route("person")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Gets person information by id.
        /// </summary>
        /// <param name="personId">Person identifier</param>
        /// <response code="200">Successful response</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Something went wrong on server</response>
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        [Route("{personId:int}")]
        [HttpGet]
        public ActionResult GetPersonById(int personId)
        {
            var result = _personService.GetPersonById(personId);

            return Ok(result);
        }

        /// <summary>
        /// Gets persons filtered by gender.
        /// </summary>
        /// <param name="targetType">Target gender type. 1 - Male, 2 - Female </param>
        /// <response code="200">Successful response</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Something went wrong on server</response>
        [ProducesResponseType(typeof(List<Person>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        [Route("gender/{targetType:int}")]
        [HttpGet]
        public ActionResult GetPersonsByGender(GenderType targetType)
        {
            var result = _personService.GetPersonsByGender(targetType);

            return Ok(result);
        }

        /// <summary>
        /// Updates person's address
        /// </summary>
        /// <param name="personId">Person Id</param>
        /// <param name="newAddress">A new person's address</param>
        /// <response code="200">Successful response</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Something went wrong on server</response>
        [ProducesResponseType(typeof(List<Person>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        [Route("updateAddress/{personId:int}")]
        [HttpPut]
        public ActionResult UpdatePersonAddress(int personId, string newAddress)
        {
            var result = _personService.UpdatePersonAddress(personId, newAddress);

            return Ok(result);
        }
    }
}
