using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.Utility
{
    public static class SD
    {
        // Roles
        public const string Role_Admin = "Admin";
        public const string Role_Editor = "Editor";

        // File Uploads
        public const int MaxImageSizeInMB = 5;
        public const int MaxImageSizeInBytes = MaxImageSizeInMB * 1024 * 1024;
        public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Temp File Cleanup
        public const int TempFileCleanupIntervalMinutes =  1440; // 24 hours
        public const int TempFileMaxAgeMinutes = 120; // 2 hours
    }
}
