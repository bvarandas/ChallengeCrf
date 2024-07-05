using ChallengeCrf.Application.ViewModel;
using ChallengeCrf.Domain.Events;
using Newtonsoft.Json;
namespace ChallengeCrf.Application.EventSourceNormalizes;

public class CashFlowHistory
{
    public static IList<CashFlowHistoryData> HistoryData { get; set; } = null!;

    public static IList<CashFlowHistoryData> ToJavaScriptRegisterHistory(IList<StoredEvent> storedEvents)
    {
        HistoryData = new List<CashFlowHistoryData>();
        RegisterHistoryDeserializer(storedEvents);

        var sorted = HistoryData.OrderBy(c => c.When);
        var list = new List<CashFlowHistoryData>();
        var last = new CashFlowHistoryData();

        foreach (var change in sorted)
        {
            var jsSlot = new CashFlowHistoryData
            {
                RegisterId = change.RegisterId == string.Empty || change.RegisterId == last.RegisterId? "": change.RegisterId,
                Description = string.IsNullOrWhiteSpace(change.Description) || change.Description == last.Description ? "" : change.Description,
                Status = string.IsNullOrWhiteSpace(change.Status) || change.Status == last.Status ? "" : change.Status,
                Date = change.Date,
                Action = string.IsNullOrWhiteSpace(change.Action) ? "" : change.Action,
                When = change.When,
                Who = change.Who
            };

            list.Add(jsSlot);
            last = change;
        }
        return list;
    }

    private static void RegisterHistoryDeserializer(IEnumerable<StoredEvent> storedEvents)
    {
        foreach (var e in storedEvents)
        {
            var slot = new CashFlowHistoryData();
            dynamic values;

            switch (e.MessageType)
            {
                case "CashFlowInsertedEvent":
                    values = JsonConvert.DeserializeObject<dynamic>(e.Data);
                    slot.Description = values["Description"];
                    slot.Status = values["Status"];
                    slot.Date = values["Date"];
                    slot.Action = "Inserted";
                    slot.When = values["Timestamp"];
                    slot.RegisterId = values["RegisterId"];
                    slot.Who = e.User;
                    break;
                case "CashFlowUpdatedEvent":
                    values = JsonConvert.DeserializeObject<dynamic>(e.Data);
                    slot.Description = values["Description"];
                    slot.Status = values["Status"];
                    slot.Date = values["Date"];
                    slot.Action = "Updated";
                    slot.When = values["Timestamp"];
                    slot.RegisterId = values["RegisterId"];
                    slot.Who = e.User;
                    break;
                case "CashFlowRemovedEvent":
                    values = JsonConvert.DeserializeObject<dynamic>(e.Data);
                    slot.Action = "Removed";
                    slot.When = values["Timestamp"];
                    slot.RegisterId = values["RegisterId"];
                    slot.Who = e.User;
                    break;
            }
            HistoryData.Add(slot);
        }
    }
}