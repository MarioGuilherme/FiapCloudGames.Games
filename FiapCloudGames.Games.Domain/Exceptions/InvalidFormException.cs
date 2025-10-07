namespace FiapCloudGames.Games.Domain.Exceptions;

public class InvalidFormException(string message, ICollection<string> errors) : Exception(message)
{
    public ICollection<string> Errors { get; set; } = errors;
}
