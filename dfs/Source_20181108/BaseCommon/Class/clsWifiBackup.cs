using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCommon
{
    class WifiBackup : INotifyPropertyChanged
    {
        private int stt;
        private AlarmSetting alarmSetting;
        private string name = "";
        private string ssid = "";
        private string runName = "";
        private string imageSource = "";
        private bool state = false;
        //private SolidColorBrush foregroundAlarm;
        private string lastAlarm;
        private int countAlarm;
        //private SolidColorBrush backgroundColor;

        private double currentVolt;
        public double CurrentVolt
        {
            get
            {
                return currentVolt;
            }

            set
            {
                currentVolt = value;
                OnPropertyChanged("CurrentVolt");
            }
        }


        //public SolidColorBrush BackgroundColor
        //{
            //get { return backgroundColor; }
            //set { backgroundColor = value; OnPropertyChanged("BackgroundColor"); }
        //}

        public int CountAlarm
        {
            get { return countAlarm; }
            set { countAlarm = value; OnPropertyChanged("CountAlarm"); }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Ssid
        {
            get
            {
                return ssid;
            }

            set
            {
                ssid = value;
                OnPropertyChanged(nameof(Ssid));
            }
        }

        public string RunName
        {
            get
            {
                return runName;
            }
            set
            {
                runName = value;
                OnPropertyChanged(nameof(RunName));
            }
        }

        public string ImageSource
        {
            get
            {
                return imageSource;
            }

            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public bool State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        public AlarmSetting AlarmSetting
        {
            get
            {
                return alarmSetting;
            }

            set
            {
                alarmSetting = value;
                OnPropertyChanged("AlarmSetting");
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

        //public SolidColorBrush ForegroundAlarm
        //{
        //    get
        //    {
        //        return foregroundAlarm;
        //    }

        //    set
        //    {
        //        foregroundAlarm = value;
        //        OnPropertyChanged("ForegroundAlarm");
        //    }
        //}

        public string LastAlarm
        {
            get
            {
                return lastAlarm;
            }

            set
            {
                lastAlarm = value;
                OnPropertyChanged("LastAlarm");
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
