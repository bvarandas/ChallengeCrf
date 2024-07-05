using AutoMapper;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.EventSourceNormalizes;
using ChallengeCrf.Application.Interfaces;
using ChallengeCrf.Application.ViewModel;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Infra.Data.Repository.EventSourcing;
using Microsoft.Extensions.Logging;
namespace ChallengeCrf.Application.Services;

public  class CashFlowService : ICashFlowService
{
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediator;
    private readonly ICashFlowRepository _cashFlowRepository;
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly ILogger<CashFlowService> _logger;
    public CashFlowService(
        IMapper mapper, 
        ICashFlowRepository registerRepository, 
        IMediatorHandler mediator, 
        IEventStoreRepository eventStoreRepository,
        ILogger<CashFlowService> logger)
    {
        _mapper = mapper;
        _cashFlowRepository = registerRepository;
        _eventStoreRepository = eventStoreRepository;
        _mediator = mediator;
        _logger = logger;
    }
    public async Task<IAsyncEnumerable<CashFlowViewModel>> GetListAllAsync()
    {
        _logger.LogInformation("Tentando ir no GetAllCashFlowAsync");
        var enumarable = await _cashFlowRepository.GetAllCashFlowAsync();
        var listResult = new List<CashFlowViewModel>();
        
        await foreach (var item in enumarable.Value)
            listResult.Add(_mapper.Map<CashFlowViewModel>(item));

        //var result = _mapper.Map<IAsyncEnumerable<CashFlow>, IAsyncEnumerable<CashFlowViewModel>>(await enumarable);
        return listResult.ToAsyncEnumerable();
    }

    public async Task<CashFlowViewModel> GetCashFlowyIDAsync(string cashFlowId)
    {
        var cashFlow = await _cashFlowRepository.GetCashFlowByIDAsync(cashFlowId);
        var result = _mapper.Map<CashFlowViewModel>(cashFlow);
        return  result;
    }

    public async Task AddCashFlowAsync(CashFlowCommand register)
    {
        _logger.LogInformation("Tentando inserir no banco de dados");

        var addCommand = _mapper.Map<InsertCashFlowCommand>(register);
        await _mediator.SendCommand(addCommand);
    }

    public async Task UpdateCashFlowAsync(CashFlowCommand register)
    {
        var updateCommand = _mapper.Map<UpdateCashFlowCommand>(register);
        await _mediator.SendCommand(updateCommand);
    }

    public async void RemoveCashFlowAsync(string cashFlowId)
    {
        var deleteCommand = new RemoveCashFlowCommand(cashFlowId);
        await _mediator.SendCommand(deleteCommand);
    }

    public IList<CashFlowHistoryData> GetAllHistory(int cashFlowId)
    {
        return CashFlowHistory.ToJavaScriptRegisterHistory(_eventStoreRepository.All(cashFlowId));
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}