using FiapCloudGames.Games.API.Extensions;
using FiapCloudGames.Games.Application.Interfaces;
using FiapCloudGames.Games.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Games.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMyOrders()
    {
        RestResponse<IEnumerable<OrderViewModel>> restResponse = await _orderService.GetOrdersByUserIdAsync(User.UserId());
        return Ok(restResponse);
    }

    [HttpPost]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateForGameIds([FromBody] HashSet<int> gamesIds)
    {
        await _orderService.CreateForGameIdsAsync(User.UserId(), gamesIds);
        return NoContent();
    }

    [HttpPatch("{orderId}/update-paymentId/{paymentId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePaymentIdAsync(int orderId, int paymentId)
    {
        await _orderService.UpdatePaymentIdAsync(orderId, paymentId);
        return NoContent();
    }

    [HttpPatch("{orderId}/unlock-games")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnlockGamesFromOrderToUser(int orderId)
    {
        await _orderService.UnlockGamesFromOrderToUserAsync(orderId);
        return NoContent();
    }

    [HttpPatch("{orderId}/cancel")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Cancel(int orderId)
    {
        await _orderService.CancelByIdAsync(orderId);
        return NoContent();
    }
}
