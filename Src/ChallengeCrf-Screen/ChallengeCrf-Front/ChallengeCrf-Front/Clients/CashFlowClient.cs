using ChallengeCrf_Front.ViewModels;

namespace ChallengeCrf_Front.Clients;

public class CashFlowClient
{
    private readonly HttpClient _httpClient;
    public CashFlowClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<CashFlowSummary[]> GetCashFlowAsync()
        => await _httpClient.GetFromJsonAsync<CashFlowSummary[]>("cashflow/") ?? new CashFlowSummary[0];

    public async Task AddCashFlowAsync(CashFlowDetails cash)
        => await _httpClient.PostAsJsonAsync("cashflow/", cash);

    public async Task<CashFlowDetails> GetCashFlowAsync(string id)
        => await _httpClient.GetFromJsonAsync<CashFlowDetails>($"cashflow/{id}") ?? throw new Exception("Could not find Cash!");

    public async Task UpdateCashFlowAsync(CashFlowDetails updatedCash)
        => await _httpClient.PutAsJsonAsync($"cashflow/{updatedCash.CashFlowId}", updatedCash);

    public async Task DeleteCashFlowAsync(string id)
        => await _httpClient.DeleteAsync($"cashFlow/{id}");
}
