using System.Threading.Tasks;
using CashFlow.Frontend.Models;

namespace CashFlow.Frontend.Clients;

public class CashFlowClient(HttpClient httpClient)
{
    public async Task<CashFlowSummary[]> GetCashFlowAsync()
        => await httpClient.GetFromJsonAsync<CashFlowSummary[]>("cashflow/") ?? [];
   
    public async Task AddCashFlowAsync(CashFlowDetails cash)
        => await httpClient.PostAsJsonAsync("cashflow/", cash);

    public async Task<CashFlowDetails> GetCashFlowAsync(string id)
        => await httpClient.GetFromJsonAsync<CashFlowDetails>($"cashflow/{id}") ?? throw new Exception("Could not find Cash!");

    public async Task UpdateCashFlowAsync(CashFlowDetails updatedCash)
        => await httpClient.PutAsJsonAsync($"cashflow/{updatedCash.CashFlowId}", updatedCash);

    public async Task  DeleteCashFlowAsync(string id)
        => await httpClient.DeleteAsync($"cashFlow/{id}");
}