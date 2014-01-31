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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using NotepadPlus.Notes;
using Microsoft.Phone.Shell;
using Coding4Fun.Toolkit.Controls;

namespace NotepadPlus
{
    public partial class AddOrEditNote : PhoneApplicationPage
    {
        private string NoteId = string.Empty;

        private Note EditedNote = null;
        NoteManager noteManager = new NoteManager();
        MessagePrompt ReminderPopup = null;

        Note _currentNote = null;
        Note CurrentNote
        {
            get
            {
                if (_currentNote == null)
                    _currentNote = new Note();

                return _currentNote;
            }
            set
            {
                _currentNote = value;
            }
        }

        NoteReminder _reminderControl = null;
        NoteReminder ReminderControl
        {
            get
            {
                if (_reminderControl == null)
                    _reminderControl = new NoteReminder();

                return _reminderControl;
            }
        }

        public AddOrEditNote()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AddOrEditNote_Loaded);
        }

        void GotoNotesView()
        {
            this.NavigationService.Navigate(new Uri("/ViewNotes.xaml", UriKind.Relative));
        }

        void ShowMessageDialog(string title, string message, bool showCancel)
        {
            var msgDialog = new MessagePrompt()
            {
                Title = title,
                Message = message,
                IsCancelVisible = showCancel
            };

            msgDialog.Show();
        }

        void AddOrEditNote_Loaded(object sender, RoutedEventArgs e)
        {            
            //initialize reminder details
            if (ReminderPopup == null)
            {
                ReminderPopup = new MessagePrompt();
                ReminderPopup.Title = NotepadSettings.NoteReminderTitle;
                ReminderPopup.Body = this.ReminderControl;
                ReminderPopup.IsCancelVisible = true;                

                ReminderPopup.Completed += (s, args) =>
                {
                    this.ReminderControl.IsReminderSet = args.PopUpResult == PopUpResult.Ok;

                    if (args.PopUpResult == PopUpResult.Ok)
                    {                        
                        imgReminderClock.Visibility = this.ReminderControl.IsReminderEnabled ? Visibility.Visible : Visibility.Collapsed;
                    }
                };
            }
            else if (!this.ReminderControl.IsReminderSet)
            {
                ReminderPopup.Show();
            }
            
            txtNoteTitle.BorderBrush.Opacity = 0;
            txtNoteContent.BorderBrush.Opacity = 0;
        }

        private Note GetCurrentNote()
        {            
            if (string.IsNullOrEmpty(this.NoteId)) //new note
            {
                this.CurrentNote = new Note()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = txtNoteTitle.Text.Trim(),
                    Content = txtNoteContent.Text.Trim(),
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };
            }
            else //edit note
            {
                this.CurrentNote = this.EditedNote;
                this.CurrentNote.Title = txtNoteTitle.Text.Trim();
                this.CurrentNote.Content = txtNoteContent.Text.Trim();
                this.CurrentNote.Modified = DateTime.Now;
            }

            if (this.ReminderControl.IsReminderSet && this.ReminderControl.IsReminderEnabled)
            {
                this.CurrentNote.ReminderDate = this.ReminderControl.Date;
                this.CurrentNote.ReminderTime = this.ReminderControl.Time;
                this.CurrentNote.HasReminder = Visibility.Visible;
            }            

            return this.CurrentNote;
        }

        private void SaveNote_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNoteTitle.Text.Trim()))
            {
                ShowMessageDialog("Warning!", NotepadSettings.EmptyNoteTitle, false);
                return;
            }
            else if (txtNoteTitle.Text.Trim().Length > 20)
            {
                ShowMessageDialog("Warning!", NotepadSettings.NoteTitleExceedLength, false);
                return;
            }

            Note note = GetCurrentNote();            

            //note reminder
            if (this.ReminderControl.IsReminderSet && this.ReminderControl.IsReminderEnabled)
            {    
                NotesReminder.AddOrUpdateReminder(note);
            }
            else
            {                
                NotesReminder.DeleteReminder(note.Id);
            }            

            //save note
            NoteManager noteManager = new NoteManager();
            bool success = noteManager.AddOrUpdateNote(note);
            note = null;

            if (success)
            {
                GotoNotesView();
            }
            else
            {
                ShowMessageDialog("Failure", NotepadSettings.NoteSaveFailure, false);
            }
        }

        private void DeleteNote_Click(object sender, EventArgs e)
        {
            var DeleteNotePrompt = new MessagePrompt()
            {
                Title = "Delete",
                Message = NotepadSettings.DeleteNoteConfirm,
                IsCancelVisible = true
            };            

            DeleteNotePrompt.Completed += (s, args) => //delete note on confirm
            {
                if (args.PopUpResult == PopUpResult.Ok)
                {
                    NotesReminder.DeleteReminder(this.NoteId);
                    bool success = noteManager.DeleteNote(this.NoteId);

                    if (success)
                    {
                        GotoNotesView();
                    }
                    else
                    {
                        ShowMessageDialog("Failure", NotepadSettings.DeleteNoteFailure, false);
                    }
                }
            };

            DeleteNotePrompt.Show();
        }        

        private void CancelNote_Click(object sender, EventArgs e)
        {
            //goto home page
            GotoNotesView(); 
        }

        protected void ReminderNote_Click(object sender, EventArgs e)
        {
            //set saved reminder details on edit
            if (this.EditedNote != null && this.EditedNote.HasReminder == Visibility.Visible)
            {
                this.ReminderControl.Date = this.EditedNote.ReminderDate;
                this.ReminderControl.Time = this.EditedNote.ReminderTime;
                this.ReminderControl.IsReminderEnabled = true;
            }
            else
            {
                this.ReminderControl.IsReminderEnabled = false;
            }

            ReminderPopup.Show();
        }

        private void EmailNote_Click(object sender, EventArgs e)
        {
            Common.EmailNote(GetCurrentNote());
        }

        private void PinNoteToStart_Click(object sender, EventArgs e)
        {
            Note note = GetCurrentNote();

            if (string.IsNullOrEmpty(this.NoteId))
            {
                note.Title = NotepadSettings.NewNoteTitle;
                note.Content = string.Empty;
            }

            Common.PinNoteToStart(note);
        }

        private void Share_Click(object sender, EventArgs e)
        {
            Common.ShareNote(GetCurrentNote());
        }  

        private void txtNoteTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNoteTitle.Background = new SolidColorBrush(Color.FromArgb(0, 255, 183, 149));
            txtNoteTitle.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void txtNoteContent_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNoteContent.Background = new SolidColorBrush(Color.FromArgb(255, 255, 238, 184));
            txtNoteContent.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            object obj = ApplicationBar.Buttons[0];

            if (this.NavigationContext.QueryString.ContainsKey("noteId"))
            {
                this.NoteId = this.NavigationContext.QueryString["noteId"];
                this.EditedNote = noteManager.GetNote(this.NoteId);

                if (this.EditedNote == null)
                {
                    this.NoteId = string.Empty;
                    txtNoteTitle.Text = NotepadSettings.NewNoteTitle;
                    return;
                }

                txtNoteTitle.Text = this.EditedNote.Title;
                txtNoteContent.Text = this.EditedNote.Content;
                imgReminderClock.Visibility = this.EditedNote.HasReminder;

                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = true;
            }
        }        

    }
}