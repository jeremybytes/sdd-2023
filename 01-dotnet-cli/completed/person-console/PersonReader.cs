using Newtonsoft.Json;

namespace person_console;

public class PersonReader
{
    HttpClient client =
        new() { BaseAddress = new Uri("http://localhost:9874") };
    public async Task<List<Person>> GetPeopleAsync()
    {
        HttpResponseMessage response =
            await client.GetAsync("people");

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Unable to retrieve People. Status code {response.StatusCode}");

        var stringResult =
            await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<Person>>(stringResult);
        if (result is null)
            throw new JsonException("Unable to deserialize List<Person> object (json object may be empty)");
        return result;
    }
}
