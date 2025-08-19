using System;
using NHibernate;

namespace OpenProfiler
{
    public class OpenProfilerNHibernateLogger : INHibernateLogger
    {
        public void Log(NHibernateLogLevel logLevel, NHibernateLogValues state, Exception exception)
        {
            var timeStamp = DateTime.Now;
            OpenProfilerInfrastructure.PublishEvent(
                "",
                timeStamp,
                state.ToString());
        }

        public bool IsEnabled(NHibernateLogLevel logLevel)
        {
            if (logLevel == NHibernateLogLevel.None) return true;

            return false;
        }
    }
}
