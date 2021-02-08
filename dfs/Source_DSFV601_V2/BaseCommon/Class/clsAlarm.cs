using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;

namespace BaseCommon
{
    class Alarm : INotifyPropertyChanged
    {
        private bool is2kDevices;
        private int stt;
        private string date;
        private string time;
        private string upper;
        private string lower;
        private bool isAlarm;
        private double volt;
        private Point point;

        public Alarm(int stt, string date, string time, string upper, string lower, double volt, bool is2kDevices)
        {
            this.stt = stt;
            this.date = date;
            this.time = time;
            this.upper = upper;
            this.lower = lower;
            this.volt = volt;
            this.is2kDevices = is2kDevices;
        }

        public Alarm(int stt, string date, string time, string upper, string lower, double volt)
        {
            this.stt = stt;
            this.date = date;
            this.time = time;
            this.upper = upper;
            this.lower = lower;
            this.volt = volt;
        }

        public Alarm(int stt, string date, string time, string upper, string lower)
        {
            this.stt = stt;
            this.date = date;
            this.time = time;
            this.upper = upper;
            this.lower = lower;
        }

        public bool Is2kDevice
        {
            get
            {
                return is2kDevices;
            }

            set
            {
                is2kDevices = value;
                OnPropertyChanged("Is2kDevices");
            }
        }

        public int Stt
        {
            get
            {
                return stt;
            }

            set
            {
                stt = value;
                OnPropertyChanged("Stt");
            }
        }

        public string Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public string Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }

        public string Upper
        {
            get
            {
                return upper;
            }

            set
            {
                upper = value;
                OnPropertyChanged("Upper");
            }
        }

        public string Lower
        {
            get
            {
                return lower;
            }

            set
            {
                lower = value;
                OnPropertyChanged("Lower");
            }
        }

        public Point Point
        {
            get
            {
                return point;
            }

            set
            {
                point = value;
                OnPropertyChanged("Point");
            }
        }

        public double Volt
        {
            get
            {
                return volt;
            }

            set
            {
                volt = value;
                if (volt > double.Parse(upper) || volt < double.Parse(lower))
                {
                    IsAlarm = true;
                }
                else
                {
                    IsAlarm = false;
                }
                OnPropertyChanged("Volt");
            }
        }

        public bool IsAlarm
        {
            get
            {
                return isAlarm;
            }

            set
            {
                isAlarm = value;
                OnPropertyChanged("IsAlarm");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
    }
}
