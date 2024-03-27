using poc_sync_spot_instance_retry_api.Resilience;
using poc_sync_spot_instance_retry_api.Service;
using Polly;
using Polly.Wrap;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AsyncPolicyWrap>(ResilienceExtensions.CreateResiliencePolicy(new[]
{
    TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(6)
}, 4, 30));

builder.Services.AddSingleton<ISpotInstanceService, SpotInstanceService>();

builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "POC - Retry + Circuit Breaker",
            Version = "v1",
            Description = "Cenário Síncrono: Avaliação para utilização junto a Spot Instance"
        }
    ));

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
