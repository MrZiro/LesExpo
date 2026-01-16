namespace LesExpo.web.Services
{
    public interface IContentIndexService
    {
        Task<object> GetSearchIndexAsync(string language, string query = "");
        string ExtractTextFromView(string viewPath);
        object CreatePageSearchItem(string pageKey, string title, string content, string language, string query = "");
    }
} 