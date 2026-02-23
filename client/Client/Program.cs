using System.Net.Http.Json;

var client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:7076/"); //dejo mi api escuchando en https

var users = new[]
{
    new { Name = "Ana",  Email = "ana@test.com"  },
    new { Name = "Luis", Email = "luis@test.com" },
    new { Name = "Marta", Email = "marta@test.com" },
    new { Name = "Carlos", Email = "carlos@test.com" },
    new { Name = "Laura", Email = "laura@test.com" },
    new { Name = "David", Email = "david@test.com" },
    new { Name = "Elena", Email = "elena@test.com" },
    new { Name = "Javier", Email = "javier@test.com" },
    new { Name = "Paula", Email = "paula@test.com" },
    new { Name = "Sergio", Email = "sergio@test.com" },
    new { Name = "Claudia", Email = "claudia@test.com" },
    new { Name = "Raul", Email = "raul@test.com" },
    new { Name = "Lucia", Email = "lucia@test.com" },
    new { Name = "Mario", Email = "mario@test.com" },
    new { Name = "Sara", Email = "sara@test.com" }
};

foreach (var user in users)
{
    // POST
    var postResponse = await client.PostAsJsonAsync("api/users", user);

    if (!postResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"POST error: {postResponse.StatusCode}");
        var body = await postResponse.Content.ReadAsStringAsync();
        if (!string.IsNullOrWhiteSpace(body))
            Console.WriteLine(body);
        continue;
    }

    var createdUser = await postResponse.Content.ReadFromJsonAsync<UserDto>();

    if (createdUser == null)
    {
        Console.WriteLine("POST OK pero no pude leer el JSON de respuesta.");
        continue;
    }

    Console.WriteLine($"POST OK -> Id {createdUser.Id}");

    // PUT 
    var updatedUser = new
    {
        Id = createdUser.Id,
        Name = createdUser.Name + " Updated",
        Email = createdUser.Email
    };

    var putResponse = await client.PutAsJsonAsync($"api/users/{createdUser.Id}", updatedUser);

    if (putResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"PUT OK -> Id {createdUser.Id}");
    }
    else
    {
        Console.WriteLine($"PUT error: {putResponse.StatusCode}");
        var putBody = await putResponse.Content.ReadAsStringAsync();
        if (!string.IsNullOrWhiteSpace(putBody))
            Console.WriteLine(putBody);
    }
}

// DTO de respuesta 
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}