using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const String GetGameEndPointName = "GetGame";
        public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes){
            var group = routes.MapGroup("/games")
                        .WithParameterValidation();

            group.MapGet("/", async (IGamesRepository repository) =>
            (await repository.GetAllAsync()).Select(game => game.AsDto()));

            group.MapGet("/{id}",async (IGamesRepository repository,int id) =>{
                Game? game = await repository.GetAsync(id);
                return game is not null ?   Results.Ok(game.AsDto()): Results.NotFound();
                }).WithName(GetGameEndPointName);
                
            group.MapPost("/", async (IGamesRepository repository,CreateGameDto gameDto)=>{
                Game game = new(){
                    Name = gameDto.Name,
                    Genre = gameDto.Genre,
                    Price = gameDto.Price,
                    ReleaseDate = gameDto.ReleaseDate,
                    ImageUri = gameDto.ImageUri
                };
                await repository.CreateAsync(game);
                return Results.CreatedAtRoute(GetGameEndPointName,new {id = game.Id}, game);
            });

            group.MapPut("/{id}", async (IGamesRepository repository,int id, UpdateGameDto updatedGameDto) =>{
                Game? ExistingGame = await repository.GetAsync(id);
                if(ExistingGame is null){
                    return Results.NotFound();
            }
            ExistingGame.Name = updatedGameDto.Name;
            ExistingGame.Genre = updatedGameDto.Genre;
            ExistingGame.Price = updatedGameDto.Price;
            ExistingGame.ImageUri = updatedGameDto.ImageUri;
            await repository.UpdateAsync(ExistingGame);
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (IGamesRepository repository, int id)=>{
            Game? game = await repository.GetAsync(id);
            if(game is not null){
                await repository.DeleteAsync(id);
            }
            return Results.NoContent();
        });
        return group;
    }
}