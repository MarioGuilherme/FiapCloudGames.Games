namespace FiapCloudGames.Games.Domain.Entities;

public class GameGenre
{
    public int GameGenreId { get; private set; }
    public string Title { get; private set; } = null!;
    public virtual ICollection<Game> Games { get; } = [];

    public GameGenre(int gameGenreId, string title)
    {
        GameGenreId = gameGenreId;
        Title = title;
    }

    public GameGenre(int gameGenreId) => GameGenreId = gameGenreId;
}
