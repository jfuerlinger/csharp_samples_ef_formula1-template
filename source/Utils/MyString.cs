using System;

namespace Utils
{
    /// <summary>
    /// Extension-Methoden für String
    /// </summary>
    public static class MyString
    {
        /// <summary>
        /// Text in DoubleWert umwandeln
        /// </summary>
        /// <param name="text"></param>
        /// <returns>DoubleWert oder MinValue im Fehlerfall</returns>
        public static double ParseDoubleOrError(this string text)
        {
            double wert;
            if (!Double.TryParse(text, out wert))
            {
                wert = Double.MinValue;
            }
            return wert;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToShortString(this string text, int length)
        {
            if (text.Length <= length)
            {
                return text;
            }
            return text.Substring(0, length-3)+"...";
        }
    }
}
