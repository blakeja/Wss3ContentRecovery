using System;
using NLog;

namespace Wss3ContentRecovery.Recovery
{
    public static class PathFormatter
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public static string GetSafePath(string dirName, string leafName)
        {
            string path;

            if (dirName.Length + leafName.Length >= 240)
            {
                var longPath = dirName + leafName;
                var shortLeafName = leafName.Substring(0, 6) + Guid.NewGuid().ToString().Substring(0, 6);
                path = dirName + shortLeafName;

                Logger.Warn("Path too long: " + longPath);
                Logger.Info("Using path with shortened leaf name: " + path);
            }
            else
            {
                path = dirName + leafName;
            }
            
            return path;
        }
    }
}
