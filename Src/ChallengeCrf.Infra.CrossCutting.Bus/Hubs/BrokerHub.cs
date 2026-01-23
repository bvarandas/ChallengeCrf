using Microsoft.AspNetCore.SignalR;
namespace ChallengeCrf.Infra.CrossCutting.Bus;

public class BrokerHub : Hub
{
    public Task ConnectToMessageBroker()
    {
        Groups.AddToGroupAsync(Context.ConnectionId, "CrudMessage");

        return Task.CompletedTask;
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.Caller.SendAsync("ReceiveMessage", user, message);
    }
}
