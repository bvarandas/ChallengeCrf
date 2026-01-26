using ChallengeCrf_Front.ViewModels;

namespace ChallengeCrf_Front.Clients;

public class DailyConsolidatedClient
{
    public readonly HttpClient _httpClient;
    public DailyConsolidatedClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<DailyConsolidatedSummary[]> GetDailyConsolidatedAsync()
    {
        var daily = await _httpClient.GetFromJsonAsync<DailyConsolidatedSummary>($"dailyconsolidated?date={DateTime.Now.ToString("MM/dd/yyyy")}");
        var list = new List<DailyConsolidatedSummary>();

        if (daily is not null)
            list.Add(daily);

        return list.ToArray();
    }
}
