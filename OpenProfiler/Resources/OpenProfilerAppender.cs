using log4net.Appender;
using log4net.Core;

namespace OpenProfiler
{
    public class OpenProfilerAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            OpenProfilerInfrastructure.PublishEvent(
                loggingEvent.LoggerName,
                loggingEvent.TimeStamp,
                loggingEvent.RenderedMessage);
        }
    }
}
