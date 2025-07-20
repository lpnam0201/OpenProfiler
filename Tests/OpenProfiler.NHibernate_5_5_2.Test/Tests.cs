using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using OpenProfiler.Tests.Common;

namespace OpenProfiler.NHibernate_5_5_2.Test
{
    [TestFixture]
    public class Tests
    {
        private ISessionFactory _sessionFactory;
        private UdpListener _udpListener;

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

            _udpListener = new UdpListener("127.0.0.1", 29817);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
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
        }
    }
}