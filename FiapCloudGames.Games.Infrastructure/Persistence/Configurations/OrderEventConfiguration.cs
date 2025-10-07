using FiapCloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Games.Infrastructure.Persistence.Configurations;

public class OrderEventConfiguration : IEntityTypeConfiguration<OrderEvent>
{
    public void Configure(EntityTypeBuilder<OrderEvent> builder)
    {
        builder.HasKey(ou => ou.OrderEventId);

        builder.Property(ou => ou.UserId).IsRequired();

        builder.Property(ou => ou.PaymentId).IsRequired(false);

        builder.Property(ou => ou.OrderedAt).IsRequired();

        builder.Property(ou => ou.CanceledAt).IsRequired(false);

        builder.Property(ou => ou.EventAt)
            .HasDefaultValue(DateTime.Now);

        builder.HasOne(ou => ou.Order)
               .WithMany(o => o.OrderEvents)
               .HasForeignKey(ou => ou.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("OrderEvents");
    }
}
