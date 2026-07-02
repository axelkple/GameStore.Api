using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetgameEndpointName = "GetGame";

 
    public static void MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/games");

        // GET /games(all)
        group.MapGet("/", async (GameStoreContext dbcontext)
         => await dbcontext.Games
                           .Include(game => game.Genre)
                           .Select(game => new GameDto(

                            game.Id,
                            game.Name,
                            game.Genre!.Name,
                            game.Price,
                            game.Releasedate

        ))
        .AsNoTracking()
        .ToListAsync());

        // GET/games/id
        group.MapGet("/{id}", async (int id, GameStoreContext dbcontext) =>
        {
            var game = await dbcontext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(

            new GameDetailsDto(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.Releasedate
            )
            );

        }).WithName(GetgameEndpointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbcontext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                Releasedate = newGame.Releasedate
            };

            dbcontext.Games.Add(game);
            dbcontext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.Releasedate
            );

            return Results.CreatedAtRoute(GetgameEndpointName, new { id = gameDto.Id }, gameDto);
        });


        // PUT/games/id

        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStoreContext dbcontext ) =>
        {
            var existingGame = await dbcontext.Games.FindAsync(id);
            if (existingGame is null)
            {

                return Results.NoContent();
            }
                existingGame.Name = updateGame.Name;
                existingGame.GenreId = updateGame.GenreId;
                existingGame.Price = updateGame.Price;
                existingGame.Releasedate = updateGame.Releasedate;

                await dbcontext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /games/id

        group.MapDelete("/{id}", async (int id, GameStoreContext dbcontext) =>
        {
            await dbcontext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();

        });

    }

    private static object GameDto(int id, string name1, string name2, decimal price, DateOnly releasedate)
    {
        throw new NotImplementedException();
    }
}