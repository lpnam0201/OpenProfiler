using OpenProfiler.GUI.Model;
using OpenProfiler.GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.Service
{
    public class FormatService : IFormatService
    {
        public List<QueryListItem> Transform(string dataStr)
        {
            var dataItems = JsonSerializer.Deserialize<List<DataItem>>(dataStr);
            return dataItems.Select(x => new QueryListItem()
            {
                Text = x.Message
            }).ToList();
        }
    }
}
