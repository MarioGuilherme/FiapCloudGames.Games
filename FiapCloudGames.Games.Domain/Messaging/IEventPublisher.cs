namespace FiapCloudGames.Games.Domain.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, string routingKey, string? correlationId = default);
}