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
using Microsoft.Phone.Scheduler;

namespace NotepadPlus.Notes
{
    public class NotesReminder
    {
        public static bool AddOrUpdateReminder(Note note)
        {
            try
            {
                Reminder noteReminder = GetReminder(note.Id);

                if (noteReminder == null)
                {
                    noteReminder = new Reminder(note.Id);                    
                    noteReminder.Content = note.Title;
                    noteReminder.BeginTime = note.ReminderDate + note.ReminderTime.TimeOfDay;
                    noteReminder.ExpirationTime = noteReminder.BeginTime.AddMinutes(10);
                    noteReminder.NavigationUri = new Uri("/AddOrEditNote.xaml?noteId=" + note.Id, UriKind.Relative);

                    ScheduledActionService.Add(noteReminder);
                }
                else
                {
                    noteReminder.Content = note.Title;
                    noteReminder.BeginTime = note.ReminderDate + note.ReminderTime.TimeOfDay;
                    noteReminder.ExpirationTime = noteReminder.BeginTime.AddMinutes(10);

                    ScheduledActionService.Replace(noteReminder);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Reminder GetReminder(string reminderId)
        {
            return ScheduledActionService.Find(reminderId) as Reminder;            
        }

        public static void DeleteReminder(string reminderId)
        {
            Reminder noteReminder = GetReminder(reminderId);

            if (noteReminder != null)
                ScheduledActionService.Remove(reminderId);
        }

    }
}
