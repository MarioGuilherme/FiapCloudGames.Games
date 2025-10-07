namespace FiapCloudGames.Games.Domain.Entities;

public class Order(int userId)
{
    public int OrderId { get; private set; }
    public int? PaymentId { get; private set; }
    public int UserId { get; private set; } = userId;
    public DateTime OrderedAt { get; } = DateTime.Now;
    public DateTime? CanceledAt { get; private set; }
    public virtual ICollection<Game> Games { get; } = [];
    public virtual ICollection<OrderEvent> OrderEvents { get; } = [];

    public void UpdatePaymentId(int paymentId)
    {
        OrderEvents.Add(OrderEvent.FromOrder(this));
        PaymentId = paymentId;
    }

    public void Cancel()
    {
        OrderEvents.Add(OrderEvent.FromOrder(this));
        CanceledAt = DateTime.Now;
    }
}

