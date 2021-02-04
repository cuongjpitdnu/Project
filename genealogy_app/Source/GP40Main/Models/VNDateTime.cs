namespace GP40Main.Models
{
    public class VNDate
    {
        public int DaySun { get; set; }
        public int MonthSun { get; set; }
        public int YearSun { get; set; }

        public int DayMoon { get; set; }
        public int MonthMoon { get; set; }
        public int YearMoon { get; set; }

        private const string UNKNOWN = "--";
        private const string FOMAT_DISPLAY = "{0}/{1}/{2}";

        public string ToDateSun()
        {
            return ToDate(DaySun, MonthSun, YearSun);
        }

        public string ToDateMoon()
        {
            return ToDate(DayMoon, MonthMoon, YearMoon);
        }

        private string ToDate(int day, int moth, int year)
        {
            var dayString = day > 0 ? day.ToString() : UNKNOWN;
            var monthString = moth > 0 ? moth.ToString() : UNKNOWN;
            var yearString = year > 0 ? year.ToString() : UNKNOWN;

            dayString = dayString.PadLeft(2, '0');
            monthString = monthString.PadLeft(2, '0');
            yearString = yearString.PadLeft(4, yearString != UNKNOWN ? '0' : '-');

            return string.Format(FOMAT_DISPLAY, dayString, monthString, yearString);
        }
    }
}
