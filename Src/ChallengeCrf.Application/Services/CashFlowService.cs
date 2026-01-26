using AutoMapper;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.Dto;
using ChallengeCrf.Application.EventSourceNormalizes;
using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Application.ViewModel;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Infra.Data.Context;
using ChallengeCrf.Infra.Data.Repository.EventSourcing;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
namespace ChallengeCrf.Application.Services;

public class CashFlowService : ICashFlowService
{
    private readonly IMapper _mapper;

    private readonly ICashFlowRepository _cashFlowRepository;
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly ILogger<CashFlowService> _logger;
    protected readonly OutboxContext _dbContext;
    public CashFlowService(
        IMapper mapper,
        ICashFlowRepository registerRepository,
        IEventStoreRepository eventStoreRepository,
        IOutboxCache outboxRepository,
        ILogger<CashFlowService> logger)
    {
        _mapper = mapper;
        _cashFlowRepository = registerRepository;
        _eventStoreRepository = eventStoreRepository;
        _logger = logger;
    }

    public async Task<IAsyncEnumerable<CashFlowDto>> GetListAllAsync()
    {
        _logger.LogInformation("Tentando ir no GetAllCashFlowAsync");
        var enumarable = await _cashFlowRepository.GetAllCashFlowAsync();
        var listResult = new List<CashFlowDto>();

        await foreach (var item in enumarable.Value)
            listResult.Add(_mapper.Map<CashFlowDto>(item));

        return listResult.ToAsyncEnumerable();
    }

    public async Task<CashFlowDto> GetCashFlowyIDAsync(string cashFlowId)
    {
        var cashFlow = await _cashFlowRepository.GetCashFlowByIDAsync(cashFlowId);

        if (cashFlow.IsSuccess)
        {
            var result = _mapper.Map<CashFlowDto>(cashFlow.Value);
            return result;
        }
        return new CashFlowDto();
    }

    public async Task AddCashFlowAsync(CashFlowCommand command)
    {
        _logger.LogInformation("Tentando inserir no banco de dados");

        var cash = new CashFlow(ObjectId.GenerateNewId().ToString(),
            command.CashFlowId, command.Description, command.Amount, command.Entry, command.Date);

        await _cashFlowRepository.AddCashFlowAsync(cash);

    }

    public async Task UpdateCashFlowAsync(CashFlowCommand command)
    {
        var cash = new CashFlow(ObjectId.GenerateNewId().ToString(),
            command.CashFlowId, command.Description, command.Amount, command.Entry, command.Date);

        await _cashFlowRepository.UpdateCashFlowAsync(cash);
    }

    public async void RemoveCashFlowAsync(string cashFlowId)
    {
        await _cashFlowRepository.DeleteCashFlowAsync(cashFlowId);
    }

    public IList<CashFlowHistoryDto> GetAllHistory(int cashFlowId)
    {
        return CashFlowHistory.ToJavaScriptRegisterHistory(_eventStoreRepository.All(cashFlowId));
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}