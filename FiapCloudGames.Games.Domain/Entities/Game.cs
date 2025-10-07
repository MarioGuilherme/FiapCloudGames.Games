namespace FiapCloudGames.Games.Domain.Entities;

public class Game(string title, string? description, decimal price)
{
    public int GameId { get; private set; }
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public decimal Price { get; private set; } = price;
    public virtual ICollection<GameUser> Users { get; private set; } = [];
    public virtual ICollection<GameGenre> Genres { get; private set; } = [];
    public virtual ICollection<Order> Orders { get; private set; } = [];
    public virtual ICollection<GameEvent> GameEvents { get; private set; } = [];

    public void Update(string title, string? description, decimal price)
    {
        Title = title;
        Description = description;
        Price = price;
        GameEvents.Add(GameEvent.FromGame(this));
    }
}
