using GameStore.Api.Data;
using GameStore.Api.Features.Baskets;
using GameStore.Api.Features.Baskets.Authorization;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using GameStore.Api.Shared.Authorization;
using GameStore.Api.Shared.Cdn;
using GameStore.Api.Shared.ErrorHandling;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails()
    .AddExceptionHandler<GlobalExceptionHandler>();

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration;
    options.CombineLogs = true;
});

builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

builder.AddFileUploader();
builder.AddGameStoreAuthentication();
builder.AddGameStoreAuthorization();
builder.Services
    .AddSingleton<IAuthorizationHandler, BasketAuthorizationHandler>();


builder.Services.AddScoped<CdnUrlTransformer>();
builder.Services.AddSingleton<AzureEventSourceLogForwarder>();

var app = builder.Build();

app.UseAuthorization();

app.MapGames();
app.MapGenres();
app.MapBaskets();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
    app.UseSwagger();
else
{
    app.Services.GetRequiredService<AzureEventSourceLogForwarder>().Start();
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

await app.InitializeDbAsync();

app.Run();