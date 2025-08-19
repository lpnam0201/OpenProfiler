using log4net;
using log4net.Repository.Hierarchy;

namespace OpenProfiler
{
    public static class Log4NetInitializer
    {
        public static void Initialize()
        {
            var appender = new OpenProfilerAppender()
            {
                Name = nameof(OpenProfilerAppender)
            };
            var hierachy = LogManager.GetRepository() as Hierarchy;
            foreach (var loggerName in LoggerNamesConstants.Names)
            {
                var logger = hierachy.GetLogger(loggerName) as Logger;
                logger.Level = log4net.Core.Level.Debug;
                var existingAppender = logger.GetAppender(nameof(OpenProfilerAppender));
                if (existingAppender == null)
                {
                    logger.AddAppender(appender);
                }
            }
            hierachy.Configured = true;
        }
    }
}
