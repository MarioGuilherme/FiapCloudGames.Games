namespace FiapCloudGames.Games.Domain.Entities;

public class GameUser(int userId, int gameId)
{
    public int UserId { get; private set; } = userId;
    public int GameId { get; private set; } = gameId;
    public Game Game { get; private set; } = null!;
}
