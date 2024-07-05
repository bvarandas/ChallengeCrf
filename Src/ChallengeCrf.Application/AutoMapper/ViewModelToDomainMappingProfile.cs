using AutoMapper;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.ViewModel;
using ChallengeCrf.Domain.Models;

namespace ChallengeCrf.Application.AutoMapper;

public class ViewModelToDomainMappingProfile : Profile
{
    public ViewModelToDomainMappingProfile()
    {
        CreateMap<CashFlowViewModel, InsertCashFlowCommand>()
                .ConstructUsing(c => new InsertCashFlowCommand(c.Description, c.Amount, c.Entry, c.Date));
        
        CreateMap<CashFlowViewModel, UpdateCashFlowCommand>()
            .ConstructUsing(c => new UpdateCashFlowCommand(c.CashFlowId, c.Description, c.Amount, c.Entry,  c.Date));


        CreateMap<CashFlow, InsertCashFlowCommand>()
                .ConstructUsing(c => new InsertCashFlowCommand(c.Description, c.Amount, c.Entry, c.Date));

        CreateMap<CashFlow, UpdateCashFlowCommand>()
            .ConstructUsing(c => new UpdateCashFlowCommand(c.CashFlowId, c.Description, c.Amount,c.Entry, c.Date));

        //CreateMap<CashFlow, CashFlowCommand>()
        //    .ConstructUsing(c => new CashFlowCommand(c.CashFlowId, c.Description, c.Amount, c.Entry, c.Date));


    }
}
