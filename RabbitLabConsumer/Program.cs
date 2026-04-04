using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitLab.Consumer.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<ConsumidorPessoaWorker>();

using IHost host = builder.Build();
await host.RunAsync();