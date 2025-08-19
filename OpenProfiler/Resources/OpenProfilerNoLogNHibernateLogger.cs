using System;
using NHibernate;

namespace OpenProfiler
{
    public class OpenProfilerNoLogNHibernateLogger : INHibernateLogger
    {
        public void Log(NHibernateLogLevel logLevel, NHibernateLogValues state, Exception exception)
        {
            // do nothing
        }

        public bool IsEnabled(NHibernateLogLevel logLevel)
        {
            if (logLevel == NHibernateLogLevel.None) return true;

            return false;
        }
    }
}
