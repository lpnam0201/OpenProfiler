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
        private static Regex SqlParamPattern = new Regex(@"@p\d+ = .+? \[Type.+?\]");

        public static (string SqlQueryText, string SqlParamsText) Format(string str)
        {
            var splitted = str.Split(";\n;");

            var sqlQueryText = splitted[0];
            var sqlParamsText = splitted[1];

            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(sqlQueryText);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);
            var formattedSqlQueryText = new TSqlStandardFormatter().FormatSQLTree(parsed);
            var formattedSqlParamsText = FormatSqlParams(sqlParamsText);

            return (formattedSqlQueryText, formattedSqlParamsText);
        }

        private static string FormatSqlParams(string sqlParamsText)
        {
            var matches = SqlParamPattern.Matches(sqlParamsText);

            return String.Join("\r\n", matches.Select(x => x.ToString()));
        }
    }
}
