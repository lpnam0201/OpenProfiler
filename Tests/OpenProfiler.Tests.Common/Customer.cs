using System;

namespace OpenProfiler.Tests.Common
{
    public class Customer
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual long HP { get; set; }
    }
}
