using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RabbitLab.Domain.Interface;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitLabWorker.Workers;

public class ProcessadorMensagemWorker : BackgroundService
{
    private readonly ILogger<ProcessadorMensagemWorker> _logger;
    private readonly IPessoaRepository _repository;
    private readonly ConnectionFactory _factory;
    private readonly string _queueName;

    public ProcessadorMensagemWorker(
        ILogger<ProcessadorMensagemWorker> logger,
        IPessoaRepository repository,
        IConfiguration configuration)
    {
        _logger = logger;
        _repository = repository;

        _queueName = configuration.GetValue<string>("RabbitMqConfig:QueueName") ?? "fila_padrao";

        _factory = new ConnectionFactory { HostName = "localhost" };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("=== Worker de Mensagens Iniciado ===");
        _logger.LogInformation("Monitorando a fila: {queue}", _queueName);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var pendentes = await _repository.ListarPendentesAsync();

                if (pendentes.Any())
                {
                    using var connection = await _factory.CreateConnectionAsync(stoppingToken);
                    using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

                    await channel.QueueDeclareAsync(
                        queue: _queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null,
                        cancellationToken: stoppingToken);

                    foreach (var pessoa in pendentes)
                    {
                        _logger.LogInformation("Processando: {nome} (Id: {id})", pessoa.Nome, pessoa.Id);

                        var json = JsonSerializer.Serialize(pessoa);
                        var body = Encoding.UTF8.GetBytes(json);

                        await channel.BasicPublishAsync(
                            exchange: string.Empty,
                            routingKey: _queueName,
                            body: body,
                            cancellationToken: stoppingToken);

                        _logger.LogInformation("Mensagem enviada para o RabbitMQ com sucesso!");

                        pessoa.Status = "PROCESSADO";
                        await _repository.AtualizarStatusAsync(pessoa);

                        _logger.LogInformation("Status da Pessoa {id} atualizado para PROCESSADO no SQL.", pessoa.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro crítico no Worker: {message}", ex.Message);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
}