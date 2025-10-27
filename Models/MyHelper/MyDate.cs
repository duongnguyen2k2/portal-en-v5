using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.MyHelper
{
    public class MyDate
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static List<SelectListItem> GetListDay()
        {
            List<SelectListItem> ListItem = new List<SelectListItem>();
            for (int i = 1; i < 32; i++)
            {
                ListItem.Insert(i - 1, (new SelectListItem { Text = i.ToString(), Value = i.ToString() }));
            }
            return ListItem;
        }

        public static List<SelectListItem> GetListMonth() {
            List<SelectListItem> ListItem = new List<SelectListItem>();
            for (int i = 1; i < 13; i++)
            {
                ListItem.Insert(i-1, (new SelectListItem { Text = i.ToString(), Value = i.ToString() }));
            }
            return ListItem;
        }

        public static List<SelectListItem> GetListYear(int Start=0, int End=0)
        {
            List<SelectListItem> ListItem = new List<SelectListItem>();
            if (Start == 0) {
                 Start = 2016;
            }
            if (End == 0)
            {
                End = DateTime.Now.Year + 1;
            }
            int n = 0;
            for (int i = Start; i < End+1; i++)
            {
                ListItem.Insert(n, (new SelectListItem { Text = i.ToString(), Value = i.ToString() }));
                n++;
            }
            return ListItem;
        }
    }
}
