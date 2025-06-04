using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace LesExpo.web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "en").ToLower();

        protected IActionResult LocalizedView(string viewName = null, object model = null)
        {
            var controllerName = ControllerContext.RouteData.Values["controller"]?.ToString();
            viewName ??= ControllerContext.RouteData.Values["action"]?.ToString();

            var viewPath = $"~/Views/{Lang}/{controllerName}/{viewName}.cshtml";
            var viewEngine = HttpContext.RequestServices.GetRequiredService<ICompositeViewEngine>();

            // Check if view exists
            var result = viewEngine.GetView("", viewPath, false);
            if (result.Success)
            {
                return View(viewPath, model);
            }

            // Fallback to English if localized view doesn't exist
            var fallbackPath = $"~/Views/en/{controllerName}/{viewName}.cshtml";
            return View(fallbackPath, model);
        }
    }
}
