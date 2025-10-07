using Elastic.Clients.Elasticsearch;
using FiapCloudGames.Games.Application.Interfaces;
using FiapCloudGames.Games.Application.ViewModels;
using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Exceptions;
using FiapCloudGames.Games.Infrastructure.ElasticSearch.Models;
using FiapCloudGames.Games.Infrastructure.ElasticSearch;
using FiapCloudGames.Games.Infrastructure.Persistence;
using Serilog;

namespace FiapCloudGames.Games.Application.Services;

public class OrderService(IUnitOfWork unitOfWork, ElasticsearchClient elasticsearchClient) : IOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IElasticClient<GameElasticSearchModel> _elasticClient = new ElasticClient<GameElasticSearchModel>(elasticsearchClient);

    public async Task CancelByIdAsync(int orderId)
    {
        Log.Information("Iniciando cancelamento do pedido {orderId}", orderId);
        Order? order = await _unitOfWork.Orders.GetByIdTrackingAsync(orderId);
        if (order is null)
        {
            Log.Warning("Pedido {orderId} não encontrado", orderId);
            throw new OrderNotFoundException();
        }
        order.Cancel();
        Log.Information("Pedido {paymentId} cancelado com sucesso", orderId);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<RestResponse> CreateForGameIdsAsync(int userId, IEnumerable<int> gamesIds)
    {
        Log.Information("Iniciando criação do pedido para o usuário {userId} com {totalGames} jogos", userId, gamesIds.Count());
        List<Game> gamesFromUser = await _unitOfWork.Games.GetAllByUserId(userId);
        Order order = new(userId);

        Log.Information("Filtrando os jogos do pedido do usuário {userId} excluindo os que já possui", userId);
        foreach (int gameId in gamesIds)
        {
            if (gamesFromUser.Any(g => g.GameId == gameId))
            {
                Log.Warning("Jogo {gameId} já pertence a biblioteca do usuário {userId}, ignorando jogo da lista", gameId, userId);
                continue;
            }
            Game? game = await _unitOfWork.Games.GetByIdTrackingAsync(gameId);
            if (game is null)
            {
                Log.Warning("Jogo {gameId} não encontrado para o pedido do usuário {userId}", gameId, userId);
                throw new GameNotFoundException();
            }
            order.Games.Add(game);
        }

        if (order.Games.Count == 0)
        {
            Log.Warning("Nenhum jogo presente na lista não foi comprado o usuário {userId}", userId);
            return RestResponse.Success(); // Se nenhum jogo, ignora a compra
        }

        await _unitOfWork.Orders.CreateAsync(order);
        await _unitOfWork.CompleteAsync();

        Log.Information("Pedido {orderId} criado com sucesso para o usuário {userId}", order.OrderId, userId);
        return RestResponse.Success();
    }

    public async Task<RestResponse<IEnumerable<OrderViewModel>>> GetOrdersByUserIdAsync(int userId)
    {
        Log.Information("Buscando pedidos do usuário {userId}", userId);
        List<Order> orders = await _unitOfWork.Orders.GetAllByUserIdAsync(userId);
        Log.Information("{totalOrders} pedidos encontrados para o usuário {userId}", orders.Count, userId);
        return RestResponse<IEnumerable<OrderViewModel>>.Success(orders.Select(OrderViewModel.FromDomain));
    }

    public async Task UnlockGamesFromOrderToUserAsync(int orderId)
    {
        Log.Information("Desbloqueando jogos do pedido {orderId}", orderId);
        Order? order = await _unitOfWork.Orders.GetByIdTrackingAsync(orderId);
        if (order is null)
        {
            Log.Warning("Pedido {orderId} não encontrado", orderId);
            throw new OrderNotFoundException();
        }

        Log.Information("Desbloqueando {totalGames} jogos do pedido {orderId} para o usuário {userId}", order.Games.Count, orderId, order.UserId);
        await _unitOfWork.Games.UnlockGamesFromOrderToUserAsync(order.UserId, order.Games);
        await _unitOfWork.CompleteAsync();
        Log.Information("Jogos do pedido {orderId} desbloqueados com sucesso para o usuário {userId}", orderId, order.UserId);

        Log.Information("Atualizando total de vendas dos jogos do pedido {orderId} no ElasticSearch", orderId);
        foreach (Game game in order.Games)
        {
            GameElasticSearchModel gameElasticSearchModel = (await _elasticClient.GetByIdAsync(game.GameId))!;
            gameElasticSearchModel.TotalSales++;
            await _elasticClient.UpinsertAsync(gameElasticSearchModel);
        }
    }

    public async Task UpdatePaymentIdAsync(int orderId, int paymentId)
    {
        Log.Information("Atualizando o Id do pagamento do pedido {orderId} para {paymentId}", orderId, paymentId);
        Order? order = await _unitOfWork.Orders.GetByIdTrackingAsync(orderId);
        if (order is null)
        {
            Log.Warning("Pedido {orderId} não encontrado", orderId);
            throw new OrderNotFoundException();
        }
        order.UpdatePaymentId(paymentId);
        await _unitOfWork.CompleteAsync();
        Log.Information("Id do pagamento do pedido {orderId} atualizado com sucesso para {paymentId}", orderId, paymentId);
    }
}
