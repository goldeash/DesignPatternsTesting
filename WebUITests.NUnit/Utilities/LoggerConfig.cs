using Serilog;
using Serilog.Events;
using System;

namespace WebUITests.NUnit.Utilities
{
    public static class LoggerConfig
    {
        public static ILogger ConfigureLogger(string testName)
        {
            var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("TestName", testName)
                .Enrich.WithProperty("TestRunId", Guid.NewGuid())
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{TestName}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    path: $"logs/{DateTime.Now:yyyyMMdd}/test-.log",
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{TestName}] [{TestRunId}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    shared: true)
                .CreateLogger();

            return log;
        }
    }
}