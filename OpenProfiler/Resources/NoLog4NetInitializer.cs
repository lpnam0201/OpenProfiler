using NHibernate;

namespace OpenProfiler
{
    public static class NoLog4NetInitializer
    {
        public static void Initialize()
        {
            SetProfilerLoggerFactory();
        }

        private static void AttachToUserDefinedLoggerFactory()
        {

        }

        private static void SetProfilerLoggerFactory()
        {
            NHibernateLogger.SetLoggersFactory(
                new OpenProfilerNHibernateLoggerFactory());
        }
    }
}
