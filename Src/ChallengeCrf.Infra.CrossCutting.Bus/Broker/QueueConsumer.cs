using ChallengeCrf.Domain.Extesions;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChallengeCrf.Api.Producer;

public class QueueConsumer : BackgroundService, IQueueConsumer
{
    private readonly ILogger<QueueConsumer> _logger;
    private readonly QueueEventSettings _queueSettings;
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceScopeFactory _serviceProvider;

    public QueueConsumer(
        IOptions<QueueEventSettings> queueSettings,
        ILogger<QueueConsumer> logger,
        IServiceScopeFactory provider)
    {
        _logger = logger;
        _queueSettings = queueSettings.Value;
        _factory = new ConnectionFactory()
        {
            HostName = _queueSettings.HostName,
            Port = 5672,
        };

        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(_queueSettings.QueueNameCashFlow,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.QueueDeclare(_queueSettings.QueueNameDailyConsolidated,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _serviceProvider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Aguardando mensagens Event...");

        var consumerCashFlow = new EventingBasicConsumer(_channel);
        consumerCashFlow.Received += Consumer_ReceivedCashFlow;

        var consumerDailyConsolidated = new EventingBasicConsumer(_channel);
        consumerDailyConsolidated.Received += ConsumerDailyConsolidated_Received;


        _channel.BasicConsume(
            queue: _queueSettings.QueueNameCashFlow,
            autoAck: false,
            consumer: consumerCashFlow);

        _channel.BasicConsume(
            queue: _queueSettings.QueueNameDailyConsolidated,
            autoAck: false,
            consumer: consumerDailyConsolidated);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_queueSettings.Interval, stoppingToken);
        }
    }

    private async void ConsumerDailyConsolidated_Received(object? sender, BasicDeliverEventArgs e)
    {
        try
        {
            _logger.LogInformation("Chegou mensagem nova");
            var message = e.Body.ToArray().DeserializeFromByteArrayProtobuf<DailyConsolidated>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BrokerHub>>();

                await hubContext.Clients.Groups("CrudMessage").SendAsync("ReceiveMessageDC", message);
            }

            _channel.BasicAck(e.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            _channel.BasicNack(e.DeliveryTag, false, true);
        }
    }



    private async void Consumer_ReceivedCashFlow(object? sender, BasicDeliverEventArgs e)
    {
        try
        {
            _logger.LogInformation("Chegou mensagem nova");
            var messageList = e.Body.ToArray().DeserializeFromByteArrayProtobuf<List<CashFlow>>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BrokerHub>>();

                await hubContext.Clients.Groups("CrudMessage").SendAsync("ReceiveMessageCF", messageList);
            }

            _channel.BasicAck(e.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            var messageList = e.Body.ToArray().DeserializeFromByteArrayProtobuf<EnvelopeMessage<List<CashFlow>>>();
            var oType = messageList.GetType();

            if (oType.IsGenericType &&
                oType.GetGenericTypeDefinition() == typeof(List<CashFlow>))
            {
                _channel.BasicAck(e.DeliveryTag, false);
            }
            else
            {
                _channel.BasicNack(e.DeliveryTag, false, true);
            }
        }
    }
}
