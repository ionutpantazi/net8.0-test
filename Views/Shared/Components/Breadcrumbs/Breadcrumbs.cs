// MyComponentViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc.Models;
using System.Xml.Linq;

public class BreadcrumbsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("Breadcrumbs");
    }
}
