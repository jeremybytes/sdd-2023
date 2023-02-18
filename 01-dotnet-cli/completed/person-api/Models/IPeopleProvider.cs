namespace person_api;

public interface IPeopleProvider
{
    List<Person> GetPeople();
    Person? GetPerson(int id);
}