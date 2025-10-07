namespace FiapCloudGames.Games.Domain.Entities;

public class GameEvent
{
    public int GameEventId { get; private set; }
    public int GameId { get; private set; }
    public Game Game { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTime EventAt { get; } = DateTime.Now;

    private GameEvent() { }

    public static GameEvent FromGame(Game game) => new()
    {
        GameId = game.GameId,
        Title = game.Title,
        Description = game.Description,
        Price = game.Price
    };
}
