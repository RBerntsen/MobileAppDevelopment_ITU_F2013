using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1887.Backend.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string RemoveNewLineTag(this string stringInput)
        {
            return stringInput.Replace("\n", string.Empty);
        }

        public static string RemoveTabTag(this string stringInput)
        {
            return stringInput.Replace("\t", string.Empty);
        }
    }
}
