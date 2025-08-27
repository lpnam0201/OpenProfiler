using OpenProfiler.GUI.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        private QueryListItem _selectedQueryListItem;
        public QueryListItem SelectedQueryListItem 
        { 
            get { return _selectedQueryListItem;  }
            set { SetProperty(ref _selectedQueryListItem, value); } 
        }

        private bool _isCollecting;
        public bool IsCollecting
        {
            get { return _isCollecting; }
            set { SetProperty(ref _isCollecting, value); }
        }

        private bool _isNotCollecting;
        public bool IsNotCollecting
        {
            get { return _isNotCollecting; }
            set { SetProperty(ref _isNotCollecting, value); }
        }

        public DelegateCommand QueryListItemSelectedCommand { get; set; }
        public DelegateCommand ClearCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand PauseCommand { get; set; }
        public DelegateCommand ResumeCommand { get; set; }

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
            QueryListItemSelectedCommand = new DelegateCommand(DisplayQueryDetail);
            ClearCommand = new DelegateCommand(ClearQueryList);
            PauseCommand = new DelegateCommand(Pause);
            ResumeCommand = new DelegateCommand(Resume);
            SearchCommand = new DelegateCommand(SearchAndUpdateQueryList);

            WatchBuffer();
            _dataReceivingService.StartCollecting();
            SetIsCollectingStates(true);
        }

        private void SetIsCollectingStates(bool isCollecting)
        {
            IsCollecting = isCollecting;
            IsNotCollecting = !isCollecting;
        }

        private void SearchAndUpdateQueryList()
        {

        }

        private void Pause()
        {
            _dataReceivingService.StopCollecting();
            SetIsCollectingStates(false);
        }

        private void Resume()
        {
            _dataReceivingService.StartCollecting();
            SetIsCollectingStates(true);
        }

        private void DisplayQueryDetail()
        {

            QueryDetail = SelectedQueryListItem != null
                ? new QueryDetail
                {
                    Text = SelectedQueryListItem.Text
                }
                : null;
        }

        private void ClearQueryList()
        {
            QueryListItems.Clear();
            QueryDetail = null;
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
