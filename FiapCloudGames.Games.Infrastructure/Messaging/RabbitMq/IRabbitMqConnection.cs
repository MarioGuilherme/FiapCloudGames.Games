using RabbitMQ.Client;

namespace FiapCloudGames.Games.Infrastructure.Messaging.RabbitMq;

public interface IRabbitMqConnection
{
    Task<IConnection> GetConnectionAsync();
}