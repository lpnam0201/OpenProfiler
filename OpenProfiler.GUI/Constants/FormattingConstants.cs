using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.Constants
{
    public static class FormattingConstants
    {
        public const string ParameterAndValuePattern = @"(@p\d+) = (.*?) \[(Type:)( )(.+?)( )(.+?)\]";
        public const string ParameterPattern = @"(@p)(\d+)";
    }
}
