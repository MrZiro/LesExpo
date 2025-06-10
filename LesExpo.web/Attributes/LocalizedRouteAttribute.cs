using LesExpo.web.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LesExpo.web.Attributes
{
    /// <summary>
    /// Custom route attribute that automatically generates localized routes from configuration
    /// Usage: [LocalizedRoute("Blogs", "Index")]
    /// </summary>
    public class LocalizedRouteAttribute : Attribute, IRouteTemplateProvider
    {
        private readonly string _controller;
        private readonly string _action;
        
        public LocalizedRouteAttribute(string controller, string action)
        {
            _controller = controller;
            _action = action;
        }
        
        public string? Template { get; private set; }
        public int? Order { get; set; }
        public string? Name { get; set; }
        
        /// <summary>
        /// This method is called by ASP.NET Core to get the route template
        /// Note: This is a simplified implementation. For production, you might want to use IActionModelConvention
        /// </summary>
        public void SetTemplate(IServiceProvider serviceProvider)
        {
            var routesConfig = serviceProvider.GetService<IOptions<LocalizedRoutesConfig>>()?.Value;
            if (routesConfig == null) return;
            
            var routes = routesConfig.GetAllRoutesForAction(_controller, _action);
            if (routes.Any())
            {
                // Create a template that matches all language routes
                var routeTemplates = routes.Values.Select(route => route);
                Template = string.Join("|", routeTemplates);
            }
        }
    }
    
    /// <summary>
    /// Extension method to help generate route attributes from configuration
    /// </summary>
    public static class ConfigurationRouteHelper
    {
        /// <summary>
        /// Generates HttpGet attributes for all configured routes of an action
        /// </summary>
        public static List<HttpGetAttribute> GenerateHttpGetAttributes(LocalizedRoutesConfig config, string controller, string action)
        {
            var attributes = new List<HttpGetAttribute>();
            var routes = config.GetAllRoutesForAction(controller, action);
            
            foreach (var route in routes.Values)
            {
                attributes.Add(new HttpGetAttribute(route));
            }
            
            return attributes;
        }
    }
} 