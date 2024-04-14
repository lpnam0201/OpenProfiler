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
        private string sqlParamsText;
        private TextDocument sqlParamsTextDocument;

        public SessionEventViewModel(DateTime timeStamp, string message)
        {
            this.TimeStamp = timeStamp;
            this.message = message.Trim();
            var (sqlQueryText, sqlParamsText) = SqlStringUtils.Format(message);
            this.sqlQueryText = sqlQueryText;
            this.sqlParamsText = sqlParamsText;
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

        public TextDocument SqlParametersText
        {
            get
            {
                if (sqlParamsTextDocument == null)
                {
                    this.sqlParamsTextDocument = new TextDocument(this.sqlParamsText);
                }
                return this.sqlParamsTextDocument;
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
