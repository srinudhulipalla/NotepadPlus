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
using Microsoft.Phone.Controls;

namespace NotepadPlus
{
    public partial class NoteReminder : ChildWindow
    {
        public NoteReminder1()
        {
            InitializeComponent();



            closeButton.Click += (s, e) =>
            {
                this.DialogResult = true;
                this.Close();
            };
        }

        private void datePicker1_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            //this.Close();
        }

        private void datePicker1_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            //this.Show();
        }

        private void datePicker1_KeyUp(object sender, KeyEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void datePicker1_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            this.Show();
        }

        private void datePicker1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Close();
        }

        private void timeSpanPicker1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Close();
        }

        private void timeSpanPicker1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<TimeSpan> e)
        {
            this.Show();
        }
    }
}