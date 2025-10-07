namespace FiapCloudGames.Games.Domain.Entities;

public class OrderEvent
{
    public int OrderEventId { get; private set; }
    public int OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public int? PaymentId { get; private set; }
    public int UserId { get; private set; }
    public DateTime OrderedAt { get; private set; }
    public DateTime? CanceledAt { get; private set; }
    public DateTime EventAt { get; } = DateTime.Now;

    private OrderEvent() { }

    public static OrderEvent FromOrder(Order order) => new()
    {
        OrderId = order.OrderId,
        PaymentId = order.PaymentId,
        UserId = order.UserId,
        OrderedAt = order.OrderedAt,
        CanceledAt = order.CanceledAt
    };
}
