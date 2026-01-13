namespace FiapCloudGames.Games.Domain.Events;

public record PaidOrderEvent(int OrderId, int UserId);
