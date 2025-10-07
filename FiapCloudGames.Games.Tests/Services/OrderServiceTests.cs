using Elastic.Clients.Elasticsearch;
using FiapCloudGames.Games.Application.Interfaces;
using FiapCloudGames.Games.Application.Services;
using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Exceptions;
using FiapCloudGames.Games.Infrastructure.ElasticSearch;
using FiapCloudGames.Games.Infrastructure.ElasticSearch.Models;
using FiapCloudGames.Games.Infrastructure.Persistence;
using Moq;

namespace FiapCloudGames.Games.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ElasticsearchClient> _elasticClient = new();
    private readonly IOrderService _orderService;

    public OrderServiceTests() => _orderService = new OrderService(_unitOfWork.Object, _elasticClient.Object);

    [Fact]
    public async Task CancelByIdAsync_ShouldCancelOrder_WhenOrderExists()
    {
        // Arrange
        Order order = new(1);

        _unitOfWork.Setup(u => u.Orders.GetByIdTrackingAsync(1)).ReturnsAsync(order);

        // Act
        await _orderService.CancelByIdAsync(1);

        // Assert
        Assert.True(order.CanceledAt is not null); // assumindo que Cancel() muda Status
        _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task CancelByIdAsync_ShouldThrowException_WhenOrderNotFound()
    {
        // Arrange
        _unitOfWork.Setup(u => u.Orders.GetByIdTrackingAsync(999)).ReturnsAsync((Order?)null);

        // Act & Assert
        await Assert.ThrowsAsync<OrderNotFoundException>(() => _orderService.CancelByIdAsync(999));
    }
}
