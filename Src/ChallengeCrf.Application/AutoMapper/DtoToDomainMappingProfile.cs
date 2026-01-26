using AutoMapper;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.Dto;
using ChallengeCrf.Domain.Models;

namespace ChallengeCrf.Application.AutoMapper;

public class DtoToDomainMappingProfile : Profile
{
    public DtoToDomainMappingProfile()
    {
        CreateMap<CashFlowDto, InsertCashFlowCommand>()
                .ConstructUsing(c => new InsertCashFlowCommand(c.Description, c.Amount, c.Entry, c.Date));

        CreateMap<CashFlowDto, UpdateCashFlowCommand>()
            .ConstructUsing(c => new UpdateCashFlowCommand(c.CashFlowId, c.Description, c.Amount, c.Entry, c.Date));


        CreateMap<CashFlow, InsertCashFlowCommand>()
                .ConstructUsing(c => new InsertCashFlowCommand(c.Description, c.Amount, c.Entry, c.Date));

        CreateMap<CashFlow, UpdateCashFlowCommand>()
            .ConstructUsing(c => new UpdateCashFlowCommand(c.CashFlowId, c.Description, c.Amount, c.Entry, c.Date));

        //CreateMap<CashFlow, CashFlowCommand>()
        //    .ConstructUsing(c => new CashFlowCommand(c.CashFlowId, c.Description, c.Amount, c.Entry, c.Date));


    }
}
