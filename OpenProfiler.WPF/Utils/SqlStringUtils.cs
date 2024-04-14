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
        private static Regex variableRegex = new Regex(@"((?:@|\?)p\d+)\s=\s(.+?)\s\[Type:\s(.+?)\s\(\d+\)\]");
        private static Dictionary<DbType, Type> typeMap = new Dictionary<DbType, Type>
        {
            { DbType.AnsiString, typeof(string) },
            { DbType.AnsiStringFixedLength, typeof(string) },
            { DbType.Binary, typeof(byte[]) },
            { DbType.Boolean, typeof(bool) },
            { DbType.Byte, typeof(byte) },
            { DbType.Currency, typeof(decimal) },
            { DbType.Date, typeof(DateTime) },
            { DbType.DateTime, typeof(DateTime) },
            { DbType.DateTime2, typeof(DateTime) },
            { DbType.DateTimeOffset, typeof(DateTimeOffset) },
            { DbType.Decimal, typeof(decimal) },
            { DbType.Double, typeof(double) },
            { DbType.Guid, typeof(Guid) },
            { DbType.Int16, typeof(short) },
            { DbType.Int32, typeof(int) },
            { DbType.Int64, typeof(long) },
            { DbType.Object, typeof(object) },
            { DbType.SByte, typeof(sbyte) },
            { DbType.Single, typeof(float) },
            { DbType.String, typeof(string) },
            { DbType.StringFixedLength, typeof(string) },
            { DbType.Time, typeof(DateTime) },
            { DbType.UInt16, typeof(ushort) },
            { DbType.UInt32, typeof(uint) },
            { DbType.UInt64, typeof(ulong) }
        };

        public static string Format(string str)
        {
            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(str);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);
            var outputSql = new TSqlStandardFormatter().FormatSQLTree(parsed);
            List<string> result = new List<string> { outputSql };

            string query;

            if (result.Count > 1)
            {
                query = string.Join(string.Empty, result.Take(result.Count - 1));

                var queryBuilder = new StringBuilder(query);

                MatchCollection variableMatches = variableRegex.Matches(result.Last());

                for (int i = variableMatches.Count - 1; i >= 0; i--)
                {
                    Match match = variableMatches[i];

                    string variableName = match.Groups[1].Value;
                    string processed = ProcessMatch(match.Groups[2].Value, match.Groups[3].Value);

                    queryBuilder.Replace(variableName, processed);
                }

                query = outputSql;
            }
            else
            {
                query = outputSql;
            }

            return query;
        }

        private static string ProcessMatch(string variableValue, string variableType)
        {
            DbType databaseType = (DbType)Enum.Parse(typeof(DbType), variableType);

            Type dotNetType = typeMap[databaseType];

            string result = null;

            if (dotNetType == typeof(Guid))
            {
                result = string.Format("'{0}'", variableValue);
            }
            else if (dotNetType == typeof(bool))
            {
                bool value = bool.Parse(variableValue);
                result = Convert.ToInt32(value).ToString();
            }
            else if (dotNetType == typeof(DateTime))
            {
                DateTime value = DateTime.Parse(variableValue);

                result = string.Format("'{0:yyyy-MM-dd HH:mm:ss}'", value);
            }
            else
            {
                result = variableValue.ToString();
            }

            return result;
        }
    }
}
