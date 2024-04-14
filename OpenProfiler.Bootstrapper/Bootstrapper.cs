namespace OpenProfiler.Bootstrapper
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using OpenProfiler.Bootstrapper.log4netProxies;

    public class Bootstrapper
    {
        private const string LocalhostAddress = "127.0.0.1";
        private const string NHibernateDllName = "NHibernate.dll";
        private const string Log4NetDllName = "log4net.dll";
        private const string NHibernateSQLLoggerName = "NHibernate.SQL";

        private const int RemotePort = 329;

        private static readonly IPAddress RemoteAddress = IPAddress.Parse(LocalhostAddress);

        private static Assembly nHibernateAssembly = FindAssembly(NHibernateDllName);
        private static Assembly log4netAssembly = FindAssembly(Log4NetDllName);        

        public static void Initialize()
        {

            if (nHibernateAssembly != null && log4netAssembly != null)
            {
                var appenderBuilder = new AppenderBuilder(nHibernateAssembly, log4netAssembly);
                var openProfilerAppender = appenderBuilder.BuildAppender();

                Loader.Initialize(log4netAssembly);

                Hierarchy hierarchy = LogManager.GetRepository();
                Logger logger = hierarchy.GetLogger(NHibernateSQLLoggerName);

                UdpAppender appender = new UdpAppender(openProfilerAppender);
                appender.Encoding = Encoding.UTF8;
                appender.RemoteAddress = RemoteAddress;
                appender.RemotePort = RemotePort;

                appender.Layout = new XmlLayout();
                appender.ActivateOptions();

                logger.Level = Level.DebugLevel();
                logger.AddAppender(appender);

                hierarchy.Configured = true;
                hierarchy.RaiseConfigurationChanged(EventArgs.Empty);
            }
        }

        private static Assembly FindAssembly(string dllName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
            string binPath = relativeSearchPath == null ? baseDirectory : Path.Combine(baseDirectory, relativeSearchPath);
            string path = binPath == null ? dllName : Path.Combine(binPath, dllName);

            return File.Exists(path) ? Assembly.LoadFrom(path) : null;
        }
    }
}
