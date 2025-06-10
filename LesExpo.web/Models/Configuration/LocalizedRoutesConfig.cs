namespace LesExpo.web.Models.Configuration
{
    /// <summary>
    /// Configuration model for localized routes
    /// </summary>
    public class LocalizedRoutesConfig
    {
        public const string SectionName = "LocalizedRoutes";
        
        /// <summary>
        /// Controllers and their localized routes
        /// </summary>
        public Dictionary<string, ControllerRoutes> Controllers { get; set; } = new();
        
        /// <summary>
        /// List of supported languages (e.g., "en", "tr")
        /// </summary>
        public List<string> SupportedLanguages { get; set; } = new();
        
        /// <summary>
        /// Default language to use as fallback
        /// </summary>
        public string DefaultLanguage { get; set; } = "en";
        
        /// <summary>
        /// Gets localized route for a controller action
        /// </summary>
        public string? GetRoute(string controller, string action, string language)
        {
            return Controllers.TryGetValue(controller, out var controllerRoutes) &&
                   controllerRoutes.TryGetValue(action, out var actionRoutes) &&
                   actionRoutes.TryGetValue(language.ToLower(), out var route)
                ? route
                : null;
        }
        
        /// <summary>
        /// Checks if a route is valid for the given language and controller/action
        /// </summary>
        public bool IsValidRoute(string controller, string action, string language, string route)
        {
            var expectedRoute = GetRoute(controller, action, language);
            return string.Equals(expectedRoute, route, StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// Gets the correct language for a given route and controller/action
        /// </summary>
        public string? GetLanguageForRoute(string controller, string action, string route)
        {
            if (!Controllers.TryGetValue(controller, out var controllerRoutes) ||
                !controllerRoutes.TryGetValue(action, out var actionRoutes))
            {
                return null;
            }
            
            return actionRoutes.FirstOrDefault(kvp => 
                string.Equals(kvp.Value, route, StringComparison.OrdinalIgnoreCase)).Key;
        }
        
        /// <summary>
        /// Gets all routes for a specific controller and action
        /// </summary>
        public Dictionary<string, string> GetAllRoutesForAction(string controller, string action)
        {
            return Controllers.TryGetValue(controller, out var controllerRoutes) &&
                   controllerRoutes.TryGetValue(action, out var actionRoutes)
                ? actionRoutes
                : new Dictionary<string, string>();
        }
    }
    
    /// <summary>
    /// Controller routes: Action -> Language -> Route
    /// Example: "Index" -> { "en": "all-news", "tr": "tum-haberler" }
    /// </summary>
    public class ControllerRoutes : Dictionary<string, ActionRoutes>
    {
    }
    
    /// <summary>
    /// Action routes: Language -> Route
    /// Example: { "en": "all-news", "tr": "tum-haberler" }
    /// </summary>
    public class ActionRoutes : Dictionary<string, string>
    {
    }
} 