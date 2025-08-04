using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using OpenProfiler.Tests.Common;

namespace OpenProfiler.NHibernate.SampleConsole
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

                var f1 = session.Query<Customer>().Where(x => x.Name == "1").ToFuture();
                var f2 = session.Query<Customer>().Where(x => x.Name == "2").ToFuture();
                var x = f1.ToList();
            }

            Console.ReadLine();
        }
    }
}
