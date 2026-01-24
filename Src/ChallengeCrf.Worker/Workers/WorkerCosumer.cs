using AutoMapper;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Application.ViewModel;
using ChallengeCrf.Appplication.Interfaces;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Constants;
using ChallengeCrf.Domain.Extesions;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChallengeCrf.Queue.Worker.Workers;

public class WorkerConsumer : BackgroundService, IWorkerConsumer
{
    private ICashFlowService _flowService;

    private readonly IDailyConsolidatedService _dailyConsolidatedService;
    private readonly ILogger<WorkerConsumer> _logger;
    private readonly QueueCommandSettings _queueSettings;
    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediatorHandler;
    public WorkerConsumer(IOptions<QueueCommandSettings> queueSettings,
        ILogger<WorkerConsumer> logger,
        ICashFlowService cashFlowService,
        IDailyConsolidatedService dailyConsolidatedService,
        IMediatorHandler mediatorHandler,
        IMapper mapper)
    {
        _logger = logger;
        _queueSettings = queueSettings.Value;
        _dailyConsolidatedService = dailyConsolidatedService;
        _mapper = mapper;
        _flowService = cashFlowService;
        _mediatorHandler = mediatorHandler;
    }

    public override Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"O hostname é {_queueSettings.HostName}");

        _factory = new ConnectionFactory()
        {
            HostName = _queueSettings.HostName,
            Port = _queueSettings.Port,
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

        _logger.LogInformation("Aguardando mensagens Command...");

        var consumerCashFlow = new EventingBasicConsumer(_channel);
        consumerCashFlow.Received += ConsumerCashFlow_Received;

        var consumerDailyConsolidated = new EventingBasicConsumer(_channel);
        consumerDailyConsolidated.Received += ConsumerDailyConsolidated_Received;

        _channel.BasicConsume(queue: _queueSettings.QueueNameCashFlow, autoAck: false, consumer: consumerCashFlow);
        _channel.BasicConsume(queue: _queueSettings.QueueNameDailyConsolidated, autoAck: false, consumer: consumerDailyConsolidated);

        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    private async void ConsumerDailyConsolidated_Received(object? sender, BasicDeliverEventArgs e)
    {
        try
        {
            _logger.LogInformation("Chegou nova mensagem no Worker Consumer");
            var message = e.Body.ToArray().DeserializeFromByteArrayProtobuf<EnvelopeMessage<DailyConsolidated>>();
            _logger.LogInformation("Conseguiu Deserializar a mensagem");

            switch (message.Action)
            {
                case "get":
                    var dailyConsolidated = await _dailyConsolidatedService.GetDailyConsolidatedByDateAsync(message.Body.Date);
                    var list = new List<DailyConsolidated>();

                    if (dailyConsolidated.IsSuccess && dailyConsolidated.Value is not null)
                        await WorkerProducer._Singleton.PublishMessages(dailyConsolidated.Value);

                    break;
            }

            _channel.BasicAck(e.DeliveryTag, true);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            _channel.BasicNack(e.DeliveryTag, false, true);
        }
    }

    private async void ConsumerCashFlow_Received(object? sender, BasicDeliverEventArgs e)
    {
        try
        {
            _logger.LogInformation("Chegou nova mensagem no Worker Consumer");
            var messageEnvelope = e.Body.ToArray().DeserializeFromByteArrayProtobuf<EnvelopeMessage<CashFlow>>();
            var message = messageEnvelope.Body;
            _logger.LogInformation("Conseguiu Deserializar a mensagem");
            _logger.LogInformation($"{message.Description} | {message.Amount}| {message.Entry} | {message.Date} | {messageEnvelope.Action}");


            switch (messageEnvelope.Action)
            {
                case UserAction.Insert:
                    var commandInsert = _mapper.Map<CashFlow, InsertCashFlowCommand>(message);
                    await _mediatorHandler.SendCommand(commandInsert);

                    break;

                case UserAction.Update:
                    var commandUpdate = _mapper.Map<CashFlow, UpdateCashFlowCommand>(message);
                    await _mediatorHandler.SendCommand(commandUpdate);

                    break;

                case UserAction.Delete:
                    _flowService.RemoveCashFlowAsync(message.CashFlowId);
                    break;

                case UserAction.GetAll:

                    var cashlist = await _flowService.GetListAllAsync();

                    if (cashlist is not null)
                    {
                        await WorkerProducer._Singleton.PublishMessages(cashlist.ToListAsync().Result);
                    }
                    break;

                case UserAction.Get:
                    var cash = await _flowService.GetCashFlowyIDAsync(message.CashFlowId);
                    var list = new List<CashFlowViewModel>();
                    if (cash is not null)
                    {
                        list.Add(cash);
                        await WorkerProducer._Singleton.PublishMessages(list);
                    }
                    break;

            }

            _channel.BasicAck(e.DeliveryTag, true);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            //_channel.BasicNack(e.DeliveryTag, false, true);
            _channel.BasicAck(e.DeliveryTag, true);
        }
    }
}