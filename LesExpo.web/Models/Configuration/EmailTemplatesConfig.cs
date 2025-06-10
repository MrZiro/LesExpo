namespace LesExpo.web.Models.Configuration
{
    /// <summary>
    /// Configuration model for email templates
    /// </summary>
    public class EmailTemplatesConfig
    {
        public const string SectionName = "EmailTemplates";
        
        /// <summary>
        /// Email templates organized by type (Contact, etc.)
        /// </summary>
        public Dictionary<string, Dictionary<string, EmailTemplate>> Templates { get; set; } = new();
        
        /// <summary>
        /// Gets email template for a specific type and language
        /// </summary>
        public EmailTemplate? GetTemplate(string templateType, string language)
        {
            return Templates.TryGetValue(templateType, out var typeTemplates) &&
                   typeTemplates.TryGetValue(language.ToLower(), out var template)
                ? template
                : null;
        }
        
        /// <summary>
        /// Gets contact email template for a specific language
        /// </summary>
        public EmailTemplate? GetContactTemplate(string language)
        {
            return GetTemplate("Contact", language);
        }
    }
    
    /// <summary>
    /// Email template for a specific language
    /// </summary>
    public class EmailTemplate
    {
        public string SubjectPrefix { get; set; } = string.Empty;
        public string ConfirmationSubject { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ValidationMessage { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
        public string ConfirmationGreeting { get; set; } = string.Empty;
        public string ConfirmationText { get; set; } = string.Empty;
        public string ConfirmationFooter { get; set; } = string.Empty;
        
        /// <summary>
        /// Formats the subject with the prefix
        /// </summary>
        public string FormatSubject(string subject)
        {
            return $"{SubjectPrefix} {subject}";
        }
        
        /// <summary>
        /// Formats the confirmation greeting with the name
        /// </summary>
        public string FormatGreeting(string name)
        {
            return ConfirmationGreeting.Replace("{name}", name);
        }
    }
} 