using Microsoft.AspNetCore.Mvc;

namespace person_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    IPeopleProvider provider;

    public PeopleController(IPeopleProvider provider)
    {
        this.provider = provider;
    }

    [HttpGet(Name = "GetPeople")]
    public IEnumerable<Person> Get()
    {
        return provider.GetPeople();
    }

    [HttpGet("{id}", Name = "GetPerson")]
    public Person? Get(int id)
    {
        return provider.GetPerson(id);
    }
}
