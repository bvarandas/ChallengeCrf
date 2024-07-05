using Amazon.Runtime.Internal.Util;
using AutoFixture;
using AutoMapper;
using ChallengeCrf.Application.Commands;
using ChallengeCrf.Application.Services;
using ChallengeCrf.Domain.Bus;
using ChallengeCrf.Domain.Interfaces;
using ChallengeCrf.Domain.Models;
using ChallengeCrf.Infra.Data.Repository.EventSourcing;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ChallengeCrf.Tests.Services;

public class CashFlowServiceTests
{
    private CashFlowService cashFlowService;

    public CashFlowServiceTests()
    {
        cashFlowService = new CashFlowService(
            new Mock<IMapper>().Object,
            new Mock<ICashFlowRepository>().Object,
            new Mock<IMediatorHandler>().Object,
            new Mock<IEventStoreRepository>().Object,
            new Mock<ILogger<CashFlowService>>().Object);

    }

    //[Fact]
    //public void AddCashFlowAsync_InsertSendingValidObject()
    //{
    //    var mediatorMoq = new Mock<IMediator>();
    //    var fixture = new Fixture();
    //    var handlerInsert = fixture.Create<InsertCashFlowCommand>();
    //    mediatorMoq.Setup(x => x.Send(new InsertCashFlowCommand("total recall insert", 55.66, "Debito", DateTime.Now)))
    //        .Returns(Task.FromResult(handlerInsert));
    //    //var cashFlow = new CashFlow("total recall insert", 55.66, "Debito", DateTime.Now, "insert");
    //    var result = cashFlowService.AddCashFlowAsync(cashFlow);
    //    Assert.NotNull(result);
    //}
    public void AddCashFlowAsync_InsertNotNull()
    {
        var cashFlow = new InsertCashFlowCommand(
            "total recall insert",
            55.66,
            "Debito",
            DateTime.Now);
        
        var result = cashFlowService.AddCashFlowAsync(cashFlow);
        Assert.NotNull(result);
    }

    [Fact]
    public async void AddCashFlowAsync_InsertNull()
    {
        //var cashFlow = new InsertCashFlowCommand("total recall insert", 55.66, "Debito", DateTime.Now);
        InsertCashFlowCommand cashFlow = null;
        var result =  cashFlowService.AddCashFlowAsync(cashFlow);
        Assert.Null(result);
    }

    [Fact]
    public void UpdateCashFlowAsync_InsertNotNull()
    {
        var cashFlow = new UpdateCashFlowCommand("507f1f77bcf86cd799439011", "total recall update", 66.77, "Debito", DateTime.Now);
        var result = cashFlowService.UpdateCashFlowAsync(cashFlow);
        Assert.NotNull(result);
    }

    [Fact]
    public void UpdateCashFlowAsync_InsertNull()
    {
        var cashFlow = new UpdateCashFlowCommand("507f1f77bcf86cd799439011","total recall update", 66.77, "Debito", DateTime.Now);
        var result = cashFlowService.UpdateCashFlowAsync(cashFlow);
        Assert.Null(result);
    }


    [Fact]
    public void GetListAllAsync_ReturnNull()
    {
        var result = cashFlowService.GetListAllAsync();
        Assert.True(result.IsCompleted);
        Assert.False(result.IsFaulted);
        Assert.Null(result.Result);
    }

    [Fact]
    public void GetCashFlowyIDAsync_SendingValidIdNotFound()
    {
        var result = cashFlowService.GetCashFlowyIDAsync("");
        Assert.True(result.IsCompleted);
        Assert.False(result.IsFaulted);
        Assert.Null(result.Result);

        //var exception = Assert.ThrowsAsync<Exception>(() => cashFlowService.GetCashFlowyIDAsync(""));
        //
    }

    [Fact]
    public void RemoveCashFlowAsync_SendingValidIdNotFound()
    {
        var result = cashFlowService.GetCashFlowyIDAsync("");
        Assert.True(result.IsCompleted);
        Assert.False(result.IsFaulted);
        Assert.Null(result.Result);

        //var exception = Assert.ThrowsAsync<Exception>(() => cashFlowService.GetCashFlowyIDAsync(""));
        //
    }

    [Fact]
    public void GetCashFlowyIDAsync_SendingValidId()
    {
        var result = cashFlowService.GetCashFlowyIDAsync("507f1f77bcf86cd799439011");
        Assert.True(result.IsCompleted);
        Assert.False(result.IsFaulted);
        Assert.NotNull(result);

        //var exception = Assert.ThrowsAsync<Exception>(() => cashFlowService.GetCashFlowyIDAsync(""));
        //
    }
}
