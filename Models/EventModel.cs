
using System.Text.Json.Nodes;

namespace mvc.Models
{
    public class EventModel
    {
        public int? count { get; set; }
        
        public EventJsonModel json { get; set; }
        public List<Dictionary<string, object>> resultList { get; set; }
    }

    public class EventJsonModel
    {
        public string? id { get; set; }
        public string? event_name { get; set; }
        public DateTime? date { get; set; }
        public string? description { get; set; }

    }
}
