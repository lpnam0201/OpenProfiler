using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.ViewModel
{
    public class QueryListItem : BindableBase
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { SetProperty(ref _timestamp, value); }
        }

        private string _loggerName;
        public string LoggerName
        {
            get { return _loggerName; }
            set { SetProperty(ref _loggerName, value); }
        }
    }
}
