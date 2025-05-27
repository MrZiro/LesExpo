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

                // Extract image sources from edited content
                var editedImgNodes = editedDoc.DocumentNode.SelectNodes("//img");
                HashSet<string> editedImageFiles = new HashSet<string>();
                
                if (editedImgNodes == null)
                {
                    // No images in edited content, delete all images from original content
                    DeleteImagesFromOriginalContent(originalImageFiles);
                    return editedHtml;
                }

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
                if (imgNodes == null)
                {
                    return; // No images to process
                }

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
            catch (Exception ex)
            {
                // Log the exception - in a production app you'd use proper logging
                System.Diagnostics.Debug.WriteLine($"Error deleting content images: {ex.Message}");
            }
        }
    }
} 