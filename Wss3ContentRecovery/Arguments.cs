using NLog;
using Wss3ContentRecovery.Exceptions;
using Wss3ContentRecovery.Models;

namespace Wss3ContentRecovery
{
    public class Arguments
    {
        #region Fields

        private readonly string[] _args;
        private readonly SettingsModel _settings = new SettingsModel();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const int DefaultBufferSize = 1000000;
        private const int DefaultCommandTimeout = 60;
        private const int DefaultConnectionTimeout = 60;

        #endregion

        #region Constructor

        public Arguments(string[] args)
        {
            _args = args;
        }

        #endregion

        public SettingsModel Process()
        {
            CheckNumberOfArguments();
            
            return _settings;
        }

        private void CheckNumberOfArguments()
        {
            if (_args.Length < 2)
            {
                throw new InvalidArgumentException("Not enough arguments");
            }

            GetServer();
        }

        private void GetServer()
        {
            var value = GetArgument("-server", true);
            _settings.Server = value.ToString();

            Logger.Info("Server set to: " + _settings.Server);

            GetDatabase();
        }

        private void GetDatabase()
        {
            var value = GetArgument("-database", true);
            _settings.Database = value.ToString();

            Logger.Info("Database set to: " + _settings.Database);

            GetCommandTimeout();
        }

        private void GetCommandTimeout()
        {
            var value = GetArgument("-commandtimeout");

            if (value == null)
            {
                _settings.CommandTimeout = DefaultCommandTimeout;
            }
            else
            {
                int timeout;
                var success = int.TryParse(value.ToString(), out timeout);

                if (success)
                {
                    _settings.CommandTimeout = timeout;
                }
                else
                {
                    throw new InvalidArgumentException("Invalid value '" + value + "' supplied for argument -commandtimeout");
                }
            }

            Logger.Info("SQL command timeout set to: " + _settings.CommandTimeout);

            GetConnectionTimeout();
        }

        private void GetConnectionTimeout()
        {
            var value = GetArgument("-connectiontimeout");

            if (value == null)
            {
                _settings.ConnectionTimeout = DefaultConnectionTimeout;
            }
            else
            {
                int timeout;
                var success = int.TryParse(value.ToString(), out timeout);

                if (success)
                {
                    _settings.ConnectionTimeout = timeout;
                }
                else
                {
                    throw new InvalidArgumentException("Invalid value '" + value + "' supplied for argument -connectiontimeout");
                }
            }

            Logger.Info("SQL connection timeout set to: " + _settings.ConnectionTimeout);
            
            GetBufferSize();
        }

        private void GetBufferSize()
        {
            var value = GetArgument("-buffersize");

            if (value == null)
            {
                _settings.BufferSize = DefaultBufferSize;
            }
            else
            {
                int size;
                var success = int.TryParse(value.ToString(), out size);

                if (success)
                {
                    _settings.BufferSize = size;
                }
                else
                {
                    throw new InvalidArgumentException("Invalid value '" + value + "' supplied for argument -buffersize");
                }
            }

            Logger.Info("Buffer size set to: " + _settings.BufferSize);

            GetWhatIf();
        }

        private void GetWhatIf()
        {
            foreach (var arg in _args)
            {
                if (arg.ToLower() == "-whatif")
                {
                    _settings.WhatIf = true;
                    Logger.Warn("Detected -whatif, this will be a dry run, no files will be written");
                }
            }
        }

        private object GetArgument(string argument, bool required = false)
        {
            object value = null;
            var i = 0;

            foreach (var arg in _args)
            {
                if (arg.ToLower() == argument)
                {
                    var index = i + 1;
                    value = _args[index];

                    if (value.ToString().StartsWith("-"))
                    {
                        throw new InvalidArgumentException("Incorrect value '" + value + "' specified for " + argument);
                    }

                    break;
                }

                i++;
            }

            if (value == null && required)
            {
                throw new InvalidArgumentException("Required argument '" + argument + "' is missing");
            }

            return value;
        }

    }
}
