namespace OpenProfiler.WPF.ViewModels
{
    using System;
    using System.Text.RegularExpressions;
    using ICSharpCode.AvalonEdit.Document;
    using OpenProfiler.WPF.Utils;

    public class SessionEventViewModel : ViewModelBase
    {
        private string message;
        private string sqlQueryText;
        private TextDocument sqlQueryTextDocument;

        public SessionEventViewModel(DateTime timeStamp, string message)
        {
            this.TimeStamp = timeStamp;
            this.message = message.Trim();
            this.sqlQueryText = SqlStringUtils.Format(message);
        }

        public DateTime TimeStamp { get; private set; }

        public TextDocument FormattedSqlDocument
        {
            get
            {
                if (sqlQueryTextDocument == null)
                {
                    this.sqlQueryTextDocument = new TextDocument(this.sqlQueryText);
                }
                return this.sqlQueryTextDocument;
            }
        }

        public string MessagePreview
        {
            get
            {
                return Regex.Replace(this.message, @"\s+", " ").Trim();
            }
        }

        public override string ToString()
        {
            return this.message;
        }
    }
}
