using System;
using System.Collections.Generic;
using System.Linq;
using WingsOn.API.BusinessLogic.Contracts;
using WingsOn.Dal;
using WingsOn.Domain;

namespace WingsOn.API.BusinessLogic.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<Person> _personRepository;

        public PersonService(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        public Person GetPersonById(int personId)
        {
            var person = _personRepository.Get(personId);
            if (person == null)
            {
                throw new NotSupportedException($"A person with id={personId} does not exist");
            }
            return person;
        }

        public List<Person> GetPersonsByGender(GenderType targetType)
        {
            return _personRepository.GetAll().Where(x => x.Gender == targetType).ToList();
        }

        public Person UpdatePersonAddress(int personId, string newAddress)
        {
            var person = _personRepository.Get(personId);
            if (person == null)
            {
                throw new NotSupportedException($"A person with id={personId} does not exist");
            }
            person.Address = newAddress;
            _personRepository.Save(person);
            return person;
        }
    }
}
