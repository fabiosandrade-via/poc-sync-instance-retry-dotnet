// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using poc_consumer_spot_instance_srv;

Console.WriteLine("Consumo de tópico iniciado...");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<ConsumerBrokerKafka>();
using IHost host = builder.Build();

_ = host.Services.GetService<ConsumerBrokerKafka>();
