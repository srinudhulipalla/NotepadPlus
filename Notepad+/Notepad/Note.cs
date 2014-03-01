using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace NotepadPlus.Notes
{   
    public class Note : INotifyPropertyChanged
    {
        private string _id = string.Empty;
        private string _title = string.Empty;
        private string _content = string.Empty;
        private Visibility _hasReminder = Visibility.Collapsed;
        private Visibility _hasAudio = Visibility.Collapsed;
        private DateTime _reminderDate = DateTime.Now;
        private DateTime _reminderTime = DateTime.Now;
        private DateTime _created;
        private DateTime _modified;        

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value != _content)
                {
                    _content = value;
                    NotifyPropertyChanged("Content");
                }
            }
        }

        public Visibility HasAudio
        {
            get
            {
                return _hasAudio;
            }
            set
            {
                if (value != _hasAudio)
                {
                    _hasAudio = value;
                    NotifyPropertyChanged("HasAudio");
                }
            }
        }

        public Visibility HasReminder
        {
            get
            {
                return _hasReminder;
            }
            set
            {
                if (value != _hasReminder)
                {
                    _hasReminder = value;
                    NotifyPropertyChanged("HasReminder");
                }
            }
        }

        public DateTime ReminderDate
        {
            get
            {
                return _reminderDate;
            }
            set
            {
                if (value != _reminderDate)
                {
                    _reminderDate = value;
                    NotifyPropertyChanged("ReminderDate");
                }
            }
        }

        public DateTime ReminderTime
        {
            get
            {
                return _reminderTime;
            }
            set
            {
                if (value != _reminderTime)
                {
                    _reminderTime = value;
                    NotifyPropertyChanged("ReminderTime");
                }
            }
        }

        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                if (value != _created)
                {
                    _created = value;
                    NotifyPropertyChanged("Created");
                }
            }
        }

        public DateTime Modified
        {
            get
            {
                return _modified;
            }
            set
            {
                if (value != _modified)
                {
                    _modified = value;
                    NotifyPropertyChanged("Modified");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    } 
}
