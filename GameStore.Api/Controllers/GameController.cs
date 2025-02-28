using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : Controller
{
    private readonly GameStoreContext gameStoreContext;

    public GameController(GameStoreContext gameStoreContext)
    {
        this.gameStoreContext = gameStoreContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await gameStoreContext.Games
            .Select(game => new GameDto(
                game.Id,
                game.Name,
                game.Genre,
                game.Price,
                game.ReleaseDate,
                game.ImageUri))
            .ToListAsync();
        return Ok(games);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGameById([FromRoute] int id)
    {
        var game = await gameStoreContext.Games
            .Where(g => g.Id == id)
            .Select(game => new GameDto(
                game.Id,
                game.Name,
                game.Genre,
                game.Price,
                game.ReleaseDate,
                game.ImageUri))
            .FirstOrDefaultAsync();

        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameDto createGameDto)
    {
        var game = new Game
        {
            Name = createGameDto.Name,
            Genre = createGameDto.Genre,
            Price = createGameDto.Price,
            ReleaseDate = createGameDto.ReleaseDate,
            ImageUri = createGameDto.ImageUri
        };

        gameStoreContext.Games.Add(game);
        await gameStoreContext.SaveChangesAsync();

        var gameDto = new GameDto(
            game.Id,
            game.Name,
            game.Genre,
            game.Price,
            game.ReleaseDate,
            game.ImageUri);

        return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, gameDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGame([FromRoute] int id, [FromBody] UpdateGameDto updateGameDto)
    {
        var game = await gameStoreContext.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        game.Name = updateGameDto.Name;
        game.Genre = updateGameDto.Genre;
        game.Price = updateGameDto.Price;
        game.ReleaseDate = updateGameDto.ReleaseDate;
        game.ImageUri = updateGameDto.ImageUri;

        await gameStoreContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGame([FromRoute] int id)
    {
        var game = await gameStoreContext.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        gameStoreContext.Games.Remove(game);
        await gameStoreContext.SaveChangesAsync();

        return NoContent();
    }
}