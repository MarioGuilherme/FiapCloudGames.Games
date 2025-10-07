namespace FiapCloudGames.Games.Application.InputModels;

public record UpdateGameInputModel(string Title, string Description, decimal Price, HashSet<int> GameGenreIds);
