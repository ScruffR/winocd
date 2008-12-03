using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDB_Frontend.Parsing
{
    public class ParseTools
    {
        /// <summary>
        /// Extrahiert den String der zwischen einem Start Pattern und einem Endpattern liegt
        /// </summary>
        /// <param name="data">Der zu untersuchende String</param>
        /// <param name="start_pattern">Startstring nachdem der gesuchte String beginnt</param>
        /// <param name="end_pattern">Stopstring der das Ende des gesuchten Strings makiert</param>
        /// <returns>Den gesuchten String zwischen start und end Pattern</returns>
        public static string extract_data_betweenPatterns(string data, string start_pattern, string end_pattern)
        {
            int start_index, end_index;

            try
            {
                start_index = data.IndexOf(start_pattern);

                if (start_index == -1) return "";
                else
                {
                    start_index = start_index + start_pattern.Length;
                }
            }
            catch
            {
                return "";
            }

            try
            {
                end_index = data.IndexOf(end_pattern, start_index);
                if (end_index == -1) return "";

            }
            catch
            {
                return "";
            }



            return data.Substring(start_index, end_index - start_index);
        }

       public static string extract_data_fromPatternToEnd(string data, string start_pattern)
        {
            int start_index;

            try
            {
                start_index = data.IndexOf(start_pattern);

                if (start_index == -1) return "";
                else
                {
                    start_index = start_index + start_pattern.Length;
                }
            }
            catch
            {
                return "";
            }

            return data.Substring(start_index, data.Length - start_index);
        }

    }
}
