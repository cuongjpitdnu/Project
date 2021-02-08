using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeaDSF601
{
    class AlarmSetting : INotifyPropertyChanged
    {
        public const int CHANGE_IP = 1;
        public const int CHANGE_PASSWORD = 2;
        public const int CHANGE_LEVEL = 4;
        public const int CHANGE_TIMES = 8;
        public const int CHANGE_SIGNAL_OUTPUT = 16;

        private bool isSetting;
        private bool isSignalOutput;
        private int level;
        private int times;
        private string ip;
        private string password;
        private int change;
        private ObservableCollection<Alarm> alarmList;

        public bool IsSetting
        {
            get
            {
                return isSetting;
            }

            set
            {
                isSetting = value;
                OnPropertyChanged("IsSetting");
            }
        }

        public bool IsSignalOutput
        {
            get
            {
                return isSignalOutput;
            }

            set
            {
                isSignalOutput = value;
                OnPropertyChanged("IsSignalOutput");
            }
        }

        public int Level
        {
            get
            {
                return level;
            }

            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
        }

        public int Times
        {
            get
            {
                return times;
            }

            set
            {
                times = value;
                OnPropertyChanged("Times");
            }
        }

        public string Ip
        {
            get
            {
                return ip;
            }

            set
            {
                ip = value;
                OnPropertyChanged("Ip");
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public int Change
        {
            get { return change; }
            set
            {
                change = value;
                OnPropertyChanged("Change");
            }
        }

        public ObservableCollection<Alarm> AlarmList
        {
            get
            {
                return alarmList;
            }

            set
            {
                alarmList = value;
                OnPropertyChanged("AlarmList");
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
