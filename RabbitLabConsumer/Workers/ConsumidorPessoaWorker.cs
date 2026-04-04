using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitLab.Consumer.Workers;

public class ConsumidorPessoaWorker : BackgroundService
{
    private readonly ILogger<ConsumidorPessoaWorker> _logger;
    private readonly ConnectionFactory _factory;
    private const string QueueName = "isaac.queue.pessoas";

    public ConsumidorPessoaWorker(ILogger<ConsumidorPessoaWorker> logger)
    {
        _logger = logger;
        _factory = new ConnectionFactory { HostName = "localhost" };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var connection = await _factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        
        await channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        _logger.LogInformation(" Aguardando mensagens na fila: {queue}", QueueName);

     
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation(" [x] Mensagem Recebida: {message}", message);

       
            await Task.Delay(500, stoppingToken);

            
            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
        };

     
        await channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}