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
        return Accepted();
    }
}
