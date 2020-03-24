using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.NewRelic;

namespace Serilog
{
    /// <summary>
    /// Extends <see cref="LoggerConfiguration" /> with methods to write events to NewRelic.
    /// </summary>
    public static class NewRelicLoggerConfigurationExtensions
    {
        /// <summary>
        /// Write log events to NewRelic.
        /// </summary>
        /// <param name="loggerSinkConfiguration">The logger configuration.</param>
        /// <param name="applicationName">Application name in NewRelic.</param>
        /// <param name="customEventName">
        /// The name of a custom event name emitted by logging events Warning, Information, Debug, Verbose. Defaults to "Serilog".
        /// </param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="batchPostingLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <returns>The provided logger sink configuration after being configured with the NewRelic sink.</returns>
        public static LoggerConfiguration NewRelic(this LoggerSinkConfiguration loggerSinkConfiguration,
            string applicationName,
            string customEventName = "Serilog",
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchPostingLimit = NewRelicSink.DefaultBatchPostingLimit,
            TimeSpan? period = null)
        {
            if (loggerSinkConfiguration == null)
                throw new ArgumentNullException(nameof(loggerSinkConfiguration));

            if (string.IsNullOrEmpty(applicationName))
                throw new ArgumentException("Must supply the application name", nameof(applicationName));

            var defaultedPeriod = period ?? NewRelicSink.DefaultPeriod;

            ILogEventSink sink = new NewRelicSink(applicationName, batchPostingLimit, defaultedPeriod, customEventName);

            return loggerSinkConfiguration.Sink(sink, restrictedToMinimumLevel);
        }
    }
}