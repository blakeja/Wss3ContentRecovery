using NLog;

namespace Wss3ContentRecovery.Recovery
{
    public static class DirectoryCreator
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public static void Create(string directory)
        {
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);

                Logger.Info("Created directory: " + directory);
            }
        }
    }
}
