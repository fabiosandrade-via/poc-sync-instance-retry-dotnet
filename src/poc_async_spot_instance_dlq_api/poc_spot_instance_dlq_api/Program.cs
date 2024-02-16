using poc_async_spot_instance_dlq_api.Resilience;
using poc_async_spot_instance_dlq_api.Service;
using Polly.Retry;
using Polly.Wrap;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AsyncRetryPolicy>(ResilienceExtensions.CreateResiliencePolicy(new[]
{
    TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(6)
}));

builder.Services.AddSingleton<ISpotInstanceService, SpotInstanceService>();

builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "POC - Retry + DLQ",
            Version = "v1",
            Description = "Cenário Assíncrono: Avaliação para utilização junto a Spot Instance"
        }
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
