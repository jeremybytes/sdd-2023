var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(9874));

// Add services to the container.
builder.Services.AddSingleton<IPeopleProvider, HardCodedPeopleProvider>();
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.WriteIndented = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/people", async (IPeopleProvider provider) => await provider.GetPeople())
    .WithName("GetPeople");

app.MapGet("/people/{id}",
    async (int id, IPeopleProvider provider) =>
        {
            var person = await provider.GetPerson(id);
            return person switch
            {
                null => Results.NoContent(),
                _ => Results.Json(person)
            };
        })
    .WithName("GetPerson");

app.Run();
