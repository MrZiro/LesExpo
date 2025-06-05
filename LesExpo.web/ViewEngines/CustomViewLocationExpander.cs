using Microsoft.AspNetCore.Mvc.Razor;

namespace LesExpo.web.ViewEngines
{

    public class CustomViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // Don't interfere with Area views
            if (context.ActionContext.RouteData.Values.ContainsKey("area"))
            {
                return;
            }
            // Read "lang" from RouteData or query
            var lang = context.ActionContext.RouteData.Values["lang"]?.ToString()
                       ?? "tr"; // fallback to "tr" if not provided

            context.Values["lang"] = lang;
        }

        public IEnumerable<string> ExpandViewLocations(
        ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
        {
            // If it's an area, don't modify the view locations
            if (context.ActionContext.RouteData.Values.ContainsKey("area"))
            {
                return viewLocations;
            }

            if (!context.Values.TryGetValue("lang", out var lang))
            {
                lang = "tr";
            }

            // Replace {2} with lang
            return viewLocations.Select(location => location.Replace("{2}", lang));
        }
    }
}
