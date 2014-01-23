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
using System.Windows.Controls.Primitives;

namespace NotepadPlus
{
    public partial class AddOrEditNote : PhoneApplicationPage
    {
        private string NoteId = string.Empty;
        private Note EditedNote = null;
        NoteManager noteManager = new NoteManager();

        MessagePrompt msg = null;

        NoteReminder _reminder = null;

        NoteReminder Reminder
        {
            get
            {
                if (_reminder == null)
                    _reminder = new NoteReminder();

                return _reminder;
            }
        }

        public AddOrEditNote()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AddNote_Loaded);

            
        }

        void msg_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            this.Reminder.IsComplete = e.PopUpResult == PopUpResult.Ok;
        }

        void GotoNotesView()
        {
            this.NavigationService.Navigate(new Uri("/ViewNotes.xaml", UriKind.Relative));            
        }

        void ShowDialog(string title, string message, bool showCancel)
        {
            var msgPrompt = new MessagePrompt()
            {
                Title = title,
                Message = message,
                IsCancelVisible = showCancel                
            };

            msgPrompt.Show();
        }

        void AddNote_Loaded(object sender, RoutedEventArgs e)
        {
            if (msg == null)
            {
                msg = new MessagePrompt();
                msg.Title = "Note Reminder";
                msg.Body = this.Reminder;
                msg.IsCancelVisible = true;
                msg.Completed += new EventHandler<PopUpEventArgs<string, PopUpResult>>(msg_Completed);
            }
            else
            {
                if (this.Reminder.IsComplete == false)
                {
                    msg.Show();
                }
            }

            

            txtNoteTitle.BorderBrush.Opacity = 0;            
            txtNoteContent.BorderBrush.Opacity = 0;
        }        

        private void textBox3_GotFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri("images/AddNoteBackground.jpg", UriKind.Relative));            
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

        private void SaveNote_Click(object sender, EventArgs e)
        {
            Note note = null;

            if (string.IsNullOrEmpty(txtNoteTitle.Text.Trim()))
            {                
                ShowDialog("Warning!", NotepadSettings.EmptyNoteTitle, false);
                return;
            }

            if (string.IsNullOrEmpty(this.NoteId))
            {
                note = new Note()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = txtNoteTitle.Text.Trim(),
                    Content = txtNoteContent.Text.Trim(),
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };
            }
            else
            {
                note = this.EditedNote;

                note.Title = txtNoteTitle.Text.Trim();
                note.Content = txtNoteContent.Text.Trim();
                note.Modified = DateTime.Now;
            }            

            NoteManager noteManager = new NoteManager();
            bool success = noteManager.AddOrUpdateNote(note);

            if (success)
            {
                GotoNotesView();
            }
            else
            {
                ShowDialog("Failure", NotepadSettings.NoteSaveFailure, false);
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

            DeleteNotePrompt.Completed += new EventHandler<PopUpEventArgs<string, PopUpResult>>(DeleteNotePrompt_Completed);
            DeleteNotePrompt.Show();
        }

        void DeleteNotePrompt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
            {
                bool success = noteManager.DeleteNote(this.NoteId);

                if (success)
                {
                    GotoNotesView();
                }
                else
                {
                    ShowDialog("Failure", NotepadSettings.DeleteNoteFailure, false);                    
                }
            }
        }

        private void CancelNote_Click(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        protected void Reminder_Click(object sender, EventArgs e)
        {
            //Microsoft.Phone.Scheduler.ScheduledActionService.
            //inputPrompt1.Visibility = Visibility.Visible;

            //NoteReminder nd = new NoteReminder();
            //nd.top
            //nd.Show();

            //Popup popup = new Popup();
            //popup.Height = 300;
            //popup.Width = 400;
            //popup.VerticalOffset = 100;
            //NoteReminder control = new NoteReminder();
            //popup.Child = control;
            //popup.IsOpen = true;

            //AboutPrompt p = new AboutPrompt();
            //MessagePrompt p = new MessagePrompt(); p.IsOverlayApplied = true;
            //p.Body = new NoteReminder();
            msg.Show();

            //control.btnOK.Click += (s, args) =>
            //{
            //    popup.IsOpen = false;
            //    //this.text.Text = control.tbx.Text;
            //};

            //control.btnCancel.Click += (s, args) =>
            //{
            //    popup.IsOpen = false;
            //};
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            base.OnNavigatedTo(e);

            object obj = ApplicationBar.Buttons[0];

            if (this.NavigationContext.QueryString.ContainsKey("noteId"))
            {           
                this.NoteId = this.NavigationContext.QueryString["noteId"];                
                this.EditedNote = noteManager.GetNote(this.NoteId);

                if (this.EditedNote == null) return;

                txtNoteTitle.Text = this.EditedNote.Title;
                txtNoteContent.Text = this.EditedNote.Content;

                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = true;
            }
        }
        

    }
}