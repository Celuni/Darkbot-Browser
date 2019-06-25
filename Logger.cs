﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace DarkBotBrowser
{
    public class Logger
    {
        private static ILog _log;

        public static ILog GetLogger()
        {
            if (_log == null)
            {
                ConfigureFileAppender(Path.Combine(Program.PATH_RESOURCES, "debug.log"));
                _log = LogManager.GetLogger("FileLogger");
            }
            return _log;
        }

        public static void ConfigureFileAppender(string logFile)
        {
            var fileAppender = GetFileAppender(logFile);
            BasicConfigurator.Configure(fileAppender);
            ((Hierarchy)LogManager.GetRepository()).Root.Level = Level.All;
        }

        private static IAppender GetFileAppender(string logFile)
        {
            var layout = new PatternLayout("%date{dd.mm.yyyy HH:mm:ss} [%level] - %message%newline");
            layout.ActivateOptions();

            var appender = new FileAppender
            {
                Name = "FileLogger",
                File = logFile,
                AppendToFile = true,
                Encoding = Encoding.UTF8,
                Threshold = Level.All,
                Layout = layout,
            };

            appender.ActivateOptions();

            return appender;
        }
    }
}
