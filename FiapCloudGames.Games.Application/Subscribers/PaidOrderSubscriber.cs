using Elastic.Clients.Elasticsearch.Security;
using FiapCloudGames.Games.Application.Interfaces;
using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Events;
using FiapCloudGames.Games.Domain.Messaging;
using FiapCloudGames.Games.Domain.Repositories;
using FiapCloudGames.Games.Infrastructure.Messaging.RabbitMq;
using FiapCloudGames.Games.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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

public class PaidOrderSubscriber(IServiceProvider serviceProvider,
    IOptions<RabbitMqOptions> options,
    IRabbitMqConnection connection,
    IEventPublisher eventPublisher) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly RabbitMqOptions _options = options.Value;
    private readonly IRabbitMqConnection _connection = connection;
    private readonly IEventPublisher _eventPublisher = eventPublisher;
    private const string QUEUE = "paid-order";
    private const string ROUTING_KEY = "paid.order";

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

            PaidOrderEvent paidOrderEvent = JsonSerializer.Deserialize<PaidOrderEvent>(message)!;

            using (LogContext.PushProperty("CorrelationId", ea.BasicProperties.CorrelationId))
            {
                await ProcessPaidOrderAsync(paidOrderEvent, ea.BasicProperties.CorrelationId!);
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(QUEUE, false, consumer, cancellationToken: stoppingToken);
    }

    private async Task ProcessPaidOrderAsync(PaidOrderEvent paidOrderEvent, string correlationId)
    {
        Log.Information("Subscriber {SubscriberName} iniciado às {DateTime}", nameof(PaidOrderSubscriber), DateTime.Now);

        using IServiceScope scope = _serviceProvider.CreateScope();
        IOrderRepository orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
        IOrderService orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        await orderService.UnlockGamesFromOrderToUserAsync(paidOrderEvent.OrderId);
        Order order = (await orderRepository.GetByIdTrackingAsync(paidOrderEvent.OrderId))!;
        await _eventPublisher.PublishAsync(new SendPendingEmailEvent(order.UserId, "Compra Paga", "Recebemos seu pagamento. Seus jogos estão disponível em sua biblioteca"), "send.pending.email", correlationId);

        Log.Information("Subscriber {SubscriberName} finalizado às {DateTime}", nameof(PaidOrderSubscriber), DateTime.Now);
    }
}
