using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Helper
{
    /// <summary>
    /// ProgressUpdate
    /// </summary>
    public static class ProgressUpdate
    {
        public static bool IsCurrentlyProgressed { get; set; }
        public static string CurrentStatus { get; set; }

        public static string GetCurrentStatus()
        {
            // Get current status
            string nowStatus = CurrentStatus;
            CurrentStatus = string.Empty;

            return nowStatus;
        }

        public static void SetCurrentStatus(string status)
        {
            // Set current status
            CurrentStatus += Environment.NewLine + status;
        }

    }
}