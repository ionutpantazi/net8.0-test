using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using mvc.Context;
using mvc.Models;
using Mysqlx.Crud;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace mvc.Controllers
{
    [Route("events")]
    public class EventController : Controller
    {
        public IActionResult Event(string event_id)
        {
            int count = MYSQLEvents.Count("SELECT COUNT(*) FROM events", []);
            EventDbDataModel data = MYSQLEvents.Query("SELECT * FROM events", [], [], count, 0, 10);
            List<Dictionary<string, object>> resultList = data.objResult;
            var model = new EventModel
            {
                count = count,
                resultList = resultList
            };
            return View("Event", model);
        }

    }

}
