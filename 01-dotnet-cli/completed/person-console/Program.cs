namespace person_console;
class Program
{
    static async Task Main(string[] args)
    {
        var reader = new PersonReader();
        var people = await reader.GetPeopleAsync();
        foreach(var person in people)
            Console.WriteLine(person);

        Console.WriteLine("===============");
    }
}
