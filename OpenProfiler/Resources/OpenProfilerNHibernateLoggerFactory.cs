using NHibernate;

namespace OpenProfiler
{
    public class OpenProfilerNHibernateLoggerFactory : INHibernateLoggerFactory
    {
        public INHibernateLogger LoggerFor(string keyName)
        {
            foreach (var name in LoggerNamesConstants.Names)
            {
                if (name == keyName)
                {
                    return new OpenProfilerNHibernateLogger();
                }
            }

            return new OpenProfilerNoLogNHibernateLogger();
        }
        
        public INHibernateLogger LoggerFor(System.Type type)
        {
            return new OpenProfilerNoLogNHibernateLogger();
        }
    }
}
