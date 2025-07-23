using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenProfiler.Tests.Common
{
    public class TestSetup
    {
        public ISessionFactory BuildSessionFactory()
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
            return configuration.BuildSessionFactory();
        }
    }
}
