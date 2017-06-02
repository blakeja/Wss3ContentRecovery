using NLog;

namespace Wss3ContentRecovery.Recovery
{
    public static class DirectoryCreator
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public static void Create(string dirName)
        {
            if (!System.IO.Directory.Exists(dirName))
            {
                System.IO.Directory.CreateDirectory(dirName);

                Logger.Info("Created directory: " + dirName);
            }
        }
    }
}
