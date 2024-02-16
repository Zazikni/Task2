﻿using Serilog;
using Serilog.Events;
using System.Configuration;
using System;


namespace AvaloniaUI.Models.Logging
{/// <summary>
/// Инициализирует базовый Logger из библиотеки Serilog.
/// </summary>
    internal class LoggerInit
    {
        public LoggerInit()
        {
            int logFileSize = Convert.ToInt32(ConfigurationManager.AppSettings["logMaxSizeInBytes"]);
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.File(path: "logs/debug-.log", restrictedToMinimumLevel: LogEventLevel.Debug, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: logFileSize)
                        .WriteTo.File(path: "logs/info-.log", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: logFileSize)
                        .WriteTo.File(path: "logs/warn-.log", restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: logFileSize)
                        .WriteTo.File(path: "logs/error-.log", restrictedToMinimumLevel: LogEventLevel.Error, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: logFileSize)
                        .WriteTo.Console()
                        .CreateLogger();
        }
    }
}
