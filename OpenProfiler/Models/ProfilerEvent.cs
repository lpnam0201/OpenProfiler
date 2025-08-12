using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.Models
{
    public class ProfilerEvent
    {
        public Guid Guid { get; set; }
        public DateTime Timestamp { get; set; }
        public string LoggerName { get; set; }
        public string Message { get; set; }
    }
}
