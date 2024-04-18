namespace OpenProfiler.WPF.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using PoorMansTSqlFormatterRedux.Formatters;
    using PoorMansTSqlFormatterRedux.Parsers;
    using PoorMansTSqlFormatterRedux.Tokenizers;

    public class SqlStringUtils
    {
        // Match: @p0 = '123' [Type: String (4000:0:0)]
        private static Regex SqlParamPattern = new Regex(@"(@p\d+) = (.+?) \[Type.+?\]");

        public static string Format(string str)
        {
            var splitted = str.Split(";\n;");

            var hasParams = splitted.Length > 0;

            var sqlQueryText = splitted[0];
            if (hasParams)
            {
                var sqlParamsText = splitted[1];
                var paramsDictionary = ParseToParamsDictionary(sqlParamsText);
                sqlQueryText = SubstituteParamsToSqlQuery(sqlQueryText, paramsDictionary);
            }

            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(sqlQueryText);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);
            var formattedSqlQueryText = new TSqlStandardFormatter().FormatSQLTree(parsed);

            return formattedSqlQueryText;
        }

        private static IDictionary<string, string> ParseToParamsDictionary(string sqlParamsText)
        {
            var paramsDictionary = new Dictionary<string, string>();
            var matches = SqlParamPattern.Matches(sqlParamsText).ToList();
            foreach (var match in matches)
            {
                var groups = match.Groups;
                var paramName = groups[1].Value;
                var paramValue = groups[2].Value;

                paramsDictionary.Add(paramName, paramValue);
            }

            return paramsDictionary;
        }

        private static string SubstituteParamsToSqlQuery(string sqlQueryText, IDictionary<string, string> paramsDictionary)
        {
            var result = new StringBuilder(sqlQueryText);

            // Replace @p101,...@p10 before replacing @p1
            var longestParamFirst = paramsDictionary.OrderByDescending(x => x.Key.Length);
            foreach (var param in longestParamFirst)
            {
                result = result.Replace(param.Key, param.Value);    
            }

            return result.ToString();
        }
    }
}
