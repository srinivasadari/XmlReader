using System;
using System.Collections.Generic;
using System.Linq;

namespace XmlReader.FileWatcher.Extensions
{
   public static class StringExtension
    {
        public static List<string> GetCommaSperatedValues(this string commaSeparatedValue)
        {
            var items = commaSeparatedValue.Split(",").Select(p => p.Trim())
                        .Where(v => !string.IsNullOrWhiteSpace(v)).ToList();

            return items;
        }

        public static List<int> GetCommaSperatedAndConvertToInt(this string commaSeparatedValue)
        {
            int ptr = 0;
            var items = commaSeparatedValue.Split(",").Select(p => p.Trim())
                        .Where(v => !string.IsNullOrWhiteSpace(v)).ToList()
                        .Where(str => int.TryParse(str, out ptr))
                        .Select(str => ptr).ToList();
            
            return items;
        }
    }
}
