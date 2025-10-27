using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class MyEntity
    {
        public class CRMService
        {
            private const string key = "851320a8d"; // nguoitinhkhongden
            public static double ConverTimestamp(DateTime time)
            {
                double timeConvert = Math.Truncate((time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
                return timeConvert;
            }
            public static string SubString(string str, int num)
            {
                string tmp = "";
                if (str.Length > num)
                {
                    tmp = str.Substring(0, num - 1);
                }
                else
                {
                    tmp = str;
                }
                return tmp;
            }
        }

        public class MyDateTime
        {
            public int Year { get; set; } = 0;
            public int Month { get; set; } = 0;
            public int Day { get; set; } = 0;
        }
    }

}
