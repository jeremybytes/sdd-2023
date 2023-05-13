using person_api;
using person_api.Controllers;

namespace person_api_tests;

public class PeopleControllerTests
{
    PeopleController controller;

    [SetUp]
    public void Setup()
    {
        var provider = new FakePeopleProvider();
        controller = new PeopleController(provider);
    }

    [Test]
    public void GetPeople_ReturnsAllItems()
    {
        IEnumerable<Person> actual = controller.Get();
        Assert.That(actual.Count, Is.EqualTo(9));
    }

    [Test]
    public void GetPerson_WithValidId_ReturnsPerson()
    {
        Person? actual = controller.Get(2);
        Assert.That(actual?.Id, Is.EqualTo(2));
    }

    [Test]
    public void GetPerson_WithInvalidId_ReturnsNull()
    {
        Person? actual = controller.Get(-10);
        Assert.IsNull(actual);
    }
}