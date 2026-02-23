using System.Net.Http.Json;

var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5116/");

var users = new[]
{
    new { Name = "Ana", Email = "ana@test.com" },
    new { Name = "Luis", Email = "luis@test.com" }
};

foreach (var user in users)
{
    //POST
    var postResponse = await client.PostAsJsonAsync("api/users", user);

    if (!postResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"POST error: {postResponse.StatusCode}");
        continue;
    }

    var createdUser = await postResponse.Content.ReadFromJsonAsync<UserDto>();

    Console.WriteLine($"POST OK → Id {createdUser.Id}");

    // PUT usando el Id real creado
    var updatedUser = new
    {
        Id = createdUser.Id,
        Name = createdUser.Name + " Updated",
        Email = createdUser.Email
    };

    var putResponse = await client.PutAsJsonAsync(
        $"api/users/{createdUser.Id}",
        updatedUser);

    if (putResponse.IsSuccessStatusCode)
        Console.WriteLine($"PUT OK → Id {createdUser.Id}");
    else
        Console.WriteLine($"PUT error: {putResponse.StatusCode}");
}

//DTO para user
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}