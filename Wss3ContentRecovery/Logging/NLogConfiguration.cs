using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Time;

namespace Wss3ContentRecovery.Logging
{
    public class NLogConfiguration
    {
        private readonly string _logName;
        private LoggingConfiguration _configuration;

        public NLogConfiguration(string logName)
        {
            _logName = logName;
        }

        public void Configure()
        {
            TimeSource.Current = new AccurateLocalTimeSource();
            _configuration = new LoggingConfiguration();

            SetupConsoleTarget();
        }

        private void SetupConsoleTarget()
        {
            var consoleTarget = new ColoredConsoleTarget();
            consoleTarget.Layout = Layout.FromString("${time}:${level}: ${message}");
            _configuration.AddTarget("console", consoleTarget);

            var rule = new LoggingRule("*", LogLevel.Info, consoleTarget);
            _configuration.LoggingRules.Add(rule);

            SetupLogFileTarget();
        }

        private void SetupLogFileTarget()
        {
            var csvLayout = new CsvLayout();
            csvLayout.Quoting = CsvQuotingMode.Nothing;
            csvLayout.Columns.Add(new CsvColumn("time", "${longdate}"));
            csvLayout.Columns.Add(new CsvColumn("level", "${level}"));
            csvLayout.Columns.Add(new CsvColumn("message", "${message}"));
            csvLayout.Columns.Add(new CsvColumn("exception", "${exception:format=ToString}"));

            var fileTarget = new FileTarget();
            fileTarget.FileName = _logName;
            fileTarget.CreateDirs = true;
            fileTarget.ConcurrentWrites = false;
            fileTarget.ArchiveAboveSize = 104857600;
            fileTarget.KeepFileOpen = true;
            fileTarget.Layout = csvLayout;

            _configuration.AddTarget("file", fileTarget);

            var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
            rule.Final = true;

            _configuration.LoggingRules.Add(rule);

            FinalizeConfiguration();
        }

        private void FinalizeConfiguration()
        {
            LogManager.Configuration = _configuration;
        }
    }
}
