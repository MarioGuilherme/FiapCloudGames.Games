namespace FiapCloudGames.Games.Domain.Events;

public record OrderCreatedEvent(int OrderId, int UserId, decimal Total);
