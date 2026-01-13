using Elastic.Clients.Elasticsearch.Security;
using FiapCloudGames.Games.Application.Interfaces;
using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Events;
using FiapCloudGames.Games.Domain.Messaging;
using FiapCloudGames.Games.Infrastructure.Messaging.RabbitMq;
using FiapCloudGames.Games.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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

public class OrderPendingPaymentCreatedSubscriber(IServiceProvider serviceProvider,
    IOptions<RabbitMqOptions> options,
    IRabbitMqConnection connection,
    IEventPublisher eventPublisher) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly RabbitMqOptions _options = options.Value;
    private readonly IRabbitMqConnection _connection = connection;
    private readonly IEventPublisher _eventPublisher = eventPublisher;
    private const string QUEUE = "order-pending-payment-created";
    private const string ROUTING_KEY = "order.pending.payment.created";

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

            OrderPendingPaymentCreatedEvent orderPaymentPendingCreatedEvent = JsonSerializer.Deserialize<OrderPendingPaymentCreatedEvent>(message)!;

            using (LogContext.PushProperty("CorrelationId", ea.BasicProperties.CorrelationId))
            {
                await ProcessPendingPaymentCreatedAsync(orderPaymentPendingCreatedEvent);
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(QUEUE, false, consumer, cancellationToken: stoppingToken);
    }

    private async Task ProcessPendingPaymentCreatedAsync(OrderPendingPaymentCreatedEvent orderPendingPaymentCreatedEvent)
    {
        Log.Information("Timer trigger disparada às {DateTime}", DateTime.Now);

        using IServiceScope scope = _serviceProvider.CreateScope();
        IOrderService orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        await orderService.UpdatePaymentIdAsync(orderPendingPaymentCreatedEvent.OrderId, orderPendingPaymentCreatedEvent.PaymentId);
        await _eventPublisher.PublishAsync(new SendPendingEmailEvent(orderPendingPaymentCreatedEvent.UserId, "Compra criada", "Sua compra foi criada e está pendente de pagamento"), "send.pending.email");

        Log.Information("Processamento do pedido de Id {OrderId} finalizado.", orderPendingPaymentCreatedEvent.OrderId);
    }
}
