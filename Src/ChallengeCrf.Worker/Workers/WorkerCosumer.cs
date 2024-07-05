using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.Extesions;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Appplication.Interfaces;
using AutoMapper;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.ViewModel;
using ChallengeCrf.Domain.ValueObjects;

namespace ChallengeCrf.Queue.Worker.Workers;

public class WorkerConsumer : BackgroundService, IWorkerConsumer
{
    private IWorkerProducer _workerProducer;
    private ICashFlowService _flowService;
    private readonly IDailyConsolidatedService _dailyConsolidatedService;
    private readonly ILogger<WorkerConsumer> _logger;
    private readonly QueueCommandSettings _queueSettings;
    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;
    private readonly ConcurrentQueue<CashFlow> _queueRegister;
    private readonly IMapper _mapper;
    public WorkerConsumer(IOptions<QueueCommandSettings> queueSettings, 
        ILogger<WorkerConsumer> logger, 
        ICashFlowService registerService,
        IDailyConsolidatedService dailyConsolidatedService,
        IWorkerProducer workerProducer,
        IMapper mapper)
    {
        _logger = logger;
        _queueSettings = queueSettings.Value;
        _dailyConsolidatedService = dailyConsolidatedService;
        _mapper = mapper;
        _queueRegister = new ConcurrentQueue<CashFlow>();
        _flowService = registerService;
        _workerProducer = workerProducer;
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
                    
                    if (dailyConsolidated.IsSuccess)
                        await WorkerProducer._Singleton.PublishMessages(dailyConsolidated.Value);
                    
                break;
            }

            _channel.BasicAck(e.DeliveryTag, true);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            _channel.BasicNack(e.DeliveryTag, false, true);
            //_channel.BasicAck(e.DeliveryTag, true);
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

            _queueRegister.Enqueue(message);

            switch(messageEnvelope.Action)
            {
                case "insert":
                    var commandInsert = _mapper.Map<CashFlow, InsertCashFlowCommand>(message);
                    await _flowService.AddCashFlowAsync(commandInsert);
                    break;

                case "update":
                    var commandUpdate = _mapper.Map<CashFlow, UpdateCashFlowCommand>(message);
                    await _flowService.UpdateCashFlowAsync(commandUpdate);
                    break;

                case "remove":
                    _flowService.RemoveCashFlowAsync(message.CashFlowId);
                    break;

                case "getall":
                    
                    var registerlist = await _flowService.GetListAllAsync();
                    
                    if (registerlist is not null)
                    {
                        await WorkerProducer._Singleton.PublishMessages(registerlist.ToListAsync().Result);
                    }
                    break;

                case "get":
                    var register = await _flowService.GetCashFlowyIDAsync(message.CashFlowId);
                    var list = new List<CashFlowViewModel>();
                    if (register is not null)
                    {
                        list.Add(register);
                        await WorkerProducer._Singleton.PublishMessages(list);
                    }
                    break;

            }

            _channel.BasicAck(e.DeliveryTag, true);

        }catch (Exception ex) 
        {
            _logger.LogError(ex.Message, ex);
            //_channel.BasicNack(e.DeliveryTag, false, true);
            _channel.BasicAck(e.DeliveryTag, true);
        }
    }
}