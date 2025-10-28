using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace LesExpo.web.Services
{
    public class HtmlContentService : IHtmlContentService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HtmlContentService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string ProcessEditorContentImages(string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent))
            {
                return htmlContent;
            }

            try
            {
                // Use HtmlAgilityPack to parse HTML safely
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                // Extract all image sources from content
                var imgNodes = htmlDoc.DocumentNode.SelectNodes("//img");
                if (imgNodes == null)
                {
                    return htmlContent; // No images to process
                }

                // Process temporary images
                ProcessTempImages(htmlDoc, imgNodes);

                // Return the updated HTML content
                return htmlDoc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                // Log the exception - in a production app you'd use proper logging
                System.Diagnostics.Debug.WriteLine($"Error processing editor images: {ex.Message}");
                return htmlContent; // Return original content if processing fails
            }
        }

        public string ProcessEditorContentVideos(string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent))
            {
                return htmlContent;
            }

            try
            {
                // Use HtmlAgilityPack to parse HTML safely
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                // Extract all video sources from content
                var videoNodes = htmlDoc.DocumentNode.SelectNodes("//video");
                if (videoNodes == null)
                {
                    return htmlContent; // No videos to process
                }

                // Process temporary videos
                ProcessTempVideos(htmlDoc, videoNodes);

                // Return the updated HTML content
                return htmlDoc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                // Log the exception - in a production app you'd use proper logging
                System.Diagnostics.Debug.WriteLine($"Error processing editor videos: {ex.Message}");
                return htmlContent; // Return original content if processing fails
            }
        }

        public string ProcessEditorContent(string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent))
            {
                return htmlContent;
            }

            // Process both images and videos
            string processedContent = ProcessEditorContentImages(htmlContent);
            processedContent = ProcessEditorContentVideos(processedContent);

            return processedContent;
        }

        public string ProcessEditedContent(string originalHtml, string editedHtml)
        {
            if (string.IsNullOrEmpty(editedHtml))
            {
                return editedHtml;
            }

            try
            {
                // Parse original and edited content
                var originalDoc = new HtmlDocument();
                originalDoc.LoadHtml(originalHtml ?? string.Empty);

                var editedDoc = new HtmlDocument();
                editedDoc.LoadHtml(editedHtml);

                // Extract image sources from original content
                var originalImgNodes = originalDoc.DocumentNode.SelectNodes("//img");
                HashSet<string> originalImageFiles = new HashSet<string>();

                if (originalImgNodes != null)
                {
                    foreach (var imgNode in originalImgNodes)
                    {
                        string srcAttr = imgNode.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(srcAttr) && srcAttr.Contains("/uploads/Blogs/editor/"))
                        {
                            originalImageFiles.Add(Path.GetFileName(srcAttr));
                        }
                    }
                }

                // Extract video sources from original content
                var originalVideoNodes = originalDoc.DocumentNode.SelectNodes("//video");
                HashSet<string> originalVideoFiles = new HashSet<string>();

                if (originalVideoNodes != null)
                {
                    foreach (var videoNode in originalVideoNodes)
                    {
                        // Check video source elements
                        var sourceNodes = videoNode.SelectNodes(".//source");
                        if (sourceNodes != null)
                        {
                            foreach (var sourceNode in sourceNodes)
                            {
                                string srcAttr = sourceNode.GetAttributeValue("src", "");
                                if (!string.IsNullOrEmpty(srcAttr) && srcAttr.Contains("/uploads/Blogs/video/"))
                                {
                                    originalVideoFiles.Add(Path.GetFileName(srcAttr));
                                }
                            }
                        }

                        // Also check direct src attribute on video element
                        string videoSrcAttr = videoNode.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(videoSrcAttr) && videoSrcAttr.Contains("/uploads/Blogs/video/"))
                        {
                            originalVideoFiles.Add(Path.GetFileName(videoSrcAttr));
                        }
                    }
                }

                // Extract image sources from edited content
                var editedImgNodes = editedDoc.DocumentNode.SelectNodes("//img");
                HashSet<string> editedImageFiles = new HashSet<string>();

                if (editedImgNodes == null)
                {
                    // No images in edited content, delete all images from original content
                    DeleteImagesFromOriginalContent(originalImageFiles);
                }
                else
                {
                    // Process permanent images in edited content
                    foreach (var imgNode in editedImgNodes)
                    {
                        string srcAttr = imgNode.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(srcAttr) && srcAttr.Contains("/uploads/Blogs/editor/"))
                        {
                            editedImageFiles.Add(Path.GetFileName(srcAttr));
                        }
                    }

                    // Find images that were in original but not in edited content (deleted images)
                    originalImageFiles.ExceptWith(editedImageFiles);
                    DeleteImagesFromOriginalContent(originalImageFiles);

                    // Process temporary images in the edited content
                    ProcessTempImages(editedDoc, editedImgNodes);
                }

                // Extract video sources from edited content
                var editedVideoNodes = editedDoc.DocumentNode.SelectNodes("//video");
                HashSet<string> editedVideoFiles = new HashSet<string>();

                if (editedVideoNodes == null)
                {
                    // No videos in edited content, delete all videos from original content
                    DeleteVideosFromOriginalContent(originalVideoFiles);
                }
                else
                {
                    // Process permanent videos in edited content
                    foreach (var videoNode in editedVideoNodes)
                    {
                        // Check video source elements
                        var sourceNodes = videoNode.SelectNodes(".//source");
                        if (sourceNodes != null)
                        {
                            foreach (var sourceNode in sourceNodes)
                            {
                                string srcAttr = sourceNode.GetAttributeValue("src", "");
                                if (!string.IsNullOrEmpty(srcAttr) && srcAttr.Contains("/uploads/Blogs/video/"))
                                {
                                    editedVideoFiles.Add(Path.GetFileName(srcAttr));
                                }
                            }
                        }

                        // Also check direct src attribute on video element
                        string videoSrcAttr = videoNode.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(videoSrcAttr) && videoSrcAttr.Contains("/uploads/Blogs/video/"))
                        {
                            editedVideoFiles.Add(Path.GetFileName(videoSrcAttr));
                        }
                    }

                    // Find videos that were in original but not in edited content (deleted videos)
                    originalVideoFiles.ExceptWith(editedVideoFiles);
                    DeleteVideosFromOriginalContent(originalVideoFiles);

                    // Process temporary videos in the edited content
                    ProcessTempVideos(editedDoc, editedVideoNodes);
                }

                // Return the updated HTML content
                return editedDoc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                // Log the exception - in a production app you'd use proper logging
                System.Diagnostics.Debug.WriteLine($"Error processing edited content: {ex.Message}");
                return editedHtml; // Return original edited content if processing fails
            }
        }

        private void ProcessTempImages(HtmlDocument htmlDoc, HtmlNodeCollection imgNodes)
        {
            if (imgNodes == null || htmlDoc == null)
                return;

            string tempDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Temp");
            string permanentDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Blogs", "editor");

            // Ensure permanent directory exists
            if (!Directory.Exists(permanentDirectory))
            {
                Directory.CreateDirectory(permanentDirectory);
            }

            // Process each temporary image in the content
            foreach (var imgNode in imgNodes)
            {
                string srcAttr = imgNode.GetAttributeValue("src", "");

                // Only process images from the temp folder
                if (!string.IsNullOrEmpty(srcAttr) && srcAttr.Contains("/uploads/Temp/"))
                {
                    // Extract filename from URL
                    string fileName = Path.GetFileName(srcAttr);
                    string tempFilePath = Path.Combine(tempDirectory, fileName);
                    string permanentFilePath = Path.Combine(permanentDirectory, fileName);

                    // Move the file if it exists in temp folder
                    if (System.IO.File.Exists(tempFilePath))
                    {
                        // Move file from temp to permanent folder
                        if (!System.IO.File.Exists(permanentFilePath))
                        {
                            System.IO.File.Move(tempFilePath, permanentFilePath);
                        }
                        else
                        {
                            // If file already exists in destination, delete the temp file
                            System.IO.File.Delete(tempFilePath);
                        }

                        // Update the src attribute in the HTML
                        imgNode.SetAttributeValue("src", srcAttr.Replace("/uploads/Temp/", "/uploads/Blogs/editor/"));
                    }
                }
            }
        }

        private void ProcessTempVideos(HtmlDocument htmlDoc, HtmlNodeCollection videoNodes)
        {
            if (videoNodes == null || htmlDoc == null)
                return;

            string tempDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Temp");
            string permanentDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Blogs", "video");

            // Ensure permanent directory exists
            if (!Directory.Exists(permanentDirectory))
            {
                Directory.CreateDirectory(permanentDirectory);
            }

            // Process each temporary video in the content
            foreach (var videoNode in videoNodes)
            {
                // Check video source elements
                var sourceNodes = videoNode.SelectNodes(".//source");
                if (sourceNodes != null)
                {
                    foreach (var sourceNode in sourceNodes)
                    {
                        string srcAttr = sourceNode.GetAttributeValue("src", "");

                        // Only process videos from the temp folder
                        if (!string.IsNullOrEmpty(srcAttr) && srcAttr.Contains("/uploads/Temp/"))
                        {
                            // Extract filename from URL
                            string fileName = Path.GetFileName(srcAttr);
                            string tempFilePath = Path.Combine(tempDirectory, fileName);
                            string permanentFilePath = Path.Combine(permanentDirectory, fileName);

                            // Move the file if it exists in temp folder
                            if (System.IO.File.Exists(tempFilePath))
                            {
                                // Move file from temp to permanent folder
                                if (!System.IO.File.Exists(permanentFilePath))
                                {
                                    System.IO.File.Move(tempFilePath, permanentFilePath);
                                }
                                else
                                {
                                    // If file already exists in destination, delete the temp file
                                    System.IO.File.Delete(tempFilePath);
                                }

                                // Update the src attribute in the HTML
                                sourceNode.SetAttributeValue("src", srcAttr.Replace("/uploads/Temp/", "/uploads/Blogs/video/"));
                            }
                        }
                    }
                }

                // Also check direct src attribute on video element
                string videoSrcAttr = videoNode.GetAttributeValue("src", "");
                if (!string.IsNullOrEmpty(videoSrcAttr) && videoSrcAttr.Contains("/uploads/Temp/"))
                {
                    // Extract filename from URL
                    string fileName = Path.GetFileName(videoSrcAttr);
                    string tempFilePath = Path.Combine(tempDirectory, fileName);
                    string permanentFilePath = Path.Combine(permanentDirectory, fileName);

                    // Move the file if it exists in temp folder
                    if (System.IO.File.Exists(tempFilePath))
                    {
                        // Move file from temp to permanent folder
                        if (!System.IO.File.Exists(permanentFilePath))
                        {
                            System.IO.File.Move(tempFilePath, permanentFilePath);
                        }
                        else
                        {
                            // If file already exists in destination, delete the temp file
                            System.IO.File.Delete(tempFilePath);
                        }

                        // Update the src attribute in the HTML
                        videoNode.SetAttributeValue("src", videoSrcAttr.Replace("/uploads/Temp/", "/uploads/Blogs/video/"));
                    }
                }
            }
        }

        private void DeleteImagesFromOriginalContent(HashSet<string> originalImageFiles)
        {
            if (originalImageFiles.Count == 0)
            {
                return;
            }

            string permanentDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Blogs", "editor");

            foreach (var fileName in originalImageFiles)
            {
                string filePath = Path.Combine(permanentDirectory, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                        System.Diagnostics.Debug.WriteLine($"Deleted unused image: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error deleting image {fileName}: {ex.Message}");
                    }
                }
            }
        }

        private void DeleteVideosFromOriginalContent(HashSet<string> originalVideoFiles)
        {
            if (originalVideoFiles.Count == 0)
            {
                return;
            }

            string permanentDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Blogs", "video");

            foreach (var fileName in originalVideoFiles)
            {
                string filePath = Path.Combine(permanentDirectory, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                        System.Diagnostics.Debug.WriteLine($"Deleted unused video: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error deleting video {fileName}: {ex.Message}");
                    }
                }
            }
        }

        public void DeleteContentImages(string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent))
            {
                return;
            }

            try
            {
                // Use HtmlAgilityPack to parse HTML safely
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                // Extract all image sources from content
                var imgNodes = htmlDoc.DocumentNode.SelectNodes("//img");
                if (imgNodes != null)
                {
                    string blogsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Blogs", "editor");

                    // Process each image
                    foreach (var imgNode in imgNodes)
                    {
                        string srcAttr = imgNode.GetAttributeValue("src", "");

                        // Only process images from the Blog/editor folder
                        if (!string.IsNullOrEmpty(srcAttr) && srcAttr.Contains("/uploads/Blogs/editor/"))
                        {
                            // Extract filename from URL
                            string fileName = Path.GetFileName(srcAttr);
                            string filePath = Path.Combine(blogsDirectory, fileName);

                            // Delete the file if it exists
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                    }
                }

                // Extract all video sources from content
                var videoNodes = htmlDoc.DocumentNode.SelectNodes("//video");
                if (videoNodes != null)
                {
                    string videoDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Blogs", "video");

                    // Process each video
                    foreach (var videoNode in videoNodes)
                    {
                        // Check video source elements
                        var sourceNodes = videoNode.SelectNodes(".//source");
                        if (sourceNodes != null)
                        {
                            foreach (var sourceNode in sourceNodes)
                            {
                                string srcAttr = sourceNode.GetAttributeValue("src", "");
                                if (!string.IsNullOrEmpty(srcAttr) && srcAttr.Contains("/uploads/Blogs/video/"))
                                {
                                    string fileName = Path.GetFileName(srcAttr);
                                    string filePath = Path.Combine(videoDirectory, fileName);
                                    if (System.IO.File.Exists(filePath))
                                    {
                                        System.IO.File.Delete(filePath);
                                    }
                                }
                            }
                        }

                        // Also check direct src attribute on video element
                        string videoSrcAttr = videoNode.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(videoSrcAttr) && videoSrcAttr.Contains("/uploads/Blogs/video/"))
                        {
                            string fileName = Path.GetFileName(videoSrcAttr);
                            string filePath = Path.Combine(videoDirectory, fileName);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception - in a production app you'd use proper logging
                System.Diagnostics.Debug.WriteLine($"Error deleting content media: {ex.Message}");
            }
        }
    }
}