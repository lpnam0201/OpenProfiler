using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.Util
{
    public static class SqlValueFormatter
    {
        private static IDictionary<string, Func<string, string>> typeToFormatFnMap 
            = new Dictionary<string, Func<string, string>>
        {
            { "DateTime2", FormatDateTime },
            { "Date", FormatDateTime },
            { "String", x => x },
            { "Int64", x => x },
            { "Int32", x => x },
            { "Decimal", x => x },
            { "Boolean",FormatBoolean }
        };

        public static string Format(string value, string type)
        {
            if (!typeToFormatFnMap.ContainsKey(type))
            {
                throw new NotSupportedException("Not supported sql type: " + type);
            }
            var formatFn = typeToFormatFnMap[type];
            return formatFn(value);
        }

        private static string FormatDateTime(string value)
        {
            return $"'{value}'";
        }

        private static string FormatBoolean(string value)
        {
            return value == "True"
                ? "1=1"
                : "1=0";
        }
    }
}
