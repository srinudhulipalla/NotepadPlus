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

namespace NotepadPlus
{
    public partial class AddOrEditNote : PhoneApplicationPage
    {
        private string NoteId = string.Empty;
        private Note EditedNote = null;
        NoteManager noteManager = new NoteManager();

        public AddOrEditNote()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AddNote_Loaded);
        }

        void GotoNotesView()
        {
            this.NavigationService.Navigate(new Uri("/ViewNotes.xaml", UriKind.Relative));
        }

        void AddNote_Loaded(object sender, RoutedEventArgs e)
        {
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
                MessageBox.Show(NotepadSettings.NoteSaveFailure, "Failure", MessageBoxButton.OK);
            }
        }

        private void DeleteNote_Click(object sender, EventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show(NotepadSettings.DeleteNoteConfirm, "Delete", MessageBoxButton.OKCancel);

            if (confirm == MessageBoxResult.OK)
            {
                bool success = noteManager.DeleteNote(this.NoteId);

                if (success)
                {
                    GotoNotesView();
                }
                else
                {
                    MessageBox.Show(NotepadSettings.DeleteNoteFailure, "Failure", MessageBoxButton.OK);
                }
            }
        }

        private void CancelNote_Click(object sender, EventArgs e)
        {
            GotoNotesView();
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