using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

namespace OpenProfiler.NHibernate.Test
{
    [TestFixture]
    public class Tests
    {
        private ISessionFactory _sessionFactory;
        private UdpClient _udpListener;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private const string UdpOutputFileName = "udplistened.txt";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.ConnectionString = "Data Source=NB34448;Initial Catalog=OpenProfilerSampleDatabase;Trusted_Connection=true";
            });
            var conventionModelMapper = new ConventionModelMapper();
            conventionModelMapper.AddMapping<CustomerMap>();
            configuration.AddMapping(
                conventionModelMapper.CompileMappingForAllExplicitlyAddedEntities());
            _sessionFactory = configuration.BuildSessionFactory();

            if (File.Exists(UdpOutputFileName))
            {
                File.Delete(UdpOutputFileName);
            }
            _udpListener = new UdpClient(29817);
            SpawnUdpListener();
        }

        private void SpawnUdpListener()
        {
            var thread = new Thread(async (obj) =>
            {
                var cancellationToken = (CancellationToken)obj;
                while (true)
                {
                    var ipEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 29817);
                    try
                    {
                        var bytes = (await _udpListener.ReceiveAsync(cancellationToken)).Buffer;

                        var txt = Encoding.UTF8.GetString(bytes);
                        File.WriteAllText(UdpOutputFileName, $"{txt}{System.Environment.NewLine}");
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start(_cancellationTokenSource.Token);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _cancellationTokenSource.Cancel();
            _udpListener.Close();
        }

        [Test]
        public void TryMakingNHibernateCalls__WithProfilerInitialized__ShouldWorkCorrectly()
        {
            OpenProfilerInfrastructure.Initialize();

            using (var session = _sessionFactory.OpenSession())
            {
                var result = session.QueryOver<Customer>().List();
            }

            WaitForOutputFileAvailable();
        }

        private void WaitForOutputFileAvailable()
        {
            int waitCount = 0;
            do
            {
                if (File.Exists(UdpOutputFileName))
                {
                    break;
                }
                Thread.Sleep(200);
            } while (waitCount < 10);
        }
    }
}