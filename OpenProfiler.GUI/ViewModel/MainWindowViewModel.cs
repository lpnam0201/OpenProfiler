using OpenProfiler.GUI.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.ViewModel
{
    public class MainWindowViewModel
    {
        public ObservableCollection<QueryListItem> QueryListItems = new ObservableCollection<QueryListItem>();
        public QueryDetail QueryDetail { get; set; }

        private readonly IDataCollectingService dataCollectingService;

        public MainWindowViewModel(IDataCollectingService dataCollectingService)
        {
            this.dataCollectingService = dataCollectingService;
        }
    }
}
