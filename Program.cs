using GameStore.Api.Data;


using GameStore.Api.Endpoints;




var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidation();

builder.AddGameStoreDb();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGamesEndpoints();
app.MapGenreEndpoints();


app.MigrateDb();



app.Run();
