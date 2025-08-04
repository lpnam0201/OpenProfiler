using OpenProfiler.GUI.Model;
using OpenProfiler.GUI.ViewModel;
using PoorMansTSqlFormatterRedux.Formatters;
using PoorMansTSqlFormatterRedux.Parsers;
using PoorMansTSqlFormatterRedux.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                processedText = TrimCommaAtTheEnd(processedText);
                processedText = FormatSqlText(processedText);
                processedText = ApplyParameters(processedText, parameters);
                return new QueryListItem()
                {
                    Text = processedText,
                    LoggerName = x.LoggerName,
                    Timestamp = x.Timestamp
                };
            }).ToList();
        }

        private string ApplyParameters(string sqlText, IDictionary<string, SqlParameter> parameterDictionary)
        {
            var finalText = string.Empty;
            var paramsInText = Regex.Replace("", "", match =>
            {
                var paramIndex = match.Groups[1];
            });
            foreach (var kvp in parameterDictionary)
            {
                var paramName = kvp.Key;
                var paramObj = kvp.Value;
                var dataType = paramObj.Type;
                var value = paramObj.Value;
                
            }

            return sqlText;
        }

        private (string, IDictionary<string, SqlParameter>) RemoveParametersFromSqlText(string sqlText)
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
            return (processedText, parameters.ToDictionary(x => x.Name));
        }

        private string FormatSqlText(string sqlText)
        {
            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(sqlText);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);
            var formatted = new TSqlStandardFormatter().FormatSQLTree(parsed);

            return formatted;
        }

        private string TrimCommaAtTheEnd(string sqlText)
        {
            return sqlText.TrimEnd(',', ' ', ';');
        }
    }
}
