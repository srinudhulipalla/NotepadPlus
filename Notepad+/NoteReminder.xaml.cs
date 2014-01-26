using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace NotepadPlus
{
    public partial class NoteReminder : UserControl
    {
        public bool IsReminderSet { get; set; } 

        public DateTime Date
        {
            get 
            {
                return tkDate.Value.HasValue ? tkDate.Value.Value : DateTime.Today;
            }
            set { tkDate.Value = value; }
        }

        public DateTime Time
        {
            get
            {
                return tkTime.Value.HasValue ? tkTime.Value.Value : DateTime.Now; 
            }
            set { tkTime.Value = value; }            
        }

        public bool IsReminderEnabled
        {
            get
            {
                return reminderOnOff.IsChecked.HasValue ? reminderOnOff.IsChecked.Value : false;
            }
            set
            {
                UpdateReminder(value);
                reminderOnOff.IsChecked = value;
            }
        }

        public NoteReminder()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NoteReminder_Loaded);
        }

        private void UpdateReminder(bool isReminderOn)
        {
            if (isReminderOn)
            {
                spDateTime.Visibility = Visibility.Visible;
                reminderOnOff.Content = "On";
            }
            else
            {
                spDateTime.Visibility = Visibility.Collapsed;
                reminderOnOff.Content = "Off";
            }
        }

        void NoteReminder_Loaded(object sender, RoutedEventArgs e)
        {    

        }

        private void tkDate_ValueChanged(object sender, Microsoft.Phone.Controls.DateTimeValueChangedEventArgs e)
        {
            this.Date = e.NewDateTime.HasValue ? e.NewDateTime.Value : DateTime.Today;
        }

        private void tkTime_ValueChanged(object sender, Microsoft.Phone.Controls.DateTimeValueChangedEventArgs e)
        {
            this.Time = e.NewDateTime.HasValue ? e.NewDateTime.Value : DateTime.Now;
        }

        private void reminderOnOff_Checked(object sender, RoutedEventArgs e)
        {
            UpdateReminder(true);
        }

        private void reminderOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateReminder(false);
        }
        
    }
}
