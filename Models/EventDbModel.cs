using Microsoft.Data.SqlClient;
using System.Text.Json.Serialization;

namespace mvc.Models
{
    public class EventDbModel
    {
        public EventDbDataModel? data { get; set; }

        public int? count { get; set; }

        public string? response { get; set; }

        public string? status { get; set; }

    }

    public class EventDbDataModel
    {
        public string? jsonResult { get; set; }
        public List<Dictionary<string, object>>? objResult { get; set; }

    }

    public class EventPostDataModel
    {
        public int? current_page { get; set; }
        public int? per_page { get; set; }
        public string? event_id { get; set; }
        public string? event_name { get; set; }
        public DateTime? date { get; set; }
        public string? description { get; set; }

        public string? content_body { get; set; }

    }
}
