using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace OpenProfiler.NHibernate.Test
{
    public class CustomerMap : ClassMapping<Customer>
    {
        public CustomerMap()
        {
            Table("Customer");

            Id(x => x.Id, m => {
                m.Generator(Generators.Identity);
            });

            Property(x => x.Name, m => {
                m.Length(100);
                m.NotNullable(true);
            });

            Property(x => x.Birthday, m => {
                m.NotNullable(true);
            });
        }
    }
}
