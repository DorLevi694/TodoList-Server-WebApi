using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListWebApi.Utilities
{
    public static class StringsValidator
    {
        private static readonly char[] delimiter = null ;

        public static bool ValidateLengthAndWords(this string str, int minLength, int minWords)
        {
            var wordsCount = str.Split(delimiter, StringSplitOptions.RemoveEmptyEntries).Length;

            return ((str.Length >= 30) && (wordsCount >= 10));
           
        }
    }
}
