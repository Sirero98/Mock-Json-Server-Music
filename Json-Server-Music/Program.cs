using Json_Server_Music.Routes;
using Json_Server_Music.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

var store = new JsonStore();
var songs = store.Load();

app.MapSongRoutes(songs, store);

app.Run();
