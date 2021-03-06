﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Tasks;
using System.Text;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using System.Reflection;

namespace NotepadPlus.Notes
{
    public class Common
    {
        public static string GetAppVersion()
        {
            Assembly exeAssembly = Assembly.GetExecutingAssembly();
            var name = new AssemblyName(exeAssembly.FullName);
            return name.Version.ToString(4);
        }

        public static void RateMyApp()
        {
            MarketplaceReviewTask appReview = new MarketplaceReviewTask();
            appReview.Show();
        }

        public static void MoreAppsFromMe()
        {
            MarketplaceSearchTask appSearch = new MarketplaceSearchTask();
            appSearch.ContentType = MarketplaceContentType.Applications;
            appSearch.SearchTerms = NotepadSettings.AppPublisher;
            appSearch.Show();
        }

        public static void ShareNote(Note note)
        {            
            ShareStatusTask shareTask = new ShareStatusTask();
            shareTask.Status = note.Content.Trim();
            shareTask.Show();
        }

        public static void PinNoteToStart(Note note)
        {
            try
            {
                string noteURI = "/AddOrEditNote.xaml?noteId=" + note.Id;

                ShellTile.Create(new Uri(noteURI, UriKind.Relative), new StandardTileData()
                {
                    Title = note.Title,
                    BackgroundImage = new Uri("/Background.png", UriKind.Relative),
                    BackBackgroundImage = new Uri("", UriKind.Relative),
                    BackTitle = note.Title,
                    BackContent = note.Content
                });
            }
            catch { }
        }

        public static void SendEmail(Note note)
        {
            EmailComposeTask emailTask = new EmailComposeTask();
            emailTask.Subject = string.Format("{0} - {1}", NotepadSettings.AppName, note.Title);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Note Title: " + note.Title);
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("Note Contents:");
            sb.AppendLine(note.Content);
            sb.AppendLine(Environment.NewLine);

            if (note.HasReminder == Visibility.Visible)
            {
                sb.AppendLine("Reminder: " + (note.ReminderDate + note.ReminderTime.TimeOfDay).ToString("U"));
            }

            emailTask.Body = sb.ToString();
            emailTask.Show();
        }

        public static void SendEmail(string toEmail, string subject, string body)
        {
            EmailComposeTask emailTask = new EmailComposeTask()
            {
                To = toEmail,
                Subject = subject,
                Body = body
            };

            emailTask.Show();
        }
        
    }
}
