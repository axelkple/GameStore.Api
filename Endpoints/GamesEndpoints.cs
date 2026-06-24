using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetgameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [

 new (
       1,
       "street fighter II",
       "Fighting",
       19.99M,
       new DateOnly (1992, 7, 15)),

new (
       2,
       "Final Fantasy VII Rebirth",
       "RPG",
       69.99M,
       new DateOnly (2024, 2, 29)),
new (
       3,
       "Astro Bot",
       "Platformer",
       59.99M,
       new DateOnly (2024, 9, 6)),
 ];

    public static void MapGamesEndpoints(this WebApplication app)
    {

            var group = app.MapGroup("/games");

        // GET /games(all)
        group.MapGet("/", () => games);

        // GET/games/id
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);

        }).WithName(GetgameEndpointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.Realeasedate
            );

            games.Add(game);

            return Results.CreatedAtRoute(GetgameEndpointName, new { id = game.Id }, game);
        });


        // PUT/games/id

        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {

                return Results.NoContent();
            }
            games[index] = new GameDto(

                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.Realeasedate

            );

            return Results.NoContent();
        });

        // DELETE /games/id

        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();

        });

    }

}