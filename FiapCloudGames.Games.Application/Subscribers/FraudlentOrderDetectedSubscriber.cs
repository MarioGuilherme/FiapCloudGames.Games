using FiapCloudGames.Games.Application.Interfaces;
using FiapCloudGames.Games.Domain.Events;
using FiapCloudGames.Games.Domain.Messaging;
using FiapCloudGames.Games.Infrastructure.Messaging.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Context;
using System.Text;
using System.Text.Json;

namespace FiapCloudGames.Games.Application.Subscribers;

public class FraudlentOrderDetectedSubscriber(IServiceProvider serviceProvider,
    IOptions<RabbitMqOptions> options,
    IRabbitMqConnection connection,
    IEventPublisher eventPublisher) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly RabbitMqOptions _options = options.Value;
    private readonly IRabbitMqConnection _connection = connection;
    private readonly IEventPublisher _eventPublisher = eventPublisher;
    private const string QUEUE = "fraudlent-order-detected";
    private const string ROUTING_KEY = "fraudlent.order.detected";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IChannel channel = await (await _connection.GetConnectionAsync()).CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(_options.Exchange, ExchangeType.Topic, true, cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync(QUEUE, true, false, false, cancellationToken: stoppingToken);
        await channel.QueueBindAsync(QUEUE, _options.Exchange, ROUTING_KEY, cancellationToken: stoppingToken);
        AsyncEventingBasicConsumer consumer = new(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            string message = Encoding.UTF8.GetString(ea.Body.ToArray());

            FraudlentOrderDetectedEvent fraudlentOrderDetectedEvent = JsonSerializer.Deserialize<FraudlentOrderDetectedEvent>(message)!;

            using (LogContext.PushProperty("CorrelationId", ea.BasicProperties.CorrelationId))
            {
                await ProcessFraudlentOrderDetectedAsync(fraudlentOrderDetectedEvent);
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(QUEUE, false, consumer, cancellationToken: stoppingToken);
    }

    private async Task ProcessFraudlentOrderDetectedAsync(FraudlentOrderDetectedEvent fraudlentOrderDetectedEvent)
    {
        Log.Information("Subscriber {SubscriberName} iniciado às {DateTime}", nameof(FraudlentOrderDetectedSubscriber), DateTime.Now);

        using IServiceScope scope = _serviceProvider.CreateScope();
        IOrderService orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        await orderService.CancelByIdAsync(fraudlentOrderDetectedEvent.OrderId);

        Log.Information("Subscriber {SubscriberName} finalizado às {DateTime}", nameof(FraudlentOrderDetectedSubscriber), DateTime.Now);
    }
}
