using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<Game> games =
[
    new()
    {
        Id = Guid.NewGuid(),
        Name = "The Legend of Zelda: Breath of the Wild",
        Genre = "Action-adventure",
        Price = 59.99m,
        ReleaseDate = new DateOnly(2017, 3, 3)
    },
    new()
    {
        Id = Guid.NewGuid(),
        Name = "God of War",
        Genre = "Action-adventure",
        Price = 49.99m,
        ReleaseDate = new DateOnly(2018, 4, 20)
    },
    new()
    {
        Id = Guid.NewGuid(),
        Name = "Red Dead Redemption 2",
        Genre = "Action-adventure",
        Price = 39.99m,
        ReleaseDate = new DateOnly(2018, 10, 26)
    }
];

app.MapGet("/games", () => games);

app.MapGet("/games/{id}", (Guid id) =>
{
    var game = games.FirstOrDefault(g => g.Id == id);
    return game is not null ? Results.Ok(game) : Results.NotFound();
}).WithName(GetGameEndpointName);

app.MapPost("/games", (Game game) =>
{
    game.Id = Guid.NewGuid();
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new
    {
        id = game.Id,
    }, game);
}).WithParameterValidation();

app.MapPut("/games/{id}", (Guid id, Game game) =>
{
    var existingGame = games.FirstOrDefault(g => g.Id == id);
    if (existingGame is null)
    {
        return Results.NotFound();
    }

    existingGame.Name = game.Name;
    existingGame.Genre = game.Genre;
    existingGame.Price = game.Price;
    existingGame.ReleaseDate = game.ReleaseDate;

    return Results.NoContent();
}).WithParameterValidation();


app.MapDelete("/games/{id}", (Guid id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});

app.Run();