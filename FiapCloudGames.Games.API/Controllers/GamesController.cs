using FiapCloudGames.Games.API.Extensions;
using FiapCloudGames.Games.Application.InputModels;
using FiapCloudGames.Games.Application.Interfaces;
using FiapCloudGames.Games.Application.ViewModels;
using FiapCloudGames.Games.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Games.API.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController(IGameService gameService) : ControllerBase
{
    private readonly IGameService _gameService = gameService;

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWithPagination(int page, int size, [FromQuery] string? name = default, [FromQuery] string? description = default)
    {
        RestResponse<IEnumerable<GameViewModel>> restResponse = await _gameService.GetWithPaginationAsync(page, size, name, description);
        return Ok(restResponse);
    }

    [HttpGet("{gameId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int gameId)
    {
        RestResponse<GameViewModel> restResponse = await _gameService.GetByIdAsync(gameId);
        return Ok(restResponse);
    }

    [HttpGet("most-popular")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMostPopularGames(int page, int size)
    {
        RestResponse<IEnumerable<GameViewModel>> restResponse = await _gameService.GetMostPopularGamesWithPaginationAsync(page, size);
        return Ok(restResponse);
    }

    [Authorize]
    [HttpGet("based-user-history")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBasedOnHistoryUser(int page, int size)
    {
        RestResponse<IEnumerable<RecommendedGameViewModel>> restResponse = await _gameService.GetBasedOnUserHistoryAsync(User.UserId(), page, size);
        return Ok(restResponse);
    }

    [Authorize]
    [HttpGet("my-games")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MyGames()
    {
        RestResponse<IEnumerable<GameViewModel>> restResponse = await _gameService.GetGamesByUserIdAsync(User.UserId());
        return Ok(restResponse);
    }

    [Authorize(Roles = nameof(UserType.Admin))]
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<CreatedAtActionResult> Create([FromBody] CreateGameInputModel inputModel)
    {
        RestResponse<GameViewModel> restResponse = await _gameService.CreateGameAsync(inputModel);
        return CreatedAtAction(nameof(GetById), new { gameId = restResponse.Data!.GameId }, restResponse);
    }

    [Authorize(Roles = nameof(UserType.Admin))]
    [HttpPatch("{gameId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int gameId, [FromBody] UpdateGameInputModel inputModel)
    {
        RestResponse<GameViewModel> restResponse = await _gameService.UpdateGameAsync(gameId, inputModel);
        return Ok(restResponse);
    }

    [Authorize(Roles = nameof(UserType.Admin))]
    [HttpDelete("{gameId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int gameId)
    {
        await _gameService.DeleteByIdAsync(gameId);
        return NoContent();
    }
}
