using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using OpenProfiler.Compiling;
using OpenProfiler.Messaging;
using OpenProfiler.Models;
using System.Reflection;

namespace OpenProfiler
{
    public static class OpenProfilerInfrastructure
    {
        private static bool _isInitialized;
        private static Thread _flushMessagesBackgroundThread;
        private static bool _isProfilerRunning;
        private static List<ProfilerEvent> _profilerEvents = new List<ProfilerEvent>();
        private static Options _options;
        private static UdpMessageDispatcher _udpMessageDispatcher;

        public static void Initialize(Options options = null)
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException("Profiler is already initialized");
            }

            _options = options ?? new Options
            {
                Port = 29817,
                Host = "localhost"
            };
            _udpMessageDispatcher = new UdpMessageDispatcher(_options);
            _isProfilerRunning = true;
            _flushMessagesBackgroundThread = new Thread(SendQueuedMessages)
            {
                IsBackground = true
            };
            _flushMessagesBackgroundThread.Start();
            AttachAppenderToNHibernate();

            _isInitialized = true;
        }

        private static void AttachAppenderToNHibernate()
        {
            var log4netAssembly = GetAssemblyByName("log4net");

            if (log4netAssembly != null)
            {
                CallInitializeWithLog4Net(log4netAssembly);
            }
            else
            {
                CallInitializeWithoutLog4Net();
            }
            
        }

        private static void CallInitializeWithLog4Net(Assembly log4netAssembly)
        {
            var assemblyPaths = new List<string>()
            {
                log4netAssembly.Location
            };

            var sourceFiles = new List<string>()
            {
                "OpenProfiler.Resources.Log4NetInitializer.cs",
                "OpenProfiler.Resources.OpenProfilerAppender.cs",
                "OpenProfiler.Resources.LoggerNamesConstants.cs"
            };
            var sourceTexts = GetSourceTexts(sourceFiles);

            var assembly = CompileHelper.CompileAssembly("Temp", sourceTexts, assemblyPaths);
            var loggerInitializerType = assembly.GetType("OpenProfiler.Log4NetInitializer");
            loggerInitializerType.GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
        }

        private static void CallInitializeWithoutLog4Net()
        {
            var nhibernateAssembly = GetAssemblyByName("NHibernate");
            var assemblyPaths = new List<string>()
            {
                nhibernateAssembly.Location
            };
            var sourceFiles = new List<string>()
            {
                "OpenProfiler.Resources.NoLog4NetInitializer.cs",
                "OpenProfiler.Resources.OpenProfilerNHibernateLogger.cs",
                "OpenProfiler.Resources.OpenProfilerNHibernateLoggerFactory.cs",
                "OpenProfiler.Resources.OpenProfilerNoLogNHibernateLogger.cs",
                "OpenProfiler.Resources.LoggerNamesConstants.cs"
            };
            var sourceTexts = GetSourceTexts(sourceFiles);

            var assembly = CompileHelper.CompileAssembly("Temp", sourceTexts, assemblyPaths);
            var loggerInitializerType = assembly.GetType("OpenProfiler.NoLog4NetInitializer");
            loggerInitializerType.GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
        }

        private static List<string> GetSourceTexts(List<string> sourceFiles)
        {
            var result = new List<string>();

            var assembly = Assembly.GetExecutingAssembly();
            foreach (var sourceFile in sourceFiles)
            {
                using (var stream = assembly.GetManifestResourceStream(sourceFile))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        var sourceText = streamReader.ReadToEnd();
                        result.Add(sourceText);
                    }
                }
            }

            return result;
        }

        private static Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => assembly.GetName().Name == name);
        }

        public static void PublishEvent(string loggerName, DateTime timestamp, string message)
        {
            if (!_isInitialized)
            {
                return;
            }

            _profilerEvents.Add(new ProfilerEvent()
            {
                Guid = Guid.NewGuid(),
                LoggerName = loggerName,
                Timestamp = timestamp,
                Message = message
            });
        }

        private static void SendQueuedMessages()
        {
            while (_isProfilerRunning)
            {
                var copiedEvents = _profilerEvents.ToList();
                _profilerEvents.Clear();

                if (copiedEvents.Count > 0)
                {
                    _udpMessageDispatcher.Dispatch(copiedEvents);
                }

                Thread.Sleep(200);
            }
        }
    }
}
