namespace person_api;

public class CSVPeopleProvider : IPeopleProvider
{
    string filePath = AppDomain.CurrentDomain.BaseDirectory + "People.txt";

    public List<Person> GetPeople()
    {
        var fileData = LoadFile();
        var people = ParseData(fileData);
        return people;
    }

    public Person? GetPerson(int id)
    {
        var people = GetPeople();
        return people.SingleOrDefault(p => p.Id == id);
    }

    private List<Person> ParseData(IReadOnlyCollection<string> csvData)
    {
        var people = new List<Person>();

        foreach (string line in csvData)
        {
            try
            {
                var elems = line.Split(',');
                var per = new Person(
                
                    Int32.Parse(elems[0]),
                    elems[1],
                    elems[2],
                    DateTime.Parse(elems[3]),
                    Int32.Parse(elems[4]),
                    elems[5]
                );
                people.Add(per);
            }
            catch (Exception)
            {
                // Skip the bad record, log it, and move to the next record
                // log.write("Unable to parse record", per);
            }
        }
        return people;
    }

    private IReadOnlyCollection<string> LoadFile()
    {
        var data = new List<string>();

        using (var reader = new StreamReader(filePath))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                data.Add(line);
            }
        }

        return data;
    }
}
