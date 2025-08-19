using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using OpenProfiler.Tests.Common;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace OpenProfiler.NHibernate.NoLog4NetSampleConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sessionFactory = new TestSetup().BuildSessionFactory();

            OpenProfilerInfrastructure.Initialize();

            using (var session = sessionFactory.OpenSession())
            {
                var result = session.QueryOver<Customer>().Where(x => x.Name.IsLike("n")).List();
                session.Save(new Customer()
                {
                    Name = "test",
                    Birthday = new DateTime(2000, 1, 1)
                });

                session.Flush();

                var f1 = session.Query<Customer>().Where(x => x.Id == 1).ToFuture();
                var f2 = session.Query<Customer>().Where(x => x.HP == 100).ToFuture();
                var x = f1.ToList();
            }

            Console.ReadLine();
        }
    }
}
