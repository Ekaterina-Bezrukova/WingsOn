using System.Collections.Generic;
using WingsOn.Domain;

namespace WingsOn.API.BusinessLogic.Contracts
{
    public interface IPersonService
    {
        Person GetPersonById(int personId);

        List<Person> GetPersonsByGender(GenderType targetType);

        Person UpdatePersonAddress(int personId, string newAddress);
    }
}
