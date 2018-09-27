using System;
using System.Collections.Generic;
using System.Text;

namespace CSVSchema
{
    static class StringExtensionMethod
    {
        public static string[] Split(this string source, string separator)
        {
            return source.Split(new[] { separator }, StringSplitOptions.None);
        }
    }
}
