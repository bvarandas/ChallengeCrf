using AutoMapper;
using ChallengeCrf.Application.Dto;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
namespace ChallengeCrf.Application.AutoMapper;
public class DomainToDtoMappingProfile : Profile
{
    public DomainToDtoMappingProfile()
    {
        CreateMap<EnvelopeMessage<CashFlow>, CashFlowDto>()
            .ConstructUsing(c => new CashFlowDto(c.Body.CashFlowId,
                                                      c.Body.Description,
                                                      c.Body.Amount,
                                                      c.Body.Entry,
                                                      c.Body.Date,
                                                      c.Action));
        CreateMap<EnvelopeMessage<CashFlow>, CashFlowDto>()
            .ConstructUsing(c => new CashFlowDto(c.Body.CashFlowId,
                                                        c.Body.Description,
                                                        c.Body.Amount,
                                                        c.Body.Entry,
                                                        c.Body.Date,
                                                        c.Action));

        CreateMap<CashFlow, CashFlowDto>()
            .ConstructUsing(c => new CashFlowDto(c.CashFlowId,
                                                      c.Description,
                                                      c.Amount,
                                                      c.Entry,
                                                      c.Date,
                                                      ""));
        CreateMap<CashFlow, CashFlowDto>()
            .ConstructUsing(c => new CashFlowDto(c.CashFlowId,
                                                        c.Description,
                                                        c.Amount,
                                                        c.Entry,
                                                        c.Date,
                                                        ""));


    }
}