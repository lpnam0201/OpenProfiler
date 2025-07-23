using NHibernate;
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
                var result = session.QueryOver<Customer>().List();
            }

            Console.ReadLine();
        }
    }
}
