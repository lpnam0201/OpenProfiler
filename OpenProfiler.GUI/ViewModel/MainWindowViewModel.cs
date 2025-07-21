using OpenProfiler.GUI.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<QueryListItem> QueryListItems = new ObservableCollection<QueryListItem>();
        private QueryDetail _queryDetail;
        public QueryDetail QueryDetail
        {
            get { return _queryDetail;  }
            set { SetProperty(ref _queryDetail, value); }
        }

        private readonly IDataReceivingService _dataCollectingService;
        private readonly IBufferService _bufferService;
        private readonly IFormatService _formatService;

        public MainWindowViewModel(IDataReceivingService dataCollectingService,
            IBufferService bufferService,
            IFormatService formatService)
        {
            _dataCollectingService = dataCollectingService;
            _bufferService = bufferService;
            _formatService = formatService;
        }

        private void WatchBuffer()
        {
            _bufferService.ItemsReceived += _bufferService_ItemsReceived;

        }

        private void _bufferService_ItemsReceived(DataEventArgs<List<string>> dataItems)
        {
             _formatService.Transform
        }
    }
}
