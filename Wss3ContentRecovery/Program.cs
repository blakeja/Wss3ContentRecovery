using System;
using System.Data.SqlClient;
using NLog;
using Wss3ContentRecovery.Exceptions;
using Wss3ContentRecovery.Logging;
using Wss3ContentRecovery.Models;
using Wss3ContentRecovery.Recovery;

namespace Wss3ContentRecovery
{
    class Program
    {
        #region Fields

        private static string[] _args;

        private static string _logPath;
        private static string _logName;

        private static SettingsModel _settings;
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static string _connectionString;

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine();

            _args = args;

            InitializeNLog();
        }

        private static void InitializeNLog()
        {
            _logName = $"log_{DateTime.Now.Ticks}.log";
            _logPath = $"./logs/{_logName}";

            var config = new NLogConfiguration(_logPath);
            config.Configure();

            ProcessArgs();
        }

        private static void ProcessArgs()
        {
            if (_args.Length == 0)
            {
                Logger.Warn("No arguments specified, displaying help");
                HelpText.Display();
                return;
            }

            try
            {
                var arguments = new Arguments(_args);
                _settings = arguments.Process();
            }
            catch (InvalidArgumentException e)
            {
                Logger.Error(e.Message);
                HelpText.Display();
                return;
            }

            GetUserConsent();
        }

        private static void GetUserConsent()
        {
            Logger.Info("Press enter to continue...");
            Console.ReadLine();

            SetupConnectionString();
        }

        private static void SetupConnectionString()
        {
            _connectionString = "Server=[Server];Database=[Database];Trusted_Connection=True;Connection Timeout=[ConnectionTimeout]";
            _connectionString = _connectionString.Replace("[Server]", _settings.Server);
            _connectionString = _connectionString.Replace("[Database]", _settings.Database);
            _connectionString = _connectionString.Replace("[ConnectionTimeout]", _settings.ConnectionTimeout.ToString());

            Logger.Info("Using SQL connection string: " + _connectionString);

            RecoverFilesFromDatabase();
        }

        private static void RecoverFilesFromDatabase()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                Logger.Info("Opened database connection, SQL connection timeout set to " + sqlConnection.ConnectionTimeout);

                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = _settings.Query;
                    sqlCommand.CommandTimeout = _settings.CommandTimeout;

                    Logger.Info("Using SQL query: " + sqlCommand.CommandText);
                    Logger.Info("SQL command timeout set to " + sqlCommand.CommandTimeout);

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dirName = Convert.ToString(reader["DirName"]);
                            var leafName = Convert.ToString(reader["LeafName"]);

                            if (dirName == "")
                            {
                                Logger.Warn("Found empty dirName in database, skipping");
                                continue;
                            }

                            var fileWriter = new FileWriter(dirName, leafName, reader, _settings);
                            fileWriter.Write();
                        }
                    }
                }

                sqlConnection.Close();
            }
        }
    }
}