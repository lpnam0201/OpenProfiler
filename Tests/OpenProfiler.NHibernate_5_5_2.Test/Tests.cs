using log4net;
using log4net.Repository.Hierarchy;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;

namespace OpenProfiler.NHibernate_5_5_2.Test
{
    public class Tests
    {
        private ISessionFactory _sessionFactory;

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
        }

        [Test]
        public void TryMakingNHibernateCalls__WithProfilerInitialized__ShouldWorkCorrectly()
        {
            OpenProfilerInfrastructure.Initialize();
            var repo = LogManager.GetRepository() as Hierarchy;
            
            var logger = NHibernateLogger.For("NHibernate.SQL");

            using (var session = _sessionFactory.OpenSession())
            {
                var result = session.QueryOver<Customer>().List();
            }
        }
    }
}