using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LesExpo.Utility;

namespace LesExpo.web.Services
{
    public class TempFileCleanupService : BackgroundService
    {
        private readonly ILogger<TempFileCleanupService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(SD.TempFileCleanupIntervalMinutes);

        public TempFileCleanupService(
            ILogger<TempFileCleanupService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Temporary file cleanup service is running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var webHostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                        string tempDirectory = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "Temp");

                        if (Directory.Exists(tempDirectory))
                        {
                            _logger.LogInformation("Cleaning up temporary files in {TempDirectory}", tempDirectory);

                            var tempFiles = Directory.GetFiles(tempDirectory);
                            int deletedFilesCount = 0;

                            foreach (var file in tempFiles)
                            {
                                var fileInfo = new FileInfo(file);
                                
                                // Delete files older than configured max age
                                if ((DateTime.Now - fileInfo.CreationTime).TotalMinutes > SD.TempFileMaxAgeMinutes)
                                {
                                    try
                                    {
                                        fileInfo.Delete();
                                        deletedFilesCount++;
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Error deleting temporary file {FileName}", fileInfo.Name);
                                    }
                                }
                            }

                            _logger.LogInformation("Temporary file cleanup completed. Deleted {DeletedFilesCount} files", deletedFilesCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during temporary file cleanup");
                }

                // Wait for the next cleanup cycle
                await Task.Delay(_cleanupInterval, stoppingToken);
            }
        }
    }
} 