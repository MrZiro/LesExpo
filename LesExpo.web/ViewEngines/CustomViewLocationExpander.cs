using Microsoft.AspNetCore.Mvc.Razor;

namespace LesExpo.web.ViewEngines
{
    /// <summary>
    /// Custom view location expander for language-specific views.
    /// Only applies to public controllers, NOT to Areas (Admin, etc.)
    /// </summary>
    public class CustomViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // CRITICAL: Only process public controllers, never Areas
            if (IsAreaRequest(context))
            {
                return;
            }

            // Get language from route data (en/tr)
            var lang = context.ActionContext.RouteData.Values["lang"]?.ToString()
                       ?? "tr"; // Default to Turkish

            context.Values["lang"] = lang;
        }

        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            // CRITICAL: Never modify area view locations
            if (IsAreaRequest(context))
            {
                return viewLocations;
            }

            // Get language for public controllers
            if (!context.Values.TryGetValue("lang", out var lang))
            {
                lang = "tr"; // Default fallback
            }

            // Replace {2} placeholder with language (en/tr) for public views only
            return viewLocations.Select(location => location.Replace("{2}", lang));
        }

        /// <summary>
        /// Determines if the current request is for an Area controller
        /// </summary>
        private static bool IsAreaRequest(ViewLocationExpanderContext context)
        {
            return context.ActionContext.RouteData.Values.ContainsKey("area") &&
                   !string.IsNullOrEmpty(context.ActionContext.RouteData.Values["area"]?.ToString());
        }
    }
}
