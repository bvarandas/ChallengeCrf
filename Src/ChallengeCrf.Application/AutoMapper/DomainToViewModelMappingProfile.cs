using AutoMapper;
using ChallengeCrf.Application.ViewModel;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Domain.ValueObjects;
namespace ChallengeCrf.Application.AutoMapper;
public  class DomainToViewModelMappingProfile : Profile
{
    public DomainToViewModelMappingProfile() 
    {
        CreateMap<EnvelopeMessage<CashFlow>, CashFlowViewModel>()
            .ConstructUsing(c => new CashFlowViewModel(c.Body.CashFlowId, 
                                                      c.Body.Description, 
                                                      c.Body.Amount, 
                                                      c.Body.Entry, 
                                                      c.Body.Date, 
                                                      c.Action));
        CreateMap<EnvelopeMessage<CashFlow>, CashFlowViewModel>()
            .ConstructUsing(c => new CashFlowViewModel(c.Body.CashFlowId, 
                                                        c.Body.Description, 
                                                        c.Body.Amount, 
                                                        c.Body.Entry, 
                                                        c.Body.Date, 
                                                        c.Action));

        CreateMap<CashFlow, CashFlowViewModel>()
            .ConstructUsing(c => new CashFlowViewModel(c.CashFlowId,
                                                      c.Description,
                                                      c.Amount,
                                                      c.Entry,
                                                      c.Date,
                                                      ""));
        CreateMap<CashFlow, CashFlowViewModel>()
            .ConstructUsing(c => new CashFlowViewModel(c.CashFlowId,
                                                        c.Description,
                                                        c.Amount,
                                                        c.Entry,
                                                        c.Date,
                                                        ""));


    }
}