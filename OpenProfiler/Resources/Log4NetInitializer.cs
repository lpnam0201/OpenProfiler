using log4net;
using log4net.Repository.Hierarchy;

namespace OpenProfiler
{
    public static class Log4NetInitializer
    {
        private static string[] loggerNames = new[] 
        {
            //"NHibernate.Connection.DriverConnectionProvider",
            //"NHibernate.Loader.Entity.AbstractEntityLoader",
            //"NHibernate.Id.IdentifierGeneratorFactory",
            "NHibernate.SQL"
            //"NHibernate.Cfg.Configuration",
            //"NHibernate.Type.TypeFactory",
            //"NHibernate.SqlCommand.SqlSelectBuilder",
            //"NHibernate.Impl.SessionFactoryObjectFactory",
            //"NHibernate.Connection.ConnectionProvider",
            //"NHibernate.Cfg.XmlHbmBinding.Binder",
            //"NHibernate.Engine.CascadingAction",
            //"NHibernate.Tuple.Entity.PocoEntityTuplizer",
            //"NHibernate.Cfg.Environment",
            //"NHibernate.Cfg.SettingsFactory",
            //"NHibernate.Type.DateType",
            //"NHibernate.Exceptions.SQLExceptionConverterFactory",
            //"NHibernate.Tuple.Entity.AbstractEntityTuplizer",
            //"NHibernate.SqlCommand.SqlUpdateBuilder",
            //"NHibernate.SqlCommand.SqlInsertBuilder",
            //"NHibernate.Impl.SessionFactoryImpl",
            //"NHibernate.SqlCommand.SqlDeleteBuilder",
            //"NHibernate.Util.ReflectHelper",
            //"NHibernate.Persister.Entity.AbstractEntityPersister",
            //"NHibernate.Dialect.Dialect",
            //"NHibernate.Type"
        };

        public static void Initialize()
        {
            var appender = new OpenProfilerAppender()
            {
                Name = nameof(OpenProfilerAppender)
            };
            var hierachy = LogManager.GetRepository() as Hierarchy;
            foreach (var loggerName in loggerNames)
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
