// MyComponentViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc.Models;
using System.Xml.Linq;

public class HeadingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("Heading");
    }
}
