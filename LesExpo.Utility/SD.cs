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

        // Video Uploads
        public const int MaxVideoSizeInMB = 200; // 200MB for videos
        public const int MaxVideoSizeInBytes = MaxVideoSizeInMB * 1024 * 1024;
        public static readonly string[] AllowedVideoExtensions = { ".mp4", ".webm", ".avi", ".mov", ".wmv", ".flv", ".mkv" };

        // Temp File Cleanup
        public const int TempFileCleanupIntervalMinutes =  1440; // 24 hours
        public const int TempFileMaxAgeMinutes = 120; // 2 hours

        public const string siteUrl = "https://les-expo.com";

        public const string AdminEmail = "lesexpo@kfa.com.tr";
        public const string AdminEmailName = "LES-EXPO İletişim";

        public const int FUAR_ID = 4139;
    }
}
