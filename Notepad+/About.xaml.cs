﻿using System;
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
using NotepadPlus.Notes;

namespace NotepadPlus
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void MailToDeveloper_Click(object sender, RoutedEventArgs e)
        {
            string subject = string.Format("{0} v{1} - Feedback", NotepadSettings.AppName, Common.GetAppVersion());
            Common.SendEmail(NotepadSettings.DeveloperEmail, subject, string.Empty);
        }
    }
}