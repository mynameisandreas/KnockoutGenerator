using System;
using System.Text;

namespace KnockoutGenerator.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatCamelCase(this string str)
        {
            var sb = new StringBuilder(str.Length);

            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException("A null or empty value cannot be converted", str);
            
            var stringArray = str.ToCharArray();
            return sb.AppendFormat("{0}{1}", stringArray[0].ToString().ToLower(), str.Substring(1)).ToString();
        }
    }
}
