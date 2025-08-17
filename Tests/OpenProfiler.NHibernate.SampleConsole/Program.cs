using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using OpenProfiler.Tests.Common;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace OpenProfiler.NHibernate.SampleConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var connString = "Data Source=NB34448;Initial Catalog=OpenProfilerSampleDatabase;Trusted_Connection=true";
            //using (SqlConnection conn = new SqlConnection(connString))
            //using (SqlCommand cmd = new SqlCommand("SELECT Id FROM Customer WHERE Id IN (SELECT value FROM @Id)", conn))
            //{
            //    var dataTable = new DataTable();
            //    dataTable.Columns.Add();
            //    for (int i = 0; i <= 3000; i++)
            //    {
            //        dataTable.Rows.Add(i + 1);
            //    }
            //    var param = cmd.Parameters.AddWithValue("Id", dataTable);
            //    param.SqlDbType = SqlDbType.Structured;
            //    param.TypeName = "dbo.ListOfStrings";

            //    conn.Open();
            //    using (SqlDataReader reader = cmd.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            Console.WriteLine(reader["Id"]);
            //        }
            //    }
            //}

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
