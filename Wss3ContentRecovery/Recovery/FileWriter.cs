using System;
using System.Data.SqlClient;
using System.IO;
using NLog;
using Wss3ContentRecovery.Models;

namespace Wss3ContentRecovery.Recovery
{
    public class FileWriter
    {
        private readonly string _hostHeader;
        private readonly string _dirName;
        private readonly string _leafName;
        private readonly SqlDataReader _reader;
        private readonly SettingsModel _settings;

        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string _filepath;
        private string _directory;

        #endregion

        #region Constructor

        public FileWriter(string hostHeader, string dirName, string leafName, SqlDataReader reader, SettingsModel settings)
        {
            _hostHeader = hostHeader;
            _dirName = dirName;
            _leafName = leafName;
            _reader = reader;
            _settings = settings;
        }

        #endregion

        public void Write()
        {
            CreateDirectory();
        }

        private void CreateDirectory()
        {
            _directory = _hostHeader + "\\" + _dirName;

            if (_settings.WhatIf)
            {
                Logger.Info("Skipping " + _directory + " directory creation due to -whatif");
            }
            else
            {
                DirectoryCreator.Create(_directory);
            }

            GetSafePath();
        }

        private void GetSafePath()
        {
            try
            {
                _filepath = PathFormatter.GetSafePath(_directory, _leafName);
            }
            catch (PathTooLongException e)
            {
                if (_settings.WhatIf)
                {
                    Logger.Warn(e.Message);
                }
                else
                {
                    throw;
                }
            }

            WriteFile();
        }

        private void WriteFile()
        {
            if (_settings.WhatIf)
            {
                Logger.Info("Skipping " + _filepath + " file write due to -whatif");
            }
            else
            {
                Logger.Info("Writing file " + _filepath);

                using (var fileStream = new FileStream(_filepath, FileMode.Create, FileAccess.Write))
                {
                    using (var writer = new BinaryWriter(fileStream))
                    {
                        long startIndex = 0;
                        long bytes;
                        var outBytes = new byte[_settings.BufferSize];

                        do
                        {
                            bytes = _reader.GetBytes(4, startIndex, outBytes, 0, _settings.BufferSize);
                            startIndex += _settings.BufferSize;

                            writer.Write(outBytes, 0, (int)bytes);
                            writer.Flush();
                        } while (bytes == _settings.BufferSize);
                    }
                }

                Logger.Info("Finished writing file " + _filepath);
            }
        }
    }
}
