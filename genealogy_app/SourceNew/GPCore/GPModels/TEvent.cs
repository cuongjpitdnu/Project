using System;
using System.Collections.Generic;
using System.Text;

namespace GPModels
{
    public class TEvent : BaseModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string background_color { get; set; }
        public string text_color { get; set; }
        public string user_created { get; set; }
        public string place { get; set; }
        public int s_moon_day { get; set; }
        public int s_moon_month { get; set; }
        public int s_moon_year { get; set; }
        public int e_moon_day { get; set; }
        public int e_moon_month { get; set; }
        public int e_moon_year { get; set; }
        public DateTime? s_date { get; set; }
        public DateTime? e_date { get; set; }
        public string time_from { get; set; }
        public string time_to { get; set; }
        public string events_type { get; set; }
        public string iterate { get; set; }
        public bool calendar_type { get; set; }
        public bool important { get; set; }
        public bool leapmonthStart { get; set; }
        public bool leapmonthEnd { get; set; }
        public bool activate { get; set; }
        public DateTime? created_at { get; set; }
    }
    public class DateTimeVN
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTimeVN(string sDate)
        {
            string[] temp = sDate.Split('/');
            int.TryParse(temp[0], out int day);
            int.TryParse(temp[1], out int month);
            int.TryParse(temp[2], out int year);
            Day = day;
            Month = month;
            Year = year;
        }
    }
}
