﻿using FiapCloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Games.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(p => p.OrderId);

        builder.Property(p => p.UserId).IsRequired();

        builder.Property(p => p.PaymentId).IsRequired(false);

        builder.Property(p => p.OrderedAt).IsRequired();

        builder.Property(p => p.CanceledAt).IsRequired(false);

        builder.HasMany(p => p.Games)
               .WithMany(g => g.Orders);

        builder.HasMany(p => p.OrderEvents)
               .WithOne(oe => oe.Order)
               .HasForeignKey(oe => oe.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
