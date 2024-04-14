namespace OpenProfiler.WPF.ViewModels
{
    using System;
    using System.Text.RegularExpressions;
    using ICSharpCode.AvalonEdit.Document;
    using OpenProfiler.WPF.Utils;

    public class SessionEventViewModel : ViewModelBase
    {
        private string message;
        private TextDocument textDocument;

        public SessionEventViewModel(DateTime timeStamp, string message)
        {
            this.TimeStamp = timeStamp;
            this.message = message.Trim();
        }

        public DateTime TimeStamp { get; private set; }

        public TextDocument FormattedSqlDocument
        {
            get
            {
                if (this.textDocument == null)
                {
                    string formatted = SqlStringUtils.Format(this.message);
                    this.textDocument = new TextDocument(formatted);
                }

                return this.textDocument;
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
