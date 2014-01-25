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
using Microsoft.Phone.Tasks;
using System.Text;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;

namespace NotepadPlus.Notes
{
    public class Common
    {
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

        public static void EmailNote(Note note)
        {
            EmailComposeTask emailTask = new EmailComposeTask();
            emailTask.Subject = string.Format("{0} - {1}", NotepadSettings.AppName, note.Title);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<b>Note Title: </b>" + note.Title);
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("<b>Note Contents:</b>");
            sb.AppendLine(note.Content);
            sb.AppendLine(Environment.NewLine);

            if (note.HasReminder == Visibility.Visible)
            {
                sb.AppendLine("<b>Reminder: </b>" + (note.ReminderDate + note.ReminderTime.TimeOfDay).ToString("U"));
            }

            emailTask.Body = sb.ToString();
            emailTask.Show();
        }        
        
    }
}
