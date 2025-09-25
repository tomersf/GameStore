using GameStore.Api.Models;

namespace GameStore.Api.Data;

public class GameStoreData
{
    private readonly List<Genre> _genres = new()
    {
        new()
        {
            Id = new Guid("A0744781-E945-4C5F-B0D7-D99AF6E2F5B0"),
            Name = "Action"
        },
        new()
        {
            Id = new Guid("0ABE259B-20C0-4CB7-A052-98A81FE33B94"),
            Name = "Adventure"
        },
        new()
        {
            Id = new Guid("206B43EE-E0CE-427D-AED0-4CCE1DCFF0BE"), Name = "RPG"
        },
        new()
        {
            Id = new Guid("64553355-784F-4CB1-AA55-BA41CD732B6B"),
            Name = "Strategy"
        }
    };

    private readonly List<Game> _games;

    public GameStoreData()
    {
        _games =
        [
            new()
            {
                Id = Guid.NewGuid(),
                Name = "The Legend of Zelda: Breath of the Wild",
                Genre = _genres[0],
                Price = 59.99m,
                ReleaseDate = new DateOnly(2017, 3, 3),
                Description =
                    "An open-world adventure game set in the kingdom of Hyrule."
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "God of War",
                Genre = _genres[1],
                Price = 49.99m,
                ReleaseDate = new DateOnly(2018, 4, 20),
                Description =
                    "A mythological action-adventure game following Kratos and his son Atreus."
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Red Dead Redemption 2",
                Genre = _genres[2],
                Price = 39.99m,
                ReleaseDate = new DateOnly(2018, 10, 26),
                Description =
                    "An epic tale of life in America at the dawn of the modern age."
            }
        ];
    }

    public IEnumerable<Game> GetGames() => _games;
    public Game? GetGame(Guid id) => _games.FirstOrDefault(g => g.Id == id);

    public void AddGame(Game game)
    {
        game.Id = Guid.NewGuid();
        _games.Add(game);
    }

    public void RemoveGame(Guid id) => _games.RemoveAll(game => game.Id == id);

    public IEnumerable<Genre> GetGenres() => _genres;
    public Genre? GetGenre(Guid id) => _genres.FirstOrDefault(genre => genre.Id == id);
}