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
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        public NoteReminder()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NoteReminder_Loaded);
        }

        void NoteReminder_Loaded(object sender, RoutedEventArgs e)
        {    
        }

        private void tkDate_ValueChanged(object sender, Microsoft.Phone.Controls.DateTimeValueChangedEventArgs e)
        {
            this.Date = e.NewDateTime.HasValue ? e.NewDateTime.Value : DateTime.Now;
        }

        private void tkTime_ValueChanged(object sender, Microsoft.Phone.Controls.DateTimeValueChangedEventArgs e)
        {
            this.Time = e.NewDateTime.HasValue ? e.NewDateTime.Value : DateTime.Now;
        }

        private void reminderOnOff_Checked(object sender, RoutedEventArgs e)
        {
            tkDate.IsEnabled = tkTime.IsEnabled = true;
        }

        private void reminderOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            tkDate.IsEnabled = tkTime.IsEnabled = false;
        }
        
    }
}
