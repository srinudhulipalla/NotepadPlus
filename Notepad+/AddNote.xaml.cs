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

namespace NotepadPlus
{
    public partial class AddNote : PhoneApplicationPage
    {
        public AddNote()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(AddNote_Loaded);

           
        }

        void AddNote_Loaded(object sender, RoutedEventArgs e)
        {
            txtNoteTitle.BorderBrush.Opacity = 0;
            //textBox3.BorderBrush.Opacity = 0;


            txtNoteContent.BorderBrush.Opacity = 0;
        }

        

        private void textBox3_GotFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush s = new ImageBrush();
            s.ImageSource = new BitmapImage(new Uri("images/AddNoteBackground.jpg", UriKind.Relative));
            //textBox3.Background = s;
        }

        private void txtNoteContent_GotFocus(object sender, RoutedEventArgs e)
        {            
            txtNoteContent.Background = new SolidColorBrush(Color.FromArgb(255, 255, 238, 184));
            txtNoteContent.BorderThickness = new Thickness(0, 0, 0, 0);            
        }

        private void txtNoteTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNoteTitle.Background = new SolidColorBrush(Color.FromArgb(0, 255, 183, 149));
            txtNoteTitle.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            GotoViewNotes();
        }

        void GotoViewNotes()
        {
            this.NavigationService.Navigate(new Uri("/ViewNotes.xaml", UriKind.Relative));
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Note note = new Note()
            {
                Id = Guid.NewGuid().ToString(),
                Title = txtNoteTitle.Text.Trim(),
                Content = txtNoteContent.Text.Trim(),
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            NoteManager noteManager = new NoteManager();
            bool success = noteManager.AddOrUpdateNote(note);

            if (success)
            {
                GotoViewNotes();
            }
            else
            {
                MessageBox.Show("Unable to save the note. Please try again.", "Failure", MessageBoxButton.OK);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //e
            base.OnNavigatedTo(e);
        }

        

        

    }
}