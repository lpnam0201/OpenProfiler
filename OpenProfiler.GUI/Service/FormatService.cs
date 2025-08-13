using OpenProfiler.GUI.Model;
using OpenProfiler.GUI.Util;
using OpenProfiler.GUI.ViewModel;
using PoorMansTSqlFormatterRedux.Formatters;
using PoorMansTSqlFormatterRedux.Parsers;
using PoorMansTSqlFormatterRedux.Tokenizers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OpenProfiler.GUI.Service
{
    public class FormatService : IFormatService
    {
        public List<QueryListItem> Transform(string dataStr)
        {
            var dataItems = JsonSerializer.Deserialize<List<DataItem>>(dataStr);

            return dataItems.Select(x =>
            {
                var (processedText, parameters) = RemoveParametersFromSqlText(x.Message);
                processedText = TrimAtTheEnd(processedText);
                processedText = FormatSqlText(processedText);
                processedText = ApplyParameters(processedText, parameters);
                return new QueryListItem()
                {
                    Guid = x.Guid,
                    Text = processedText,
                    LoggerName = x.LoggerName,
                    Timestamp = x.Timestamp
                };
            }).ToList();
        }

        private string ApplyParameters(string sqlText, IList<SqlParameter> parameters)
        {
            // use StringBuilder
            var finalText = sqlText;
            var queue = new Queue<SqlParameter>(parameters);

            while (queue.Count > 0)
            {
                var param = queue.Dequeue();
                var formattedValue = SqlValueFormatter.Format(param.Value, param.Type);
                var match = Regex.Matches(finalText, Constants.FormattingConstants.ParameterPattern)
                    .FirstOrDefault(x => x.Value == param.Name);
                if (match != null)
                {
                    finalText = finalText.Remove(match.Index, match.Length)
                        .Insert(match.Index, formattedValue);
                }

            }

            return finalText;
        }

        private (string, IList<SqlParameter>) RemoveParametersFromSqlText(string sqlText)
        {
            var parameters = new List<SqlParameter>();

            var processedText = sqlText;
            do
            {
                var parameterMatch = Regex.Match(processedText, Constants.FormattingConstants.ParameterAndValuePattern);
                if (!parameterMatch.Success)
                {
                    break;
                }
                var sqlParameter = new SqlParameter
                {
                    Name = parameterMatch.Groups[1].Value,
                    Value = parameterMatch.Groups[2].Value,
                    Type = parameterMatch.Groups[5].Value
                };
                parameters.Add(sqlParameter);
                processedText = processedText.Replace(parameterMatch.Value, "");
            } while(true);
            return (processedText, parameters);
        }

        private string FormatSqlText(string sqlText)
        {
            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(sqlText);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);
            var formatted = new TSqlStandardFormatter().FormatSQLTree(parsed);

            return formatted;
        }

        private string TrimAtTheEnd(string sqlText)
        {
            return sqlText.TrimEnd(',', ' ', ';', '\r', '\n');
        }
    }
}
