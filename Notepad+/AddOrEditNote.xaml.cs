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
        string NoteId = string.Empty;
        bool IsReminderSet = true;

        Note EditedNote = null;        
        MessagePrompt ReminderPopup = null;
        Thickness popupMargin = new Thickness(10, 150, 10, 0);

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

        NoteManager _noteManager = null;
        NoteManager NoteManager
        {
            get
            {
                if (_noteManager == null)
                    _noteManager = new NoteManager();

                return _noteManager;
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
                IsCancelVisible = showCancel,
                Margin = popupMargin
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
                ReminderPopup.IsCancelVisible = false;
                ReminderPopup.Margin = popupMargin;
                //this.ReminderControl.IsReminderEnabled = false;

                UpdateUI();

                ReminderPopup.Completed += (s, args) =>
                {    
                    if (args.PopUpResult == PopUpResult.Ok)
                    {
                        this.IsReminderSet = this.ReminderControl.IsReminderEnabled;
                        imgReminderClock.Visibility = this.IsReminderSet ? Visibility.Visible : Visibility.Collapsed;

                        //if (this.IsReminderSet)
                        //{
                        //    //this.CurrentNote.ReminderDate = this.ReminderControl.Date;
                        //    //this.CurrentNote.ReminderTime = this.ReminderControl.Time;
                        //    //this.CurrentNote.HasReminder = Visibility.Visible;
                        //    imgReminderClock.Visibility = Visibility.Visible;
                        //}
                        //else
                        //{
                        //    //this.CurrentNote.HasReminder = Visibility.Collapsed;
                        //    imgReminderClock.Visibility = Visibility.Collapsed;
                        //}

                        //GetCurrentNote();
                    }
                    else
                    {                        
                        this.ReminderControl.IsReminderEnabled = IsReminderSet;                        
                    }

                    this.ReminderControl.IsCompleted = args.PopUpResult == PopUpResult.Ok;
                };
            }
            else if (!this.ReminderControl.IsCompleted)
            {
                ReminderPopup.Show();
            }
            
            txtNoteTitle.BorderBrush.Opacity = 0;
            txtNoteContent.BorderBrush.Opacity = 0;
        }

        private Note GetCurrentNote()
        {
            bool isNewNote = string.IsNullOrEmpty(this.NoteId);

            if (isNewNote) //new note
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
                //this.CurrentNote = this.EditedNote;
                this.CurrentNote.Title = txtNoteTitle.Text.Trim();
                this.CurrentNote.Content = txtNoteContent.Text.Trim();
                this.CurrentNote.Modified = DateTime.Now;
            }

            if (this.ReminderControl.IsCompleted && this.ReminderControl.IsReminderEnabled)
            {
                this.CurrentNote.ReminderDate = this.ReminderControl.Date;
                this.CurrentNote.ReminderTime = this.ReminderControl.Time;
                this.CurrentNote.HasReminder = Visibility.Visible;
            }
            else
            {
                this.CurrentNote.HasReminder = Visibility.Collapsed;
            }

            return this.CurrentNote;
        }

        private void SaveNote_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNoteTitle.Text.Trim()))
            {
                ShowMessageDialog(NotepadSettings.Warning, NotepadSettings.EmptyNoteTitle, false);
                return;
            }
            else if (txtNoteTitle.Text.Trim().Length > 20)
            {
                ShowMessageDialog(NotepadSettings.Warning, NotepadSettings.NoteTitleExceedLength, false);
                return;
            }

            Note note = GetCurrentNote();            

            //set or delete note reminder
            if (this.ReminderControl.IsCompleted && this.ReminderControl.IsReminderEnabled)
            {    
                NotesReminder.AddOrUpdateReminder(note);
            }
            else
            {                
                NotesReminder.DeleteReminder(note.Id);
            }            

            //save note            
            bool success = this.NoteManager.AddOrUpdateNote(note);
            note = null;

            if (success)
            {
                GotoNotesView();
            }
            else
            {
                ShowMessageDialog(NotepadSettings.Fail, NotepadSettings.NoteSaveFailure, false);
            }
        }

        private void DeleteNote_Click(object sender, EventArgs e)
        {
            var DeleteNotePrompt = new MessagePrompt()
            {
                Title = NotepadSettings.Delete,
                Message = NotepadSettings.DeleteNoteConfirm,
                IsCancelVisible = true,
                Margin = popupMargin
            };            

            DeleteNotePrompt.Completed += (s, args) => //delete note on confirm
            {
                if (args.PopUpResult == PopUpResult.Ok)
                {
                    NotesReminder.DeleteReminder(this.NoteId);
                    bool success = this.NoteManager.DeleteNote(this.NoteId);

                    if (success)
                    {
                        GotoNotesView();
                    }
                    else
                    {
                        ShowMessageDialog(NotepadSettings.Fail, NotepadSettings.DeleteNoteFailure, false);
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
            //set current note/saved note reminder details to reminder popup            
            //if (this.CurrentNote.HasReminder == Visibility.Visible)
            //{
            //    this.ReminderControl.Date = this.CurrentNote.ReminderDate;
            //    this.ReminderControl.Time = this.CurrentNote.ReminderTime;
            //    this.ReminderControl.IsReminderEnabled = true;
            //}            

            //UpdateUI();

            this.ReminderControl.IsCompleted = false;
            ReminderPopup.Show();
        }

        private void EmailNote_Click(object sender, EventArgs e)
        {
            Common.SendEmail(GetCurrentNote());
        }

        private void PinNoteToStart_Click(object sender, EventArgs e)
        {
            Note note = GetCurrentNote();
            
            //for new note
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
                this.EditedNote = this.NoteManager.GetNote(this.NoteId);

                if (this.EditedNote == null)
                {
                    this.NoteId = string.Empty;                    
                    return;
                }

                this.CurrentNote = this.EditedNote;                
                this.EditedNote = null;

                //txtNoteTitle.Text = this.EditedNote.Title;
                //txtNoteContent.Text = this.EditedNote.Content;
                //imgReminderClock.Visibility = this.EditedNote.HasReminder;

                //this.CurrentNote.ReminderDate =this.ReminderControl.Date = this.EditedNote.ReminderDate;
                //this.CurrentNote.ReminderTime = this.ReminderControl.Time = this.EditedNote.ReminderTime;
                //this.CurrentNote.HasReminder = this.EditedNote.HasReminder;
                //this.ReminderControl.IsCompleted = this.ReminderControl.IsReminderEnabled = (this.CurrentNote.HasReminder == Visibility.Visible) ? true : false;

                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = true;
            }
        }

        void UpdateUI()
        {
            if (string.IsNullOrEmpty(this.CurrentNote.Title))
            {
                txtNoteTitle.Text = NotepadSettings.NewNoteTitle;
                txtNoteContent.Text = string.Empty;
            }
            else
            {
                txtNoteTitle.Text = this.CurrentNote.Title;
                txtNoteContent.Text = this.CurrentNote.Content;
            }

            imgReminderClock.Visibility = this.CurrentNote.HasReminder;

            this.ReminderControl.IsReminderEnabled = this.ReminderControl.IsCompleted = (this.CurrentNote.HasReminder == Visibility.Visible) ? true : false;
            this.ReminderControl.Date = this.CurrentNote.ReminderDate;
            this.ReminderControl.Time = this.CurrentNote.ReminderTime;
        }

    }
}