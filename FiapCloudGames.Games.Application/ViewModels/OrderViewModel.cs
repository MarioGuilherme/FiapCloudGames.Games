using FiapCloudGames.Games.Domain.Entities;

namespace FiapCloudGames.Games.Application.ViewModels;

public record OrderViewModel(int OrderId, DateTime OrderedAt, IEnumerable<GameViewModel> Games)
{
    public static OrderViewModel FromDomain(Order order) => new(
        order.OrderId,
        order.OrderedAt,
        order.Games.Select(GameViewModel.FromDomain).ToList()
    );
}
