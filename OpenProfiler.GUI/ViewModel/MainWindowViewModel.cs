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
        private ObservableCollection<QueryListItem> _queryListItems = new ObservableCollection<QueryListItem>();
        public ObservableCollection<QueryListItem> QueryListItems
        {
            get { return _queryListItems; }
            set { SetProperty(ref _queryListItems, value); }
        }
        private QueryDetail _queryDetail;
        public QueryDetail QueryDetail
        {
            get { return _queryDetail;  }
            set { SetProperty(ref _queryDetail, value); }
        }

        private readonly IDataReceivingService _dataReceivingService;
        private readonly IBufferService _bufferService;
        private readonly IFormatService _formatService;

        public MainWindowViewModel(IDataReceivingService dataCollectingService,
            IBufferService bufferService,
            IFormatService formatService)
        {
            _dataReceivingService = dataCollectingService;
            _bufferService = bufferService;
            _formatService = formatService;

            WatchBuffer();
            _dataReceivingService.StartCollecting();
            QueryListItems = new ObservableCollection<QueryListItem>
            {
                new QueryListItem
                {
                    Text = " 34"
                }
            };
        }

        private void WatchBuffer()
        {
            _bufferService.ItemsReceived += BufferService_ItemsReceived;

        }

        private void BufferService_ItemsReceived(DataEventArgs<List<string>> dataItems)
        {
            var queryListItem = dataItems.Value.Select(x => _formatService.Transform(x))
                .SelectMany(x => x);
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var item in queryListItem)
                {
                    QueryListItems.Add(item);
                }
            });
        }
    }
}
