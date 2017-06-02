using System;
using System.IO;
using NLog;

namespace Wss3ContentRecovery.Recovery
{
    public static class PathFormatter
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public static string GetSafePath(string directory, string leafName)
        {
            if (!directory.EndsWith("/") && !directory.EndsWith("\\"))
            {
                directory = directory + "\\";
            }

            var path = directory + leafName;

            if (path.Length >= 240)
            {
                Logger.Warn("Path too long: " + path);

                var shortLeafName = leafName.Substring(0, 6) + "-" + Guid.NewGuid().ToString().Substring(0, 6);
                path = directory + shortLeafName;

                Logger.Info("Using path with shortened leaf name: " + path);
            }
            else
            {
                Logger.Info("Using path: " + path);
            }

            if (path.Length >= 240)
            {
                throw new PathTooLongException("Path has length greater than 240 characters: '" + path);
            }

            return path;
        }
    }
}
