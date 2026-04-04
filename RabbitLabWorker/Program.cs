using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration; 
using RabbitLabWorker.Workers;
using RabbitLab.Domain.Interface;
using RabbitLab.Infra.Repositories;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = Host.CreateApplicationBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=.\\MSSQLSERVER02;Database=EstudoMensageriaDb;Trusted_Connection=True;TrustServerCertificate=True";

builder.Services.AddTransient<IDbConnection>(sp => new SqlConnection(connectionString));

builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();

builder.Services.AddHostedService<ProcessadorMensagemWorker>();

using IHost host = builder.Build();
await host.RunAsync();