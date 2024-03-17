using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using System.Diagnostics;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string event_id)
        {
            int count = MYSQLEvents.Count("SELECT COUNT(*) FROM events", []);
            EventDbDataModel data = MYSQLEvents.Query("SELECT * FROM events", [], [], count, 0, 5);
            List<Dictionary<string, object>> resultList = data.objResult;
            var model = new EventModel
            {
                count = count,
                resultList = resultList
            };
            return View("Index", model);
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("content")]
        public IActionResult Content()
        {
            return View("Content");
        }
    }
}
