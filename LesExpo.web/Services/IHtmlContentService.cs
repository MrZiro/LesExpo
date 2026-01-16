namespace LesExpo.web.Services
{
    public interface IHtmlContentService
    {
        /// <summary>
        /// Processes images in HTML content, moving them from temp to permanent storage
        /// </summary>
        /// <param name="htmlContent">HTML content with image tags</param>
        /// <returns>Updated HTML content with modified image paths</returns>
        string ProcessEditorContentImages(string htmlContent);

        /// <summary>
        /// Processes videos in HTML content, moving them from temp to permanent storage
        /// </summary>
        /// <param name="htmlContent">HTML content with video tags</param>
        /// <returns>Updated HTML content with modified video paths</returns>
        string ProcessEditorContentVideos(string htmlContent);

        /// <summary>
        /// Processes both images and videos in HTML content, moving them from temp to permanent storage
        /// </summary>
        /// <param name="htmlContent">HTML content with media tags</param>
        /// <returns>Updated HTML content with modified media paths</returns>
        string ProcessEditorContent(string htmlContent);

        /// <summary>
        /// Processes edited HTML content, comparing original and edited versions
        /// to handle moved, deleted, and new images properly
        /// </summary>
        /// <param name="originalHtml">Original HTML content before editing</param>
        /// <param name="editedHtml">New edited HTML content</param>
        /// <returns>Updated HTML content with modified image paths</returns>
        string ProcessEditedContent(string originalHtml, string editedHtml);

        /// <summary>
        /// Deletes all images referenced in HTML content
        /// </summary>
        /// <param name="htmlContent">HTML content with image tags</param>
        void DeleteContentImages(string htmlContent);
    }
}