using OpenProfiler.GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.Service
{
    public interface IFormatService
    {
        QueryListItem Transform(string dataStr);
    }
}
