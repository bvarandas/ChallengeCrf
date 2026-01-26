

namespace CashFlow.Frontend.Clients;
public class DailyConsolidatedClient(HttpClient httpClient)
{
    public async Task<DailyConsolidatedSummary[]> GetDailyConsolidatedAsync()
    {
        var daily = await httpClient.GetFromJsonAsync<DailyConsolidatedSummary>($"dailyconsolidated?date={DateTime.Now.ToString("MM/dd/yyyy")}");
        var list = new List<DailyConsolidatedSummary>();
        
        if (daily is not null)
            list.Add(daily);

        return list.ToArray();
    }
}