namespace OpenProfiler.GUI.Service
{
    public delegate void DataReceivedEvent(DataEventArgs<List<string>> dataItems);
    public class BufferService : IBufferService
    {
        public event DataReceivedEvent ItemsReceived;

        private static int FlushThreshold = 10;
        private IList<string> _dataItems = new List<string>();
        private System.Timers.Timer _timer;

        public BufferService()
        {
            _timer = new System.Timers.Timer(200);
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void TimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Flush();
        }

        private void Flush()
        {
            if (_dataItems.Count == 0)
            {
                return;
            }
            lock ()
            {
                var copiedList = _dataItems.ToList();
                ItemsReceived?.Invoke(new DataEventArgs<List<string>>(copiedList));
                _dataItems.Clear();
            }
            
        }

        public void Add(string dataItem)
        {
            if (_dataItems.Contains(dataItem))
            {
                throw new InvalidOperationException();
            }
            _dataItems.Add(dataItem);
            if (_dataItems.Count > FlushThreshold)
            {
                Flush();
            }
        }
    }
}
