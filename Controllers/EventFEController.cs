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
    [Route("event-fe/{event_id:}")]
    public class EventFEController : Controller
    {
        public IActionResult Event(string event_id)
        {
            int count = 1;
            EventDbDataModel data = MYSQLEvents.Query("SELECT * FROM events WHERE id=@event_id", ["event_id"], [event_id], count);
            List<Dictionary<string, object>> resultList = data.objResult;

            var model = new EventModel
            {
                count = count,
                resultList = resultList,
            };

            if(resultList.Count() == 0)
            {
                return View("NotFound");
            }

            return View("EventFE", model);
        }
    }
}
